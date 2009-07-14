using System;
using System.Windows.Forms;

namespace Twitter
{
    public partial class SettingsForm : Form
    {
        private MainForm _MainForm;

        public SettingsForm(MainForm MainForm) {
            InitializeComponent();

            _MainForm = MainForm;

            txtUserName.Text = SettingHelper.UserName;
            txtPassword.Text = SettingHelper.Password;
        }

        private void btnOk_Click(object sender, EventArgs e) {
            try {
                Result UserInfo = Twitter.GetUserInfo(SettingHelper.UserName);
                _MainForm.UserImage = UserInfo.ProfileImage;
                SettingHelper.UserName = txtUserName.Text.Trim();
                SettingHelper.Password = txtPassword.Text.Trim();

                this.Close();
            }
            catch {
                MessageBox.Show(SettingHelper.MessageInvalidUserSettings);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
