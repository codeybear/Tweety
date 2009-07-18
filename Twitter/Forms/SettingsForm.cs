using System;
using System.Windows.Forms;

namespace Twitter.Forms
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
                Result UserInfo = Twitter.GetUserInfo(txtUserName.Text);

                SettingHelper.UserName = txtUserName.Text.Trim();
                SettingHelper.Password = txtPassword.Text.Trim();
                SettingHelper.ProfileImageURL = UserInfo.ProfileImageUrl;

                _MainForm.UserImage = UserInfo.ProfileImage;

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
