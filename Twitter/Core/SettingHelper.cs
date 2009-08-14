using System.Configuration;

namespace Core
{
    // Wrapper class to get/store user settings
    public static class SettingHelper
    {
        public static string UserName {
            get { return Properties.Settings.Default.UserName; }
            set { Properties.Settings.Default.UserName = value; }
        }

        public static string Password {
            get { return Properties.Settings.Default.PassWord; }
            set { Properties.Settings.Default.PassWord = value; }
        }

        public static string MessageInvalidUserSettings {
            get { return Properties.Settings.Default.MessageInvalidUserSettings; }
        }

        public static string MessageNewTweets {
            get { return Properties.Settings.Default.MessageNewTweets; }
        }

        public static void Save() {
            Properties.Settings.Default.Save();
        }
    }
}
