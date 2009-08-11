using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core
{
    public class StatusAlert {
        public String ScreenName;
        public String Name;
        public String Text;
        public String CreatedAt;
    }

    public class Result
    {
        public String ID;
        public String Text;
        public String ProfileImageUrl;
        public System.Drawing.Bitmap ProfileImage;
    }

    public static class Twitter
    {
        private const int MAXCHARACTERS = 149;
        private const string TWITTER_URL = "http://twitter.com/";
        private const string PATH_FRIENDS_STATUS = "statuses/friends/";
        private const string PATH_FRIENDS_TIMELINE = "statuses/friends_timeline";
        private const string PATH_STATUS_UPDATE = "statuses/update.xml?source=tweety&status=";
        private const string PATH_USERS_SHOW = "users/show/";

        private static ImageCache _ImageCache = new ImageCache();

        // Public methods:

        public static String UpdateStatus(string sMessage, string sUserName, string sPassword) {
            Stream ResponseStream = WebHelper.GetWebResponse(TWITTER_URL + PATH_STATUS_UPDATE + sMessage, WebHelper.HTTPPOST, sUserName, sPassword);
            StreamReader reader = new StreamReader(ResponseStream);
            string returnValue = reader.ReadToEnd();
            reader.Close();

            return returnValue;
        }

        public static Result GetUserInfo(string sUserName) {
            Stream ResponseStream = WebHelper.GetWebResponse(TWITTER_URL + PATH_USERS_SHOW + sUserName + ".xml", WebHelper.HTTPGET);
            XmlDocument xml = new XmlDocument();
            xml.Load(ResponseStream);
            return GetUserInfoFromNode(xml.DocumentElement);
        }

        public static System.Drawing.Bitmap GetUserProfileImage(string sUserProfileURL) {
            if (!_ImageCache.ContainsKey(sUserProfileURL)) {
                byte[] ImageBytes = WebHelper.GetBytesFromURL(sUserProfileURL);
                _ImageCache.StoreImage(sUserProfileURL, ImageBytes);
            }

            return _ImageCache.GetImage(sUserProfileURL);
        }

        public static List<Result> GetFriendsTimeLine(string sUserName, string sPassword) {
            Stream ResponseStream = WebHelper.GetWebResponse(TWITTER_URL + PATH_FRIENDS_TIMELINE + ".xml", WebHelper.HTTPGET, sUserName, sPassword);
            XmlDocument xml = new XmlDocument();
            xml.Load(ResponseStream);
            return GetStatusList(xml);
        }

        public static List<Result> GetFriendsStatus(string sUserName) {
            Stream ResponseStream = WebHelper.GetWebResponse(TWITTER_URL + PATH_FRIENDS_STATUS + sUserName + ".xml", WebHelper.HTTPGET);
            XmlDocument xml = new XmlDocument();
            xml.Load(ResponseStream);
            return GetUserList(xml);
        }

        /// <summary> Persist user images on the file system <\summary>
        public static void SaveImageCache(string sFileName) {
            _ImageCache.Save(sFileName);
        }

        /// <summary> Retrieve user images from the file system </summary>
        public static void LoadImageCache(string sFileName) {
            if (!File.Exists(sFileName)) return;
            _ImageCache.Load(sFileName);
        }

        // Private Methods

        private static List<Result> GetStatusList(XmlDocument xml) {
            List<Result> StatusList = new List<Result>();

            foreach (XmlNode StatusNode in xml.GetElementsByTagName("status")) {
                Result StatusInfo = new Result();
                StatusInfo.Text = StatusNode["text"].InnerText;
                StatusInfo.ID = StatusNode["id"].InnerText;

                Result UserInfo = GetUserInfoFromNode(StatusNode.SelectSingleNode("user"));
                StatusInfo.ProfileImage = UserInfo.ProfileImage;

                StatusList.Add(StatusInfo);
            }

            return StatusList;
        }

        private static List<Result> GetUserList(XmlDocument xml) {
            List<Result> UserInfoList = new List<Result>();

            foreach (XmlNode UserNode in xml.GetElementsByTagName("user"))
                UserInfoList.Add(GetUserInfoFromNode(UserNode));

            return UserInfoList;
        }

        private static Result GetUserInfoFromNode(XmlNode UserNode) {
            Result UserInfo = new Result();

            // Get the user's latest status
            XmlNode UserStatusNode = UserNode.SelectSingleNode("status");

            // This info may not exist on all user nodes
            if(UserStatusNode != null)
                UserInfo.Text = UserStatusNode["text"].InnerText;

            // UserInfo.Text = UserNode["name"].InnerText;
            UserInfo.ProfileImageUrl = UserNode["profile_image_url"].InnerText;

            if (!_ImageCache.ContainsKey(UserInfo.ProfileImageUrl)) {
                byte[] ImageBytes = WebHelper.GetBytesFromURL(UserInfo.ProfileImageUrl);
                _ImageCache.StoreImage(UserInfo.ProfileImageUrl, ImageBytes);
            }

            UserInfo.ProfileImage = _ImageCache.GetImage(UserInfo.ProfileImageUrl);

            return UserInfo;
        }
    }
}
