
namespace Tweety.Core
{
    // Wrapper class to get/store user settings
    public static class SettingHelper
    {
        public static string ConsumerKey {
            get { return "NomSLaaS4Ze8Gd2wvSCrZg"; }
        }

        public static string ConsumerSecret {
            get { return "eN2YLUNeUEjWkcGbSJLwgeM2tFBKTx9dmYjwxbnXo"; }
        }

        public static string Token {
            get { return Properties.Settings.Default.Token; }
            set { Properties.Settings.Default.Token = value; }
        }

        public static string TokenSecret {
            get { return Properties.Settings.Default.TokenSecret; }
            set { Properties.Settings.Default.TokenSecret = value; }
        }

        public static string MessageInvalidUserSettings {
            get { return "Unable to validate user settings"; }
        }

        public static string MessageNewTweets {
            get { return "New tweets have arrived"; }
        }

        public static string TweetyIconUriString {
            get { return @"/Resources/Peace Dove.ico"; }
        }

        public static void Save() {
            Properties.Settings.Default.Save();
        }
    }
}
