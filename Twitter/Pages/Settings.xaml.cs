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

            //txtUserName.Text = SettingHelper.UserName;
            //PasswordBox.Password = SettingHelper.Password;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e) {
            try {
                // Make a twitter call that checks both username and password
                //Twitter.GetFriendsTimeline(txtUserName.Text, PasswordBox.Password);

                // No exception so save these settings and continue
                //SettingHelper.UserName = txtUserName.Text.Trim();
                //SettingHelper.Password = PasswordBox.Password.Trim();
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
