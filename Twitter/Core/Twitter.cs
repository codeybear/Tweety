using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;

namespace Core {
    public class StatusAlert {
        public String ScreenName;
        public String Name;
        public String Text;
    }

    public class Result {
        public String ID;
        public String Text;
        public String ProfileImageUrl;
        public String DateUpdated;
        public String CreatedAt;
    }

    public static class Twitter {
        private const int MAXCHARACTERS = 149;
        private const string TWITTER_URL = "http://twitter.com/";
        private const string PATH_FRIENDS_STATUS = "statuses/friends/";
        private const string PATH_FRIENDS_TIMELINE = "statuses/friends_timeline";
        private const string PATH_STATUS_UPDATE = "statuses/update.xml";
        private const string PATH_USERS_SHOW = "users/show/";

        public static string ConsumerKey { get; set; }
        public static string ConsumerSecret { get; set; }
        public static string Token { get; set; }
        public static string TokenSecret { get; set; }

        //--------------------------------------------------------------
        // Public methods
        //--------------------------------------------------------------

        /// <summary> Update a specified user's status </summary>
        public static String UpdateStatus(string sMessage) {
            // TODO convert
            string querystring = "?source=tweety&status=";
            Stream ResponseStream = WebHelper.GetWebResponse(TWITTER_URL + PATH_STATUS_UPDATE + sMessage, WebHelper.HTTPPOST, "", "");
            StreamReader reader = new StreamReader(ResponseStream);
            string returnValue = reader.ReadToEnd();
            reader.Close();

            return returnValue;
        }

        /// <summary> Get a specified user's details </summary>
        public static Result GetUserInfo() {
            oAuthTwitter oAuthTwitter = CreateOAuthTwitterObject();
            string xml = oAuthTwitter.oAuthWebRequest(Core.oAuthTwitter.Method.GET,
                                                      TWITTER_URL + PATH_STATUS_UPDATE + ".xml",
                                                      string.Empty);
            //Stream ResponseStream = WebHelper.GetWebResponse(TWITTER_URL + PATH_USERS_SHOW + "" + ".xml", WebHelper.HTTPGET);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            return GetUserInfoFromNode(xmldoc.DocumentElement);
        }

        /// <summary> Get friends timeline </summary>
        public static List<Result> GetFriendsTimeline() {
            string NumberOfTweets = "count=50";
            oAuthTwitter oAuthTwitter = CreateOAuthTwitterObject();
            string xml = oAuthTwitter.oAuthWebRequest(Core.oAuthTwitter.Method.GET, 
                                                      TWITTER_URL + PATH_FRIENDS_TIMELINE + ".xml", 
                                                      NumberOfTweets);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return GetStatusList(xmlDoc);
        }

        //--------------------------------------------------------------
        // Private Methods
        //--------------------------------------------------------------

        private static oAuthTwitter CreateOAuthTwitterObject() {
            oAuthTwitter oAuthTwitter = new oAuthTwitter();
            oAuthTwitter.ConsumerKey = ConsumerKey;
            oAuthTwitter.ConsumerSecret = ConsumerSecret;
            oAuthTwitter.Token = Token;
            oAuthTwitter.TokenSecret = TokenSecret;

            return oAuthTwitter;
        }

        private static List<Result> GetStatusList(XmlDocument xml) {
            List<Result> StatusList = new List<Result>();

            foreach (XmlNode StatusNode in xml.GetElementsByTagName("status")) {
                Result StatusInfo = new Result();
                string StatusText = WebHelper.UrlDecode(StatusNode["text"].InnerText);
                StatusInfo.Text = StatusText;
                StatusInfo.ID = StatusNode["id"].InnerText;
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
