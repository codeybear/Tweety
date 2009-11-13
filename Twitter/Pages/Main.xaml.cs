using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using Core;

namespace Pages {
    public partial class MainWindow : Window {
        BackgroundWorker bgwFriendsTimeLine = new BackgroundWorker();
        BackgroundWorker bgwMyStatus = new BackgroundWorker();
        System.Windows.Forms.Timer StatusTimer;

        // Keep track of the last tweet id to check to new tweets
        Int64 _lLastId;

        public MainWindow() {
            InitializeComponent();
        }

        #region Events

        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            bgwFriendsTimeLine_Start();
        }
		
		private void btnSettings_Click(object sender, RoutedEventArgs e) {
		    Settings SettingsForm = new Settings();
            SettingsForm.ShowDialog();
		}

        private void Hyperlink_RequestNavigate(object sender, System.Windows.RoutedEventArgs e) {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            Hyperlink link = (Hyperlink)sender;
            p.StartInfo.FileName = link.NavigateUri.AbsoluteUri;
            p.Start();
        }

        private void txtStatus_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {

        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            // Check for user settings before settings up form
            if (String.IsNullOrEmpty(SettingHelper.UserName)) {
                Settings SettingsForm = new Settings();
                SettingsForm.ShowDialog();

                if (String.IsNullOrEmpty(SettingHelper.UserName))
                    Close();
                else
                    Setup();
            }
            else
                Setup();
        }

        void StatusTimer_Tick(object sender, EventArgs e)
        {
            bgwFriendsTimeLine_Start();
            bgwMyStatus_Start();
        }

        #endregion

        #region BackgroundWorker Methods

        // Friends Timeline worker methods

        void bgwFriendsTimeLine_Start() {
            if (!bgwFriendsTimeLine.IsBusy) {
                this.Title = "Tweety - Looking for tweets...";
                bgwFriendsTimeLine.RunWorkerAsync();
            }
        }

        void bgwFriendsTimeLine_DoWork(object sender, DoWorkEventArgs e) {
            e.Result = Twitter.GetFriendsTimeline(SettingHelper.UserName, SettingHelper.Password);
        }

        void bgwFriendsTimeLine_Completed(object sender, RunWorkerCompletedEventArgs e) {
            this.Title = "Tweety";

            if (e.Result != null) {
                HandleResults((List<Result>)e.Result);
                // TODO sort out error button
                //ShowMessage(false, "");
            }
        }

        // My Status worker methods
        void bgwMyStatus_Start() {
            if (!bgwMyStatus.IsBusy)
                bgwMyStatus.RunWorkerAsync();
        }

        void bgwMyStatus_DoWork(object sender, DoWorkEventArgs e) {
            e.Result = Twitter.GetUserInfo(SettingHelper.UserName);
        }

        void bgwMyStatus_Completed(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Result != null) {
                Result MyInfo = (Result)e.Result;
                txtStatus.Text = MyInfo.Text;
                imgProfile.Source = new BitmapImage(new Uri(MyInfo.ProfileImageUrl));
            }
        }


        #endregion

        #region Support Methods

        /// <summary> Display list of tweets inside the Grid control </summary>
        private void AddResultsToGrid(List<Result> ResultList) {
            grdTweets.RowDefinitions.Clear();
            grdTweets.Children.Clear();

            foreach (Result Status in ResultList) {
                grdTweets.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                // Create the text for grid
                TextBlock TestBlock = new TextBlock();
                TestBlock.Margin = new Thickness(4);
                TestBlock.TextWrapping = TextWrapping.Wrap;
                TestBlock.Inlines.AddRange(WPFHelper.CreateInlineTextWithLinks(Status.Text, Hyperlink_RequestNavigate));
                Grid.SetColumn(TestBlock, 1);
                Grid.SetRow(TestBlock, grdTweets.RowDefinitions.Count - 1);
                grdTweets.Children.Add(TestBlock);

                // Create the profile image for grid
                Image ProfileImage = new Image();
                ProfileImage.Source = new BitmapImage(new Uri(Status.ProfileImageUrl));
                Grid.SetColumn(ProfileImage, 0);
                Grid.SetRow(ProfileImage, grdTweets.RowDefinitions.Count - 1);
                grdTweets.Children.Add(ProfileImage);
            }
        }

        /// <summary> Setup for the page to get tweets</summary>
        private void Setup() {
            // Setup timer to get friends timeline
            StatusTimer = new System.Windows.Forms.Timer();
            StatusTimer.Tick += new EventHandler(StatusTimer_Tick);
            StatusTimer.Interval = 1000 * 120;
            StatusTimer.Start();

            // Get friends timeline
            bgwFriendsTimeLine.DoWork += new DoWorkEventHandler(bgwFriendsTimeLine_DoWork);
            bgwFriendsTimeLine.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwFriendsTimeLine_Completed);
            bgwFriendsTimeLine_Start();

            // Get my details
            bgwMyStatus.DoWork += new DoWorkEventHandler(bgwMyStatus_DoWork);
            bgwMyStatus.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwMyStatus_Completed);
            bgwMyStatus_Start();
        }

        /// <summary> Check for new tweets, and display if there are any </summary>
        void HandleResults(List<Result> ResultList) {
            // Check to see if there are new tweets
            Int64 lLastId = Convert.ToInt64(ResultList[0].ID);

            if (_lLastId != lLastId) {
                AddResultsToGrid(ResultList);

                if (_lLastId != 0) {
                    Alert Alert = new Alert(SettingHelper.MessageNewTweets, SettingHelper.TweetyIconUri, () => this.Show());
                }
            }

            _lLastId = lLastId;
        }

        #endregion
    }
}
