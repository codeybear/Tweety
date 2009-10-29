using System.Windows;
using System.Windows.Controls;
using Core;

namespace Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings() {
            InitializeComponent();

            txtUserName.Text = SettingHelper.UserName;
            PasswordBox.Password = SettingHelper.Password;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e) {
            try {
                // TODO make a call that checks both username and password
                Result UserInfo = Twitter.GetUserInfo(txtUserName.Text);

                SettingHelper.UserName = txtUserName.Text.Trim();
                SettingHelper.Password = PasswordBox.Password.Trim();
                SettingHelper.Save();
                this.Close();
            }
            catch {
                MessageBox.Show(SettingHelper.MessageInvalidUserSettings);
                return;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
