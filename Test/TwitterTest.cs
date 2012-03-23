using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Tweety.Core;

namespace Test
{
    [TestClass]
    public class TwitterTest
    {
        private TestContext _testContextInstance;

        #region Additional test attributes

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) {
            // Setup the Ioc container
            Ioc.Register<OAuthTwitter>(() => {
                OAuthTwitter oAuthTwitter = new OAuthTwitter();
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
        [TestMethod]
        public void GetFriendsTimelineTest() {
            List<Status> actual = Twitter.GetHomeTimeline();
            Assert.IsTrue(actual.Count > 0);
            actual.ForEach(status => Debug.WriteLine("{0}***{1}***{2}", status.CreatedAt, status.User.Name, status.Text));
        }

        [TestMethod]
        public void ConvertTwitterDate() {
            string displayDate = Twitter.ConvertTwitterDateDisplay(DateTime.Now.ToString(Twitter.DATETIME_FORMAT));
        }

        [TestMethod]
        public void ConvertTwitterDateToday() {
            string displayDate = Twitter.ConvertTwitterDateDisplay(DateTime.Now.ToString(Twitter.DATETIME_FORMAT));
            Assert.AreEqual(displayDate.Substring(0, 5), "Today");
            Debug.WriteLine(displayDate);
        }

        [TestMethod]
        public void ConvertTwitterDateNotToday() {
            string displayDate = Twitter.ConvertTwitterDateDisplay(DateTime.Now.AddDays(-1).ToString(Twitter.DATETIME_FORMAT));
            Assert.AreNotEqual(displayDate.Substring(0, 5), "Today");
            Debug.WriteLine(displayDate);
        }

        [TestMethod]
        public void MyTestMethod() {

        }
    }
}
