using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Core;

namespace Forms
{
    public partial class MainForm : Form
    {
        BackgroundWorker bgwFriendsTimeLine = new BackgroundWorker();
        BackgroundWorker bgwMyStatus = new BackgroundWorker();

        Timer StatusTimer;
        Int64 _lLastId;

        public MainForm() {
            InitializeComponent();
        }

        //--------------------------------------------------------------
        // Events 
        //--------------------------------------------------------------

        private void MainForm_Load(object sender, EventArgs e) {
            // Check for user settings before settings up form
            if (String.IsNullOrEmpty(SettingHelper.UserName)) {
                using (SettingsForm SettingsForm = new SettingsForm()) {
                    SettingsForm.ShowDialog();
                }

            	if (String.IsNullOrEmpty(SettingHelper.UserName))
                    Close();
                else
                    Setup();
            }
            else
                Setup();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            Twitter.SaveImageCache(System.IO.Path.Combine(Application.StartupPath, "ImageCache.txt"));
            SettingHelper.Save();
        }

        void StatusTimer_Tick(object sender, EventArgs e) {
            bgwFriendsTimeLine_Start();
            bgwMyStatus_Start();
        }

        private void btnFriendsTimeline_Click(object sender, EventArgs e) {
            bgwFriendsTimeLine_Start();
        }

        void rchUpdate_LinkClicked(object sender, LinkClickedEventArgs e) {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = e.LinkText;
            p.Start();
        }

        private void picProfileImage_Click(object sender, EventArgs e) {
            using (SettingsForm SettingsForm = new SettingsForm())
                SettingsForm.ShowDialog();
        }

        private void btnMessage_Click(object sender, EventArgs e) {
            ShowMessage(false, "");
        }

        private void rchStatus_Click(object sender, EventArgs e) {
            UpdateForm Update = new UpdateForm(rchStatus.Text);
            Update.StatusChanged += Status => rchStatus.Text = Status;
            Update.ShowDialog();
        }

        void AlertLink_Clicked() {
            this.Activate();
            rchStatus.Text += "Link Clicked - " + DateTime.Now;
        }

        //--------------------------------------------------------------
        //  Background worker for friends timeline
        //--------------------------------------------------------------
        void bgwFriendsTimeLine_Start() {
            if (!bgwFriendsTimeLine.IsBusy) {
                this.Text = "Tweety - Looking for tweets...";
                bgwFriendsTimeLine.RunWorkerAsync();
            }
        }

        void bgwFriendsTimeLine_DoWork(object sender, DoWorkEventArgs e) {
            try {   
                e.Result = Twitter.GetFriendsTimeline(SettingHelper.UserName, SettingHelper.Password);
            }
            catch (Exception ex) {
                Utility.AccessInvoke(this, () => ShowMessage(true, ex.Message));
            }
        }

        void bgwFriendsTimeLine_Completed(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Result != null) {
                HandleResults((List<Result>)e.Result);
                ShowMessage(false, "");
                this.Text = "Tweety";
            }
        }

        //--------------------------------------------------------------
        //  Background worker for my status
        //--------------------------------------------------------------
        void bgwMyStatus_Start() {
            if (!bgwMyStatus.IsBusy)
                bgwMyStatus.RunWorkerAsync();
        }

        void bgwMyStatus_DoWork(object sender, DoWorkEventArgs e) {
            try {
                e.Result = Twitter.GetUserInfo(SettingHelper.UserName);
            }
            catch (Exception ex) {
                Utility.AccessInvoke(this, () => ShowMessage(true, ex.Message));
            }
        }

        void bgwMyStatus_Completed(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Result != null) {
                Result MyInfo = (Result)e.Result;
                rchStatus.Text = MyInfo.Text;
                picProfileImage.Image = Twitter.GetUserProfileImage(MyInfo.ProfileImageUrl);

                rchStatus.Click += new EventHandler(rchStatus_Click);
            }
        }

        //--------------------------------------------------------------
        // Support Methods
        //--------------------------------------------------------------
        /// <summary> Check for new tweets, and display if there are any </summary>
        void HandleResults(List<Result> ResultList) {
            // Check to see if there are new tweets
            Int64 lLastId = Convert.ToInt64(ResultList[0].ID);

            if (_lLastId != lLastId) {
                BindResultsToTable(ResultList);

                if (_lLastId != 0) {
                    AlertForm Alert = new Forms.AlertForm(SettingHelper.MessageNewTweets);
                    Alert.LinkClicked += new Action(AlertLink_Clicked);
                }
            }

            _lLastId = lLastId;
        }

        /// <summary> Setup for the form to get tweets</summary>
        void Setup() {
            // Get any friends images that have been stored
            Twitter.LoadImageCache(System.IO.Path.Combine(Application.StartupPath, "ImageCache.txt"));

            // Setup timer to get friends timeline
            StatusTimer = new Timer();
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

        /// <summary> Display the list of tweets inside a TableLayoutPanel control </summary>
        void BindResultsToTable(List<Result> ResultList) {
            tblTweets.RowCount = 1;
            tblTweets.Controls.Clear();

            for (int iCount = 0; iCount < ResultList.Count; iCount++) {
                Result UserInfo = ResultList[iCount];

                // Show the profile image
                PictureBox picProfile = new PictureBox();
                picProfile.Margin = new Padding(2, 0, 4, 1);
                picProfile.Image = UserInfo.ProfileImage;

                // Show the status text
                RichTextBox rchUpdate = new RichTextBox();
                rchUpdate.Anchor = (AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom);
                rchUpdate.BorderStyle = BorderStyle.None;
                rchUpdate.Margin = new Padding(0);
                rchUpdate.Height = 52;
                rchUpdate.Text = UserInfo.Text;
                rchUpdate.ReadOnly = true;
                rchUpdate.BackColor = Color.WhiteSmoke;
                rchUpdate.Cursor = Cursors.Arrow;
                rchUpdate.ReadOnly = true;

                rchUpdate.LinkClicked += new LinkClickedEventHandler(rchUpdate_LinkClicked);

                tblTweets.Controls.Add(picProfile, 0, tblTweets.RowCount - 1);
                tblTweets.Controls.Add(rchUpdate, 1, tblTweets.RowCount - 1);
                tblTweets.RowCount += 1;
                tblTweets.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
        }

        // Show/hide the message button
        private void ShowMessage(bool bShow, string sMessage) {
            btnMessage.Text = sMessage;

            // Dont animate again if the button is already visible
            if (btnMessage.Visible == true && bShow == true)
                return;

            // Dont animate again if the button is already not visible
            if (btnMessage.Visible == false && bShow == false)
                return;

            btnMessage.Visible = bShow;

            for (int iTop = 0; iTop <= 24; iTop += 2) {
                System.Threading.Thread.Sleep(25);

                if (bShow) {
                    tblTweets.Top += 2;
                    tblTweets.Height -= 2;
                }
                else {
                    tblTweets.Top -= 2;
                    tblTweets.Height += 2;
                }

                this.Refresh();
            }
        }
    }
}
