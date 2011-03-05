using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Test
{
    /// <summary>
    ///This is a test class for TwitterTest and is intended
    ///to contain all TwitterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TwitterTest
    {
        private TestContext testContextInstance;

        /// <summary>h
        #region Additional test attributes

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) {
            // Setup the Ioc container
            Ioc.Register<oAuthTwitter>(() => {
                oAuthTwitter oAuthTwitter = new oAuthTwitter();
                oAuthTwitter.ConsumerKey = SettingHelper.ConsumerKey;
                oAuthTwitter.ConsumerSecret = SettingHelper.ConsumerSecret;
                oAuthTwitter.Token = "15230032-d7CyRyzNQ2V7Br768V565OC407KnTPKRp0Lg9L7ft";
                oAuthTwitter.TokenSecret = "8ZYwrLAVYlCRLrArrQA8MjTRL0RslDiRXnH0Uu1bU";
                return oAuthTwitter;
            });
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for GetFriendsTimeline
        ///</summary>
        [TestMethod()]
        public void GetFriendsTimelineTest() {
            List<Result> actual = Twitter.GetFriendsTimeline();
            Assert.IsTrue(actual.Count > 0);
            actual.ForEach((status) => Debug.WriteLine("{0}***{1}***{2}", status.CreatedAt, status.Name, status.Text));
        }

        [TestMethod()]
        public void GetFriendsTimelineTestWithRetweets() {
            List<Result> actual = Twitter.GetFriendsTimelineWithRetweets();
            bool Retweets = true;

            foreach (Result result in actual) {
                if (!string.IsNullOrWhiteSpace(result.ReTweetedBy))
                    Retweets = true;

                Debug.WriteLine("{0}***{1}***{2}", result.CreatedAt, result.ReTweetedBy, result.Text);
            }

            if (!Retweets)
                Assert.Inconclusive("Cannot verify retweets");
        }

        [TestMethod]
        public void ConvertTwitterDate() {
            string DisplayDate = Twitter.ConvertTwitterDateDisplay(DateTime.Now.ToString(Twitter.DATETIME_FORMAT));
        }

        [TestMethod]
        public void ConvertTwitterDateToday() {
            string DisplayDate = Twitter.ConvertTwitterDateDisplay(DateTime.Now.ToString(Twitter.DATETIME_FORMAT));
            Assert.AreEqual(DisplayDate.Substring(0, 5), "Today");
            Debug.WriteLine(DisplayDate);
        }

        [TestMethod]
        public void ConvertTwitterDateNotToday() {
            string DisplayDate = Twitter.ConvertTwitterDateDisplay(DateTime.Now.AddDays(-1).ToString(Twitter.DATETIME_FORMAT));
            Assert.AreNotEqual(DisplayDate.Substring(0, 5), "Today");
            Debug.WriteLine(DisplayDate);
        }
    }
}
