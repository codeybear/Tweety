namespace Tweety.Core
{
    public class IOCSetup
    {
        public static void Setup() {
            // Setup the Ioc container
            Ioc.Register<TweetSharp.TwitterService>(() => {
                var service = new TweetSharp.TwitterService(SettingHelper.ConsumerKey, SettingHelper.ConsumerSecret);
                service.AuthenticateWith(SettingHelper.Token, SettingHelper.TokenSecret);
                return service;
            });
        }
    }
}
