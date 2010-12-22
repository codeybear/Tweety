using System.Windows;
using Core;

namespace Pages {
    public partial class App : Application {
        App() {
            // Setup the Ioc container
            Ioc.Register<oAuthTwitter>(() => {
                oAuthTwitter oAuthTwitter = new oAuthTwitter();
                oAuthTwitter.ConsumerKey = SettingHelper.ConsumerKey;
                oAuthTwitter.ConsumerSecret = SettingHelper.ConsumerSecret;
                oAuthTwitter.Token = SettingHelper.Token;
                oAuthTwitter.TokenSecret = SettingHelper.TokenSecret;
                return oAuthTwitter;
                });
        }
    }
}
