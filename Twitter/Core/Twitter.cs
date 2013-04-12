using System;

namespace Tweety.Core {

    public static class Twitter {
        public static int NumberOfTweets = 50;
        public static int TextLength = 140;
        public const string DATETIME_FORMAT = "ddd MMM dd HH:mm:ss zzzz yyyy";

        /// <summary> Parse twitter date into user friendly display date/time. </summary>
        /// <param name="twitterDate">DateTime as returned by twitter. e.g. Sat Feb 26 20:27:09 +0000 2011</param>
        public static string ConvertTwitterDateDisplay(DateTime dt) {
            string dayElement = dt.Date == DateTime.Now.Date ? "Today" : dt.DayOfWeek.ToString();
            string timeElement = dt.ToShortTimeString();

            return string.Concat(dayElement, " ", timeElement);
        }

    }
}