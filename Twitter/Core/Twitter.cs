using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;

namespace Core
{
    public class Status
    {
        public Int64 Id;
        public string Text;
        public string DateUpdated;
        public DateTime CreatedAt;
        public string CreatedAtDisplay;
        public string ReTweetedBy;
        public User User;
    }

    public class User
    {
        public string Name;
        public string ScreenName;
        public string Location;
        public string Description;
        public string ProfileImageUrl;
        public string StatusText;
    }

    public static class Twitter
    {
        private const string TWITTER_URL = "http://api.twitter.com/";
        private const string PATH_VERIFY = "account/verify_credentials";
        private const string PATH_HOME_TIMELINE = "statuses/home_timeline";
        private const string PATH_STATUS_UPDATE = "statuses/update";
        private const string PATH_FRIENDS_RETWEETS = "statuses/retweeted_to_me";
        private const string EXT = ".xml";

        /// <summary> Twitter datetime format </summary>
        public const string DATETIME_FORMAT = "ddd MMM dd HH:mm:ss zzzz yyyy";
        public static int NumberOfTweets = 50;
        public static int TextLength = 140;

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
        public static User GetUserInfo() {
            oAuthTwitter oAuthTwitter = Ioc.Create<oAuthTwitter>();
            string xml = oAuthTwitter.oAuthWebRequest(Core.oAuthTwitter.Method.GET,
                                                      TWITTER_URL + PATH_VERIFY + EXT,
                                                      "");

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            return GetUserInfoFromNode(xmldoc.DocumentElement);
        }

        /// <summary> Get friends timeline </summary>
        public static List<Status> GetHomeTimeline() {
            string NumberOfTweetsParam = "?count=" + NumberOfTweets;
            oAuthTwitter oAuthTwitter = Ioc.Create<oAuthTwitter>();
            string xml = oAuthTwitter.oAuthWebRequest(Core.oAuthTwitter.Method.GET,
                                                      TWITTER_URL + PATH_HOME_TIMELINE + ".xml" +
                                                      NumberOfTweetsParam,
                                                      "");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return GetStatusList(xmlDoc);
        }

        public static DateTime ConvertTwitterDate(string TwitterDate) {
            return DateTime.ParseExact(TwitterDate, 
                                       Twitter.DATETIME_FORMAT, 
                                       System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary> Parse twitter date into user friendly display date/time. </summary>
        /// <param name="TwitterDate">DateTime as returned by twitter. e.g. Sat Feb 26 20:27:09 +0000 2011</param>
        public static string ConvertTwitterDateDisplay(string TwitterDate) {
            DateTime dt = ConvertTwitterDate(TwitterDate);

            string DayElement = dt.Date == DateTime.Now.Date ? "Today" : dt.DayOfWeek.ToString();
            string TimeElement = dt.ToShortTimeString();

            return string.Concat(DayElement, " ", TimeElement);
        }

        //--------------------------------------------------------------
        // Private Methods
        //--------------------------------------------------------------
        private static List<Status> GetStatusList(XmlDocument xml) {
            List<Status> StatusList = new List<Status>();

            foreach (XmlNode StatusNode in xml.GetElementsByTagName("status")) {
                Status StatusInfo = new Status();

                XmlNode RetweetStatusNode = StatusNode.SelectSingleNode("retweeted_status");
                if (RetweetStatusNode != null) {
                    StatusInfo = GetStatusFromNode(RetweetStatusNode);
                    Status RetweetInfo = GetStatusFromNode(StatusNode);
                    StatusInfo.ReTweetedBy = RetweetInfo.User.Name;
                }
                else {
                    StatusInfo = GetStatusFromNode(StatusNode);
                }

                StatusList.Add(StatusInfo);
            }

            return StatusList;
        }

        private static Status GetStatusFromNode(XmlNode StatusNode) {
            Status StatusInfo = new Status();
            string StatusText = WebHelper.UrlDecode(StatusNode["text"].InnerText);
            StatusInfo.Text = StatusText;
            StatusInfo.Id = Convert.ToInt64(StatusNode["id"].InnerText);
            StatusInfo.CreatedAtDisplay = ConvertTwitterDateDisplay(StatusNode["created_at"].InnerText);
            StatusInfo.CreatedAt = ConvertTwitterDate(StatusNode["created_at"].InnerText);
            StatusInfo.User = GetUserInfoFromNode(StatusNode.SelectSingleNode("user"));

            return StatusInfo;
        }

        private static User GetUserInfoFromNode(XmlNode UserNode) {
            User UserInfo = new User();

            // Get the user's latest status
            XmlNode UserStatusNode = UserNode.SelectSingleNode("status");

            // This info may not exist on all user nodes
            if (UserStatusNode != null)
                UserInfo.StatusText = UserStatusNode["text"].InnerText;

            UserInfo.ProfileImageUrl = UserNode["profile_image_url"].InnerText;
            UserInfo.Name = UserNode["name"].InnerText;
            UserInfo.ScreenName = UserNode["screen_name"].InnerText;
            UserInfo.Location = UserNode["location"].InnerText;
            UserInfo.Description = UserNode["description"].InnerText;

            return UserInfo;
        }
    }
}
