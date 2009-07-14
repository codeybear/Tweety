using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;

namespace Twitter
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

        private static ImageCache _ImageCache = new ImageCache();

        // Public methods:

        public static String UpdateStatus(String message) {
            String returnValue;
            HttpWebResponse response;

            try {
                response = GetWebResponse("statuses/update.xml?source=threeter&status=" + message, "POST");

                StreamReader reader = new StreamReader(response.GetResponseStream());
                returnValue = reader.ReadToEnd();
                reader.Close();

                if (returnValue.Contains("<status>"))
                    returnValue = String.Empty;
            }
            catch (Exception e) {
                returnValue = e.Message;
            }

            return returnValue;
        }

        public static Result GetUserInfo(String sUserName) {
            HttpWebResponse Response = GetWebResponse("users/show/" + sUserName + ".xml", "GET");
            XmlDocument xml = LoadResponseToXMLDoc(Response);
            return GetUserInfoFromNode(xml.DocumentElement);
        }

        public static List<Result> GetFriendsTimeLine(string sUserName, string sPassword) {
            HttpWebResponse Response = GetWebResponse(PATH_FRIENDS_TIMELINE + ".xml", "GET", sUserName, sPassword);
            XmlDocument xml = LoadResponseToXMLDoc(Response);
            return GetStatusList(xml);
        }

        public static List<Result> GetFriendsStatus(string sUserName) {
            HttpWebResponse Response = GetWebResponse(PATH_FRIENDS_STATUS + sUserName + ".xml", "GET");
            XmlDocument xml = LoadResponseToXMLDoc(Response);
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
                byte[] ImageBytes = Utility.CopyStreamToByteArray(GetStreamFromURL(UserInfo.ProfileImageUrl));
                _ImageCache.StoreImage(UserInfo.ProfileImageUrl, ImageBytes);
            }

            UserInfo.ProfileImage = _ImageCache.GetImage(UserInfo.ProfileImageUrl);

            return UserInfo;
        }

        private static Stream GetStreamFromURL(string sURL) {
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(sURL);
            WebResponse Response = Request.GetResponse();
            return Response.GetResponseStream();
        }

        private static System.Drawing.Bitmap GetBitmapFromURL(string sURL) {
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(sURL);
            WebResponse Response = Request.GetResponse();
            Stream ResponseStream = Response.GetResponseStream();
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ResponseStream);

            return bmp;
        }

        private static HttpWebResponse GetWebResponse(String urlParameters, String sMethod) {
            HttpWebRequest request = WebRequest.Create(TWITTER_URL + urlParameters) as HttpWebRequest;
            request.Method = sMethod;
            return request.GetResponse() as HttpWebResponse;
        }

        private static HttpWebResponse GetWebResponse(String urlParameters, String sMethod, string sUserName, string sPassword) {
            HttpWebRequest request = WebRequest.Create(TWITTER_URL + urlParameters) as HttpWebRequest;
            request.Method = sMethod;
            request.Credentials = new NetworkCredential(sUserName, sPassword);
            return request.GetResponse() as HttpWebResponse;
        }

        private static XmlDocument LoadResponseToXMLDoc(HttpWebResponse Response) {
            using (StreamReader Reader = new StreamReader(Response.GetResponseStream())) {
                XmlDocument xml = new XmlDocument();
                xml.Load(Reader);
                return xml;
            }
        }

        //private static List<Result> GetUserInfoLinq(TextReader XMLUser)
        //{
        //    XElement xml = XElement.Load(XMLUser);

        //    var UserList = from User in xml.Elements("user")
        //                   select new Result
        //                   {
        //                       Name = User.Element("name").Value,
        //                       ProfileImageUrl = User.Element("profile_image_url").Value,
        //                       Url = User.Element("url").Value,
        //                       ProfileImage = GetBitmapFromURL(User.Element("profile_image_url").Value)
        //                   };

        //    return new List<Result>(UserList);
        //}
    }
}
