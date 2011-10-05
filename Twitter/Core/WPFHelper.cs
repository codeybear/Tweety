using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace Core
{
    public static class WPFHelper
    {
        /// <summary> Convert a string into an array of inline containing plain text and hyperlinks </summary>
        public static Inline[] CreateInlineTextWithLinks(string sText, EventHandler<System.Windows.RoutedEventArgs> ClickMethod) {
            string matchpattern = @"(?i)\b((?:[a-z][\w-]+:(?:/{1,3}|[a-z0-9%])|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'"".,<>?«»“”‘’]))";
            int LastMatchPos = 0;
            List<Inline> lines = new List<Inline>();
            MatchCollection matches = Regex.Matches(sText, matchpattern, RegexOptions.IgnoreCase);

            // Search for hyperlinks and add preceding text
            foreach (Match match in matches) {
                lines.Add(new Run(sText.Substring(LastMatchPos, match.Index - LastMatchPos)));
                lines.Add(CreateHyperLink(match.Value, match.Value, ClickMethod));
                LastMatchPos = match.Index + match.Length;
            }

            // Add any remaining text
            if (LastMatchPos == 0 || LastMatchPos != sText.Length)
                lines.Add(new Run(sText.Substring(LastMatchPos)));

            return lines.ToArray();
        }

        /// <summary> Create a WPF Hyperlink Class </summary>
        public static Hyperlink CreateHyperLink(string sURI, string sDescription, EventHandler<System.Windows.RoutedEventArgs> ClickMethod) {
            Hyperlink hyper = new Hyperlink();
            hyper.Inlines.Add(sDescription);

            if (!(sURI.StartsWith("http://") || sURI.StartsWith("https://")))
                sURI = "http://" + sURI;

            hyper.NavigateUri = new System.Uri(sURI);
            hyper.Click += new System.Windows.RoutedEventHandler(ClickMethod);
            return hyper;
        }

        /// <summary> Create an image from a url </summary>
        public static BitmapImage CreateImage(string ImageURL) {
            var bi = new BitmapImage();
            bi.BeginInit();
            // Ignore color profile as this can cause an exception on certain images
            bi.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreColorProfile;
            bi.UriSource = new Uri(ImageURL);
            bi.EndInit();

            return bi;
        }
    }
}
