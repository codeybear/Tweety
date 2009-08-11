using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Core;

namespace Forms
{
    public partial class MainForm : Form
    {
        System.ComponentModel.BackgroundWorker BackgroundWorker = new System.ComponentModel.BackgroundWorker();
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
            if (SettingHelper.UserName == "") {
                using (SettingsForm SettingsForm = new SettingsForm()) {
                    SettingsForm.ShowDialog();
                }

                if (SettingHelper.UserName == "")
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
            BackgroundWorker_GetFriendsTimeLine();
        }

        private void btnUserInfo_Click(object sender, EventArgs e) {
            Result user = Twitter.GetUserInfo(SettingHelper.UserName);
            picProfileImage.Image = user.ProfileImage;
        }

        private void btnFriendsTimeline_Click(object sender, EventArgs e) {
            BackgroundWorker_GetFriendsTimeLine();
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

        //--------------------------------------------------------------
        //  Background worker for friends timeline
        //--------------------------------------------------------------
        void BackgroundWorker_GetFriendsTimeLine() {
            if (!BackgroundWorker.IsBusy)
                BackgroundWorker.RunWorkerAsync();
        }

        void BackgroundWorker_GetFriendsTimeLine(object sender, System.ComponentModel.DoWorkEventArgs e) {
            try {
                e.Result = Twitter.GetFriendsTimeLine(SettingHelper.UserName, SettingHelper.Password);
                Result UserInfo = Twitter.GetUserInfo(SettingHelper.UserName);
                Utility.AccessInvoke(this, () => rchStatus.Text = UserInfo.Text);
                picProfileImage.Image = Twitter.GetUserProfileImage(UserInfo.ProfileImageUrl);
            }
            catch (Exception ex) {
                Utility.AccessInvoke(this, () => ShowMessage(true, ex.Message));
            }
        }

        void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
            if (e.Result != null) {
                BindResultsToTable((List<Result>)e.Result);
                // TODO tidy this up
                if (btnMessage.Visible) btnMessage_Click(null, null);
            }
        }

        //--------------------------------------------------------------
        // Support Methods
        //--------------------------------------------------------------
        /// <summary> Check for new tweets, and display if there are any </summary>
        void HandleResults(List<Result> ResultList) {
            // Check to see if there are new tweets
            Int64 lLastId = Convert.ToInt64(ResultList[0].ID);

            if (_lLastId != lLastId && _lLastId !=0) {
                    BindResultsToTable(ResultList);

                    AlertForm Alert = new Forms.AlertForm("New tweets have arrived");
                    Alert.LinkClicked += () => this.Activate();
                }

                _lLastId = lLastId;
        }

        /// <summary> Setup for the form to get tweets </summary>
        void Setup() {
            // Get any friends images that have been stored
            Twitter.LoadImageCache(System.IO.Path.Combine(Application.StartupPath, "ImageCache.txt"));

            // Setup timer to get friends timeline
            StatusTimer = new Timer();
            StatusTimer.Tick += new EventHandler(StatusTimer_Tick);
            StatusTimer.Interval = 1000 * 120;
            StatusTimer.Start();

            // Get friends timeline
            BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(BackgroundWorker_GetFriendsTimeLine);
            BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            BackgroundWorker_GetFriendsTimeLine();
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
            btnMessage.Visible = bShow;
            btnMessage.Text = sMessage;

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

        private void rchStatus_Click(object sender, EventArgs e) {
            UpdateForm Update = new UpdateForm(rchStatus.Text);
            Update.StatusChanged += Status => rchStatus.Text = Status;
            Update.ShowDialog();
        }
    }
}
