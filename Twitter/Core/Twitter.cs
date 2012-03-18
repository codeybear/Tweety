using System;
using System.Collections.Generic;
using System.Xml;

namespace Tweety.Core {
    public class Status {
        public Int64 Id;
        public string Text;
        public string DateUpdated;
        public DateTime CreatedAt;
        public string CreatedAtDisplay;
        public string ReTweetedBy;
        public User User;
    }

    public class User {
        public string Name;
        public string ScreenName;
        public string Location;
        public string Description;
        public string ProfileImageUrl;
        public string StatusText;
    }

    public static class Twitter {
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
            OAuthTwitter oAuthTwitter = Ioc.Create<OAuthTwitter>();
            return oAuthTwitter.OAuthWebRequest(OAuthTwitter.Method.Post,
                                                TWITTER_URL + PATH_STATUS_UPDATE + EXT,
                                                "source=tweety&status=" + oAuthTwitter.UrlEncode(sMessage));
        }

        /// <summary> Get a specified user's details </summary>
        public static User GetUserInfo() {
            OAuthTwitter oAuthTwitter = Ioc.Create<OAuthTwitter>();
            string xml = oAuthTwitter.OAuthWebRequest(OAuthTwitter.Method.Get,
                                                      TWITTER_URL + PATH_VERIFY + EXT,
                                                      "");

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            return GetUserInfoFromNode(xmldoc.DocumentElement);
        }

        /// <summary> Get friends timeline </summary>
        public static List<Status> GetHomeTimeline() {
            string numberOfTweetsParam = "?count=" + NumberOfTweets;
            OAuthTwitter oAuthTwitter = Ioc.Create<OAuthTwitter>();
            string xml = oAuthTwitter.OAuthWebRequest(OAuthTwitter.Method.Get,
                                                      TWITTER_URL + PATH_HOME_TIMELINE + ".xml" +
                                                      numberOfTweetsParam,
                                                      "");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return GetStatusList(xmlDoc);
        }

        public static DateTime ConvertTwitterDate(string twitterDate) {
            return DateTime.ParseExact(twitterDate,
                                       DATETIME_FORMAT,
                                       System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary> Parse twitter date into user friendly display date/time. </summary>
        /// <param name="twitterDate">DateTime as returned by twitter. e.g. Sat Feb 26 20:27:09 +0000 2011</param>
        public static string ConvertTwitterDateDisplay(string twitterDate) {
            DateTime dt = ConvertTwitterDate(twitterDate);

            string dayElement = dt.Date == DateTime.Now.Date ? "Today" : dt.DayOfWeek.ToString();
            string timeElement = dt.ToShortTimeString();

            return string.Concat(dayElement, " ", timeElement);
        }

        //--------------------------------------------------------------
        // Private Methods
        //--------------------------------------------------------------
        private static List<Status> GetStatusList(XmlDocument xml) {
            List<Status> statusList = new List<Status>();

            foreach (XmlNode statusNode in xml.GetElementsByTagName("status")) {
                Status statusInfo;

                XmlNode retweetStatusNode = statusNode.SelectSingleNode("retweeted_status");
                if (retweetStatusNode != null) {
                    statusInfo = GetStatusFromNode(retweetStatusNode);
                    Status retweetInfo = GetStatusFromNode(statusNode);
                    statusInfo.ReTweetedBy = retweetInfo.User.Name;
                }
                else {
                    statusInfo = GetStatusFromNode(statusNode);
                }

                statusList.Add(statusInfo);
            }

            return statusList;
        }

        private static Status GetStatusFromNode(XmlNode statusNode) {
            Status statusInfo = new Status();
            string statusText = WebHelper.UrlDecode(statusNode["text"].InnerText);
            statusInfo.Text = statusText;
            statusInfo.Id = Convert.ToInt64(statusNode["id"].InnerText);
            statusInfo.CreatedAtDisplay = ConvertTwitterDateDisplay(statusNode["created_at"].InnerText);
            statusInfo.CreatedAt = ConvertTwitterDate(statusNode["created_at"].InnerText);
            statusInfo.User = GetUserInfoFromNode(statusNode.SelectSingleNode("user"));

            return statusInfo;
        }

        private static User GetUserInfoFromNode(XmlNode userNode) {
            User userInfo = new User();

            // Get the user's latest status
            XmlNode userStatusNode = userNode.SelectSingleNode("status");

            // This info may not exist on all user nodes
            if (userStatusNode != null)
                userInfo.StatusText = userStatusNode["text"].InnerText;

            userInfo.ProfileImageUrl = userNode["profile_image_url"].InnerText;
            userInfo.Name = userNode["name"].InnerText;
            userInfo.ScreenName = userNode["screen_name"].InnerText;
            userInfo.Location = userNode["location"].InnerText;
            userInfo.Description = userNode["description"].InnerText;

            return userInfo;
        }
    }
}