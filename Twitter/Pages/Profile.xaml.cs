using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Tweety.Core;
using TweetSharp;

namespace Pages
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        public Profile(TwitterUser user) {
            InitializeComponent();
            txtScreenName.Text = "@" + user.ScreenName;
            txtName.Text =  user.Name;
            txtProfile.Text = user.Description;
            ImageProfile.Source = WPFHelper.CreateImage(user.ProfileImageUrl);
        }
    }
}
