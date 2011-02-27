using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class IOCSetup
    {
        public static void Setup() {
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
