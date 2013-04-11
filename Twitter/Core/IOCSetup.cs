namespace Tweety.Core
{
    public class IOCSetup
    {
        public static void Setup() {
            // Setup the Ioc container
            Ioc.Register<OAuthTwitter>(() => {
                OAuthTwitter oAuthTwitter = new OAuthTwitter();
                oAuthTwitter.ConsumerKey = SettingHelper.ConsumerKey;
                oAuthTwitter.ConsumerSecret = SettingHelper.ConsumerSecret;
                oAuthTwitter.Token = SettingHelper.Token;
                oAuthTwitter.TokenSecret = SettingHelper.TokenSecret;
                return oAuthTwitter;
            });

            Ioc.Register<TweetSharp.TwitterService>(() => {
                var service = new TweetSharp.TwitterService(SettingHelper.ConsumerKey, SettingHelper.ConsumerSecret);
                service.AuthenticateWith(SettingHelper.Token, SettingHelper.TokenSecret);
                return service;
            });
        }
    }
}
