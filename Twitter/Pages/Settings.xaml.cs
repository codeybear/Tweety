using System;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using Tweety.Core;

namespace Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private string OAuthToken { get; set; }

        public Settings() {
            InitializeComponent();

            OAuthTwitter oAuth = new OAuthTwitter();
            oAuth.ConsumerKey = SettingHelper.ConsumerKey;
            oAuth.ConsumerSecret = SettingHelper.ConsumerSecret;

            //Get the authorization url.
            Uri url = new Uri(oAuth.AuthorizationLinkGet());

            //Save the token.
            OAuthToken = HttpUtility.ParseQueryString(url.Query)["oauth_token"];

            //Show the intro text with the authorization link.
            string HTML = "<html><head><title></title><style type='text/css'>body {background-color:#5599BB;color:#ffffff;font-family:arial;}img{border:none}</style></head><body>"
                + "<h1>Tweety Authorisation</h1><p>To use this application, you must first login to Twitter.</p>"
                + "<p>Click the login button below and the Twitter login page will appear.</p>"
                + "<p>After you login, you will be provided with a PIN.</p>"
                + "<p>Please enter the PIN in the box below and click Ok.</p>"
                + "<p><a href=\"" + url.ToString() + "\"><img src=\"http://apiwiki.twitter.com/f/1242697608/Sign-in-with-Twitter-lighter.png\" alt=\"Sign in with Twitter\" /></a></p>"
                + "</body></html>";

            WebBrowser.NavigateToString(HTML);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e) {
            try {
                OAuthTwitter oAuth = new OAuthTwitter();
                oAuth.ConsumerKey = SettingHelper.ConsumerKey;
                oAuth.ConsumerSecret = SettingHelper.ConsumerSecret;
                oAuth.Token = OAuthToken;
                oAuth.AccessTokenGet(OAuthToken, txtUpdate.Text);

                SettingHelper.Token = oAuth.Token;
                SettingHelper.TokenSecret = oAuth.TokenSecret;
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
