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

        // Allow access to user image from other forms
        public Bitmap UserImage { set { picProfileImage.Image = value; } }

        //--------------------------------------------------------------
        // Events 
        //--------------------------------------------------------------

        private void MainForm_Load(object sender, EventArgs e) {
            // Get any friends images that have been stored
            Twitter.LoadImageCache(System.IO.Path.Combine(Application.StartupPath, "ImageCache.txt"));

            // Setup timer to get friends timeline
            StatusTimer = new Timer();
            StatusTimer.Tick += new EventHandler(StatusTimer_Tick);
            StatusTimer.Interval = 1000 * 60;
            StatusTimer.Start();

            // Get friends timeline
            BackgroundWorker_SetupFriendsTimeLine();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            Twitter.SaveImageCache(System.IO.Path.Combine(Application.StartupPath, "ImageCache.txt"));
            notifyIcon.Visible = false;
        }

        void StatusTimer_Tick(object sender, EventArgs e) {
            BackgroundWorker_SetupFriendsTimeLine();
        }

        private void btnUserInfo_Click(object sender, EventArgs e) {
            Result user = Twitter.GetUserInfo(SettingHelper.UserName);
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
                e.Result = Twitter.GetFriendsTimeLine(SettingHelper.UserName, SettingHelper.Password);
            }
            catch (System.Net.WebException ex) {
                if (ex.Response != null) {
                    if (btnMessage.InvokeRequired) {
                        Action ShowError = () => btnMessage.Text = ex.Response.Headers["status"];
                        btnMessage.Invoke(ShowError);
                    }
                    else
                        btnMessage.Text = "Error";
                    //btnMessage.Text = ex.Response.Headers["status"];
                }
                else
                    if (btnMessage.InvokeRequired)
                        btnMessage.Invoke(new Action(() => btnMessage.Text = "Error"));
                    else
                        btnMessage.Text = "Error";  

                    //btnMessage.Text = ex.ToString();

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
            SettingsForm SettingsForm = new SettingsForm(this);
            SettingsForm.ShowDialog();
        }
    }
}
