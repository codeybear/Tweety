using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Twitter
{
    public partial class MainForm : Form
    {
        System.ComponentModel.BackgroundWorker BackgroundWorker = new System.ComponentModel.BackgroundWorker();
        Timer StatusTimer;

        public MainForm() {
            InitializeComponent();
        }

        //--------------------------------------------------------------
        // Events 
        //--------------------------------------------------------------

        private void MainForm_Load(object sender, EventArgs e) {
            // Get any friends images that have been stored
            TwitterManager.LoadImageCache(System.IO.Path.Combine(Application.StartupPath, "ImageCache.txt"));

            // Setup timer to get friends timeline
            StatusTimer = new Timer();
            StatusTimer.Tick += new EventHandler(StatusTimer_Tick);
            StatusTimer.Interval = 1000 * 60;
            StatusTimer.Start();

            Result UserInfo = TwitterManager.GetUserInfo(SettingHelper.UserName);
            picProfileImage.Image = UserInfo.ProfileImage;

            // Get friends timeline
            BackgroundWorker_SetupFriendsTimeLine();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            TwitterManager.SaveImageCache(System.IO.Path.Combine(Application.StartupPath, "ImageCache.txt"));
        }

        void StatusTimer_Tick(object sender, EventArgs e) {
            BackgroundWorker_SetupFriendsTimeLine();
        }

        private void btnUserInfo_Click(object sender, EventArgs e) {
            Result user = TwitterManager.GetUserInfo(SettingHelper.UserName);
            picProfileImage.Image = user.ProfileImage;
        }

        private void btnFriendsTimeline_Click(object sender, EventArgs e) {
            BackgroundWorker_SetupFriendsTimeLine();
        }

        //--------------------------------------------------------------
        //  Background worker for friends timeline
        //--------------------------------------------------------------

        void BackgroundWorker_SetupFriendsTimeLine() {
            if (!BackgroundWorker.IsBusy) {
                BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(BackgroundWorker_GetFriendsTimeLine);
                BackgroundWorker.RunWorkerCompleted += (sender, e) => BindResultsToGrid((List<Result>)e.Result);
                BackgroundWorker.RunWorkerAsync();
            }
        }

        void BackgroundWorker_GetFriendsTimeLine(object sender, System.ComponentModel.DoWorkEventArgs e) {
            try {
                e.Result = TwitterManager.GetFriendsTimeLine(SettingHelper.UserName, SettingHelper.Password);
            }
            catch (System.Net.WebException ex) {
                btnMessage.Text = ex.Response.Headers["status"];
                
                e.Cancel = true;
            }
        }

        //--------------------------------------------------------------
        // Support Methods
        //--------------------------------------------------------------

        void BindResultsToGrid(List<Result> ResultList) {
            grdFriendStatus.Rows.Clear();

            foreach (Result UserInfo in ResultList)
                grdFriendStatus.Rows.Add(new object[] { UserInfo.ProfileImage, UserInfo.Text });
        }

        private void picProfileImage_Click(object sender, EventArgs e) {
            SettingsForm SettingsForm = new SettingsForm();
            SettingsForm.ShowDialog();
        }
    }
}
