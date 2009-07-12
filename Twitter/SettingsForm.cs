using System;
using System.Windows.Forms;

namespace Twitter
{
    public partial class SettingsForm : Form
    {
        public SettingsForm() {
            InitializeComponent();
            txtUserName.Text = SettingHelper.UserName;
            txtPassword.Text = SettingHelper.Password;
        }

        private void btnOk_Click(object sender, EventArgs e) {
            SettingHelper.UserName = txtUserName.Text.Trim();
            SettingHelper.Password = txtPassword.Text.Trim();
            this.Close();
        }
    }
}
