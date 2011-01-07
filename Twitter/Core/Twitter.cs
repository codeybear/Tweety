using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;

namespace Core
{
    public class StatusAlert
    {
        public String ScreenName;
        public String Name;
        public String Text;
    }

    public class Result
    {
        public String Id;
        public String Text;
        public String ProfileImageUrl;
        public String DateUpdated;
        public String CreatedAt;
    }

    public static class Twitter
    {
        private const string TWITTER_URL = "http://twitter.com/";
        private const string PATH_VERIFY = "account/verify_credentials";
        private const string PATH_FRIENDS_TIMELINE = "statuses/friends_timeline";
        private const string PATH_STATUS_UPDATE = "statuses/update";
        private const string EXT = ".xml";

        public static int NumberOfTweets = 50;

        //--------------------------------------------------------------
        // Public methods
        //--------------------------------------------------------------

        /// <summary> Update a specified user's status </summary>
        public static String UpdateStatus(string sMessage) {
            oAuthTwitter oAuthTwitter = Ioc.Create<oAuthTwitter>();
            return oAuthTwitter.oAuthWebRequest(Core.oAuthTwitter.Method.POST,
                                                      TWITTER_URL + PATH_STATUS_UPDATE + EXT,
                                                      "source=tweety&status=" + oAuthTwitter.UrlEncode(sMessage));
        }

        /// <summary> Get a specified user's details </summary>
        public static Result GetUserInfo() {
            oAuthTwitter oAuthTwitter = Ioc.Create<oAuthTwitter>();
            string xml = oAuthTwitter.oAuthWebRequest(Core.oAuthTwitter.Method.GET,
                                                      TWITTER_URL + PATH_VERIFY + EXT,
                                                      string.Empty);

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            return GetUserInfoFromNode(xmldoc.DocumentElement);
        }

        /// <summary> Get friends timeline </summary>
        public static List<Result> GetFriendsTimeline() {
            string NumberOfTweetsParam = "?count=" + NumberOfTweets;
            oAuthTwitter oAuthTwitter = Ioc.Create<oAuthTwitter>();
            string xml = oAuthTwitter.oAuthWebRequest(Core.oAuthTwitter.Method.GET, 
                                                      TWITTER_URL + PATH_FRIENDS_TIMELINE + ".xml" +
                                                      NumberOfTweetsParam, 
                                                      "");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return GetStatusList(xmlDoc);
        }

        //--------------------------------------------------------------
        // Private Methods
        //--------------------------------------------------------------

        private static List<Result> GetStatusList(XmlDocument xml) {
            List<Result> StatusList = new List<Result>();

            foreach (XmlNode StatusNode in xml.GetElementsByTagName("status")) {
                Result StatusInfo = new Result();
                string StatusText = WebHelper.UrlDecode(StatusNode["text"].InnerText);
                StatusInfo.Text = StatusText;
                StatusInfo.Id = StatusNode["id"].InnerText;
                StatusInfo.CreatedAt = ConvertTwitterDate(StatusNode["created_at"].InnerText);

                Result UserInfo = GetUserInfoFromNode(StatusNode.SelectSingleNode("user"));
                StatusInfo.ProfileImageUrl = UserInfo.ProfileImageUrl;

                StatusList.Add(StatusInfo);
            }

            return StatusList;
        }

        /// <summary> Parse twitter date into user friendly display date/time. </summary>
        /// <param name="TwitterDate">DateTime as returned by twitter. e.g. Sun Dec 20 15:16:16 +0000 2009</param>
        private static string ConvertTwitterDate(string TwitterDate) {
            string[] Elements = TwitterDate.Split(' ');

            string DayElement = Convert.ToInt32(Elements[2]) == DateTime.Now.Day ? "Today" : Elements[0];

            string TimeElement = Elements[3];
            TimeElement = TimeElement.Substring(0, TimeElement.LastIndexOf(':'));

            return string.Concat(DayElement, " ", TimeElement);
        }

        private static Result GetUserInfoFromNode(XmlNode UserNode) {
            Result UserInfo = new Result();

            // Get the user's latest status
            XmlNode UserStatusNode = UserNode.SelectSingleNode("status");

            // This info may not exist on all user nodes
            if (UserStatusNode != null)
                UserInfo.Text = UserStatusNode["text"].InnerText;

            UserInfo.ProfileImageUrl = UserNode["profile_image_url"].InnerText;

            return UserInfo;
        }
    }
}
