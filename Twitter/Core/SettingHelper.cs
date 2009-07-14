using System.Configuration;

namespace Twitter
{
    public static class SettingHelper
    {
        public static string UserName {
            get { return "pjcooke"; } //Properties.Settings.Default.UserName
            set { Properties.Settings.Default.UserName = value; }
        }

        public static string Password {
            get { return "saints99"; } //Properties.Settings.Default.PassWord
            set { Properties.Settings.Default.PassWord = value; }
        }

        public static string MessageInvalidUserSettings {
            get { return Properties.Settings.Default.MessageInvalidUserSettings; }
        }
    }
}
