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
        }
    }
}
