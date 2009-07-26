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

        // Allow access to user image from other forms
        public Bitmap UserImage { set { picProfileImage.Image = value; } }

        //--------------------------------------------------------------
        // Events 
        //--------------------------------------------------------------

        private void MainForm_Load(object sender, EventArgs e) {
            // Check for user settings before settings up form
            if (SettingHelper.UserName == "") {
                SettingsForm SettingsForm = new SettingsForm(this);
                SettingsForm.ShowDialog();

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
            notifyIcon.Visible = false;
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

        //--------------------------------------------------------------
        //  Background worker for friends timeline
        //--------------------------------------------------------------

        void BackgroundWorker_GetFriendsTimeLine() {
            if (!BackgroundWorker.IsBusy)
                BackgroundWorker.RunWorkerAsync();
        }

        void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
            if (e.Result != null) {
                BindResultsToGrid((List<Result>)e.Result);
                if (btnMessage.Visible) btnMessage_Click(null, null);
            }
        }

        void BackgroundWorker_GetFriendsTimeLine(object sender, System.ComponentModel.DoWorkEventArgs e) {
            try {
                e.Result = Twitter.GetFriendsTimeLine(SettingHelper.UserName, SettingHelper.Password);
            }
            catch (Exception ex) {
                //string sMessage;

                //if (ex.Response != null)
                //    sMessage = ex.Response.Headers["status"];
                //else
                //    sMessage = ex.Message;

                Utility.AccessInvoke(this, () => ShowMessage(ex.Message));
            }
        }

        //--------------------------------------------------------------
        // Support Methods
        //--------------------------------------------------------------

        /// <summary> Setup for the form to get tweets </summary>
        void Setup() {
            // Get any friends images that have been stored
            Twitter.LoadImageCache(System.IO.Path.Combine(Application.StartupPath, "ImageCache.txt"));

            // Setup timer to get friends timeline
            StatusTimer = new Timer();
            StatusTimer.Tick += new EventHandler(StatusTimer_Tick);
            StatusTimer.Interval = 1000 * 120;
            StatusTimer.Start();

            // Get user's profile image
            picProfileImage.Image = Twitter.GetUserProfileImageFromCache(SettingHelper.ProfileImageURL);

            // Get friends timeline
            BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(BackgroundWorker_GetFriendsTimeLine);
            BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            BackgroundWorker_GetFriendsTimeLine();
        }

        void BindResultsToGrid(List<Result> ResultList) {
            grdFriendStatus.Rows.Clear();

            foreach (Result UserInfo in ResultList) {
                grdFriendStatus.Rows.Add(new object[] { UserInfo.ProfileImage, UserInfo.Text });
            }

            // Check to see if there are new tweets
            Int64 lLastId = Convert.ToInt64(ResultList[0].ID);

            if (_lLastId != lLastId) {
                if(_lLastId != 0)
                    ShowAlert(new AlertForm("New tweets have arrived", this));

                _lLastId = lLastId;
            }
        }

        private void picProfileImage_Click(object sender, EventArgs e) {
            SettingsForm SettingsForm = new SettingsForm(this);
            SettingsForm.ShowDialog();
        }

        private void btnMessage_Click(object sender, EventArgs e) {
            btnMessage.Visible = false;

            for (int iTop = 92; iTop >= 63; iTop -= 2) {
                System.Threading.Thread.Sleep(25);
                grdFriendStatus.Top = iTop;
                grdFriendStatus.Height += 2;
            }
        }

        private void ShowMessage(string sMessage) {
            btnMessage.Visible = true;
            btnMessage.Text = sMessage;

            for (int iTop = 63; iTop <= 92; iTop += 2) {
                System.Threading.Thread.Sleep(25);
                grdFriendStatus.Top = iTop;
                grdFriendStatus.Height -= 2;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            ShowAlert(new Forms.AlertForm("Test Alert", this));
        }

        public static void ShowAlert(System.Windows.Forms.Form AlertForm) {
            AlertForm.Show();
            System.Drawing.Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;

            AlertForm.Left = rect.Width - AlertForm.Width;
            AlertForm.Top = rect.Height - AlertForm.Height;
        }

        private void notifyIcon_Click(object sender, EventArgs e) {
            this.Show();
        }
    }
}
