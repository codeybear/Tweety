using System;
using System.Windows.Forms;
using Core;

namespace Forms
{
    public partial class SettingsForm : Form
    {
        public SettingsForm() {
            InitializeComponent();

            txtUserName.Text = SettingHelper.UserName;
            txtPassword.Text = SettingHelper.Password;
        }

        private void btnOk_Click(object sender, EventArgs e) {
            try {
    			// TODO make a call that checks both username and password
                Result UserInfo = Twitter.GetUserInfo(txtUserName.Text);

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

        private void SettingsForm_Load(object sender, EventArgs e) {

        }
    }
}
