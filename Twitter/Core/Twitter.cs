using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core
{
    public class StatusAlert
    {
        public String ScreenName;
        public String Name;
        public String Text;
        public String CreatedAt;
    }

    public class Result
    {
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
        private const string PATH_USERS_SHOW = "users/show/";

        private static ImageCache _ImageCache = new ImageCache();
        private static Int64 _iLastId;

        // Public methods:

        public static String UpdateStatus(String message) {
            Stream ResponseStream = WebHelper.GetWebResponse("statuses/update.xml?source=threeter&status=" + message, WebHelper.HTTPPOST);
            StreamReader reader = new StreamReader(ResponseStream);
            string returnValue = reader.ReadToEnd();
            reader.Close();

            return returnValue;
        }

        public static Result GetUserInfo(String sUserName) {
            Stream ResponseStream = WebHelper.GetWebResponse(TWITTER_URL + PATH_USERS_SHOW + sUserName + ".xml", WebHelper.HTTPGET);
            XmlDocument xml = new XmlDocument();
            xml.Load(ResponseStream);
            return GetUserInfoFromNode(xml.DocumentElement);
        }

        public static System.Drawing.Bitmap GetUserProfileImageFromCache(string sUserProfileURL) {
            if (_ImageCache.ContainsKey(sUserProfileURL))
                return _ImageCache.GetImage(sUserProfileURL);
            else
                return null;
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
            bool bFirst = true;

            foreach (XmlNode StatusNode in xml.GetElementsByTagName("status")) {
                Result StatusInfo = new Result();
                StatusInfo.Text = StatusNode["text"].InnerText;

                if (bFirst) {
                    _iLastId = Convert.ToInt64(StatusNode["id"].InnerText);
                    bFirst = false;
                }

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

            UserInfo.Text = UserNode["name"].InnerText;
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
