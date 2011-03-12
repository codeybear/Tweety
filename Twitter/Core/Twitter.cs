using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;

namespace Core
{
    public class Result
    {
        public Int64 Id;
        public string Text;
        public string ProfileImageUrl;
        public string DateUpdated;
        public DateTime CreatedAt;
        public string CreatedAtDisplay;
        public string CreatedBy;
        public string ReTweetedBy;
        public string Name;
    }

    public static class Twitter
    {
        private const string TWITTER_URL = "http://api.twitter.com/";
        private const string PATH_VERIFY = "account/verify_credentials";
        private const string PATH_FRIENDS_TIMELINE = "statuses/friends_timeline";
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
        public static Result GetUserInfo() {
            oAuthTwitter oAuthTwitter = Ioc.Create<oAuthTwitter>();
            string xml = oAuthTwitter.oAuthWebRequest(Core.oAuthTwitter.Method.GET,
                                                      TWITTER_URL + PATH_VERIFY + EXT,
                                                      "");

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

        public static List<Result> GetFriendsTimelineWithRetweets() {
            List<Result> TimeLine = GetFriendsTimeline();
            List<Result> ReTweets = GetFriendsRetweets(TimeLine.Last().Id);

            TimeLine.AddRange(ReTweets);
            TimeLine = TimeLine.OrderByDescending((status) => status.CreatedAt).ToList();

            return TimeLine;
        }

        public static List<Result> GetFriendsRetweets(Int64 SinceId) {
            oAuthTwitter oAuthTwitter = Ioc.Create<oAuthTwitter>();
            string xml = oAuthTwitter.oAuthWebRequest(oAuthTwitter.Method.GET,
                                                      TWITTER_URL + PATH_FRIENDS_RETWEETS + ".xml" +
                                                      "?since_id=" + SinceId,
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
        private static List<Result> GetStatusList(XmlDocument xml) {
            List<Result> StatusList = new List<Result>();

            foreach (XmlNode StatusNode in xml.GetElementsByTagName("status")) {
                Result StatusInfo = new Result();

                XmlNode RetweetStatusNode = StatusNode.SelectSingleNode("retweeted_status");
                if (RetweetStatusNode != null) {
                    StatusInfo = GetStatusFromNode(RetweetStatusNode);
                    Result RetweetInfo = GetStatusFromNode(StatusNode);
                    StatusInfo.ReTweetedBy = RetweetInfo.Name;
                }
                else {
                    StatusInfo = GetStatusFromNode(StatusNode);
                }

                StatusList.Add(StatusInfo);
            }

            return StatusList;
        }

        private static Result GetStatusFromNode(XmlNode StatusNode) {
            Result StatusInfo = new Result();
            string StatusText = WebHelper.UrlDecode(StatusNode["text"].InnerText);
            StatusInfo.Text = StatusText;
            StatusInfo.Id = Convert.ToInt64(StatusNode["id"].InnerText);
            StatusInfo.CreatedAtDisplay = ConvertTwitterDateDisplay(StatusNode["created_at"].InnerText);
            StatusInfo.CreatedAt = ConvertTwitterDate(StatusNode["created_at"].InnerText);

            Result UserInfo = GetUserInfoFromNode(StatusNode.SelectSingleNode("user"));
            StatusInfo.ProfileImageUrl = UserInfo.ProfileImageUrl;
            StatusInfo.CreatedBy = UserInfo.CreatedBy;
            StatusInfo.Name = UserInfo.Name;

            return StatusInfo;
        }

        private static Result GetUserInfoFromNode(XmlNode UserNode) {
            Result UserInfo = new Result();

            // Get the user's latest status
            XmlNode UserStatusNode = UserNode.SelectSingleNode("status");

            // This info may not exist on all user nodes
            if (UserStatusNode != null)
                UserInfo.Text = UserStatusNode["text"].InnerText;

            UserInfo.ProfileImageUrl = UserNode["profile_image_url"].InnerText;
            UserInfo.Name = UserNode["name"].InnerText;

            return UserInfo;
        }
    }
}
