using System;
using System.Collections.Generic;
using System.Xml;

namespace Core
{
    public class Result
    {
        public string Id;
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
            oAuthTwitter oAuthTwitter = Ioc.Create<oAuthTwitter>();
            string xml = oAuthTwitter.oAuthWebRequest(Core.oAuthTwitter.Method.GET,
                                                      TWITTER_URL + PATH_FRIENDS_RETWEETS + ".xml" +
                                                      "",
                                                      "");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            List<Result> timeLine = GetFriendsTimeline();
            timeLine.AddRange(GetStatusList(xmlDoc));
            timeLine.Sort((Status1, Status2) => Status1.CreatedAt.CompareTo(Status2.CreatedAt));
            timeLine.Reverse();

            return timeLine;
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
            StatusInfo.Id = StatusNode["id"].InnerText;
            StatusInfo.CreatedAtDisplay = ConvertTwitterDateDisplay(StatusNode["created_at"].InnerText);
            StatusInfo.CreatedAt = ConvertTwitterDate(StatusNode["created_at"].InnerText);

            Result UserInfo = GetUserInfoFromNode(StatusNode.SelectSingleNode("user"));
            StatusInfo.ProfileImageUrl = UserInfo.ProfileImageUrl;
            StatusInfo.CreatedBy = UserInfo.CreatedBy;
            StatusInfo.Name = UserInfo.Name;

            return StatusInfo;
        }

        public static DateTime ConvertTwitterDate(string TwitterDate) {
            const string format = "ddd MMM dd HH:mm:ss zzzz yyyy";
            return DateTime.ParseExact(TwitterDate, format, System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary> Parse twitter date into user friendly display date/time. </summary>
        /// <param name="TwitterDate">DateTime as returned by twitter. e.g. Sat Feb 26 20:27:09 +0000 2011</param>
        public static string ConvertTwitterDateDisplay(string TwitterDate) {
            DateTime dt = ConvertTwitterDate(TwitterDate);

            string DayElement = dt.Date == DateTime.Now.Date ? "Today" : dt.DayOfWeek.ToString();
            string TimeElement = dt.ToShortTimeString();

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
            UserInfo.Name = UserNode["name"].InnerText;

            return UserInfo;
        }

    }
}
