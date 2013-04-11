using System;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using Tweety.Core;
using TweetSharp;

namespace Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        TwitterService service;
        OAuthRequestToken requestToken ;

        public Settings() {
            InitializeComponent();

            service = new TwitterService(SettingHelper.ConsumerKey, SettingHelper.ConsumerSecret);
            requestToken = service.GetRequestToken();
            Uri uri = service.GetAuthorizationUri(requestToken);
            WebBrowser.Navigate(uri);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e) {
            try {
                OAuthAccessToken access = service.GetAccessToken(requestToken, txtUpdate.Text);

                SettingHelper.Token = access.Token;
                SettingHelper.TokenSecret = access.TokenSecret;
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
