using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using Tweety.Core;
using TweetSharp;


namespace Pages
{
    public partial class MainWindow : Window, IDisposable
    {
        BackgroundWorker _bgwFriendsTimeLine = new BackgroundWorker();
        BackgroundWorker _bgwMyStatus = new BackgroundWorker();
        System.Windows.Forms.NotifyIcon _NotifyIcon = new System.Windows.Forms.NotifyIcon();
        System.Windows.Forms.Timer _StatusTimer;

        // Keep track of the last tweet id to check to new tweets
        Int64 _lLastId;

        // Keep track of the original status text when status is being updated
        string _OldStatusText;

        public MainWindow() {
            InitializeComponent();

            SetupNotifyIcon();

            // Setup global error handler
            App.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        #region Events

        void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
            // Display and animate the error message button
            string sMessage;

            if (e.Exception.InnerException == null)
                sMessage = e.Exception.Message;
            else
                sMessage = e.Exception.InnerException.Message;

            btnError.Content = sMessage;
            btnError.ToolTip = sMessage;

            if (btnError.Height == 0) {
                var sb = (System.Windows.Media.Animation.Storyboard)this.FindResource("DisplayError");
                sb.Begin();
            }

            e.Handled = true;
        }

        private void btnError_Click(object sender, RoutedEventArgs e) {
            var sb = (System.Windows.Media.Animation.Storyboard)this.FindResource("HideError");
            sb.Begin();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            bgwFriendsTimeLine_Start();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e) {
            string OriginalToken = SettingHelper.Token;
            Settings SettingsForm = new Settings();
            SettingsForm.ShowDialog();

            // If the settings have been modified then setup again
            if (OriginalToken != SettingHelper.Token) {
                bgwFriendsTimeLine_Start();
                bgwMyStatus_Start();
            }
        }

        private void txtStatus_TextChanged(object sender, TextChangedEventArgs e) {
            UpdateStatusButtons(true);
            CheckLength();
            _StatusTimer.Stop();
        }

        private void btnUpdateStatus_Click(object sender, RoutedEventArgs e) {
            var service = Ioc.Create<TwitterService>();
            service.SendTweet(new SendTweetOptions() { Status = txtStatus.Text });
            UpdateStatusButtons(false);
            _StatusTimer.Start();
        }

        private void btnCancelUpdate_Click(object sender, RoutedEventArgs e) {
            UpdateStatusButtons(false);

            txtStatus.TextChanged -= txtStatus_TextChanged;
            txtStatus.Text = _OldStatusText;
            txtStatus.TextChanged += txtStatus_TextChanged;

            _StatusTimer.Start();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.RoutedEventArgs e) {
            using (System.Diagnostics.Process p = new System.Diagnostics.Process()) {
                Hyperlink link = (Hyperlink)sender;
                p.StartInfo.FileName = ((Run)link.Inlines.FirstInline).Text;
                p.Start();
            }
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            // Check for user settings before setting up form
            if (String.IsNullOrEmpty(SettingHelper.Token)) {
                Settings SettingsForm = new Settings();
                SettingsForm.ShowDialog();

                if (String.IsNullOrEmpty(SettingHelper.Token))
                    Close();
                else
                    Setup();
            }
            else
                Setup();
        }

        private void window_Closing(object sender, CancelEventArgs e) {
            this.Dispose();
        }

        private void window_StateChanged(object sender, EventArgs e) {
            if (this.WindowState == WindowState.Minimized)
                this.Hide();
        }

        void StatusTimer_Tick(object sender, EventArgs e) {
            bgwFriendsTimeLine_Start();
            bgwMyStatus_Start();
        }

        private void txtStatus_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {

        }

        #endregion

        #region BackgroundWorker Methods

        // Friends Timeline worker methods

        void bgwFriendsTimeLine_Start() {
            if (!_bgwFriendsTimeLine.IsBusy) {
                this.Title = "Tweety - Looking for tweets...";
                _bgwFriendsTimeLine.RunWorkerAsync();
            }
        }

        void bgwFriendsTimeLine_DoWork(object sender, DoWorkEventArgs e) {
            var service = Ioc.Create<TwitterService>();
            e.Result = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions() { Count = Twitter.NumberOfTweets });
        }

        void bgwFriendsTimeLine_Completed(object sender, RunWorkerCompletedEventArgs e) {
            this.Title = "Tweety";

            if (e.Result != null) {
                HandleResults((List<TwitterStatus>)e.Result);

                if (btnError.Height > 0) {
                    var sb = (System.Windows.Media.Animation.Storyboard)this.FindResource("HideError");
                    sb.Begin();
                }
            }
        }

        // My Status worker methods
        void bgwMyStatus_Start() {
            if (!_bgwMyStatus.IsBusy)
                _bgwMyStatus.RunWorkerAsync();
        }

        void bgwMyStatus_DoWork(object sender, DoWorkEventArgs e) {
            var service = Ioc.Create<TwitterService>();
            e.Result = service.VerifyCredentials(new VerifyCredentialsOptions());
        }

        void bgwMyStatus_Completed(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Result != null) {
                UpdateStatusText((TwitterUser)e.Result);
            }
        }

        #endregion

        #region Support Methods

        /// <summary> Display list of tweets inside the Grid control </summary>
        private void AddResultsToGrid(List<TwitterStatus> StatusList) {
            grdTweets.RowDefinitions.Clear();
            grdTweets.Children.Clear();

            foreach (TwitterStatus Status in StatusList) {
                grdTweets.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                // Create the text for grid
                TextBlock TextBlock = new TextBlock();
                TextBlock.Margin = new Thickness(4);
                TextBlock.TextWrapping = TextWrapping.Wrap;

                if (Status.RetweetedStatus != null) {
                    TextBlock.Inlines.Add(new Italic(new Run(Environment.NewLine +
                                                             "Retweeted by " +
                                                             Status.User.ScreenName +
                                                             Environment.NewLine)));
                }

                TextBlock.Inlines.AddRange(WPFHelper.CreateInlineTextWithLinks(Status.Text, Hyperlink_RequestNavigate));
                string displayDate = Twitter.ConvertTwitterDateDisplay(Status.CreatedDate);
                TextBlock.Inlines.Add(new Italic(new Run(Environment.NewLine + displayDate)));
                Grid.SetColumn(TextBlock, 1);
                Grid.SetRow(TextBlock, grdTweets.RowDefinitions.Count - 1);
                grdTweets.Children.Add(TextBlock);

                Image ProfileImage = new Image();
                ProfileImage.Source = WPFHelper.CreateImage(Status.User.ProfileImageUrl);
                ProfileImage.ToolTip = Status.User.Name;
                TwitterUser user = Status.User;

                ProfileImage.MouseDown += (o, e) => {
                    Profile ProfileWindow = new Profile(user);
                    ProfileWindow.ShowDialog();
                };

                Grid.SetColumn(ProfileImage, 0);
                Grid.SetRow(ProfileImage, grdTweets.RowDefinitions.Count - 1);
                grdTweets.Children.Add(ProfileImage);
            }
        }

        private void UpdateStatusText(TwitterUser user) {
            // Store original text in case it gets modified
            _OldStatusText = user.Status.Text;

            txtStatus.TextChanged -= txtStatus_TextChanged;
            txtStatus.Text = user.Status.Text;
            txtStatus.TextChanged += txtStatus_TextChanged;
            imgProfile.Source = new BitmapImage(new Uri(user.ProfileImageUrl));
        }

        /// <summary> Setup for the page to get tweets</summary>
        private void Setup() {
            // Setup timer to get friends timeline
            _StatusTimer = new System.Windows.Forms.Timer();
            _StatusTimer.Tick += StatusTimer_Tick;
            _StatusTimer.Interval = 1000 * 120;
            _StatusTimer.Start();

            // Get friends timeline
            _bgwFriendsTimeLine.DoWork += bgwFriendsTimeLine_DoWork;
            _bgwFriendsTimeLine.RunWorkerCompleted += bgwFriendsTimeLine_Completed;
            bgwFriendsTimeLine_Start();

            // Get my details
            _bgwMyStatus.DoWork += bgwMyStatus_DoWork;
            _bgwMyStatus.RunWorkerCompleted += bgwMyStatus_Completed;
            bgwMyStatus_Start();
        }

        /// <summary> Check for new tweets, and display if there are any </summary>
        void HandleResults(List<TwitterStatus> ResultList) {
            // Check to see if there are new tweets
            Int64 lLastId = Convert.ToInt64(ResultList[0].Id);

            if (_lLastId != lLastId) {
                AddResultsToGrid(ResultList);

                // New tweets have been found display the alert form
                if (_lLastId != 0) {
                    Alert Alert = new Alert(SettingHelper.MessageNewTweets, SettingHelper.TweetyIconUriString, RestoreWindow);
                }
            }

            _lLastId = lLastId;
        }

        private void RestoreWindow() {
            if (this.WindowState == WindowState.Minimized) {
                this.Show();
                this.WindowState = WindowState.Normal;
            }
            else {
                this.WindowState = WindowState.Normal;
                NativeMethods.ShowWindowTopMost(window);
            }
        }

        private void SetupNotifyIcon() {
            _NotifyIcon.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Uri TweetyUri = new Uri(SettingHelper.TweetyIconUriString, UriKind.Relative);
            System.IO.Stream IconStream = Application.GetResourceStream(TweetyUri).Stream;
            _NotifyIcon.Icon = new System.Drawing.Icon(IconStream);
            _NotifyIcon.Click += new EventHandler((o, e) => RestoreWindow());
            _NotifyIcon.Visible = true;
        }

        private void UpdateStatusButtons(bool SetVisible) {
            if (SetVisible) {
                panelUpdateStatus.Visibility = Visibility.Visible;
                btnSettings.Visibility = Visibility.Hidden;
            }
            else {
                panelUpdateStatus.Visibility = Visibility.Hidden;
                btnSettings.Visibility = Visibility.Visible;
            }
        }

        private void CheckLength() {
                lblLetterCount.Content = txtStatus.Text.Length;

            if (txtStatus.Text.Length > Twitter.TextLength) {
                btnUpdateStatus.IsEnabled = false;
                lblLetterCount.Foreground = System.Windows.Media.Brushes.Red;
            }
            else {
                btnUpdateStatus.IsEnabled = true;
                lblLetterCount.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        public void Dispose() {
            if(_StatusTimer != null)_StatusTimer.Dispose();
            if (_NotifyIcon != null) _NotifyIcon.Dispose();
            if (_bgwMyStatus != null) _bgwMyStatus.Dispose();
            if (_bgwFriendsTimeLine != null) _bgwFriendsTimeLine.Dispose();
        }

        #endregion

    }
}
