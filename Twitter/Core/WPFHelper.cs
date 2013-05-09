using System;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace Tweety.Core
{
    public static class WPFHelper
    {
        /// <summary> Convert a string into an array of inline containing plain text and hyperlinks </summary>
        public static Inline[] CreateInlineTextWithLinks(string sText, EventHandler<System.Windows.RoutedEventArgs> clickMethod) {
            const string matchPattern = @"(?i)\b((?:[a-z][\w-]+:(?:/{1,3}|[a-z0-9%])|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'"".,<>?«»“”‘’]))";
            int lastMatchPos = 0;
            List<Inline> lines = new List<Inline>();
            MatchCollection matches = Regex.Matches(sText, matchPattern, RegexOptions.IgnoreCase);

            // Search for hyperlinks and add preceding text
            foreach (Match match in matches) {
                string decoded = WebHelper.UrlDecode(sText.Substring(lastMatchPos, match.Index - lastMatchPos));
                lines.Add(new Run(decoded));
                lines.Add(CreateHyperLink(match.Value, clickMethod));
                lastMatchPos = match.Index + match.Length;
            }

            // Add any remaining text
            if (lastMatchPos == 0 || lastMatchPos != sText.Length)
                lines.Add(new Run(WebHelper.UrlDecode(sText.Substring(lastMatchPos))));

            return lines.ToArray();
        }

        /// <summary> Create a WPF Hyperlink Class </summary>
        public static Hyperlink CreateHyperLink(string uri, EventHandler<System.Windows.RoutedEventArgs> clickMethod) {
            if (!(uri.StartsWith("http://") || uri.StartsWith("https://")))
                uri = "http://" + uri;

            Hyperlink hyper = new Hyperlink();
            hyper.Inlines.Add(uri);

            //hyper.NavigateUri = new System.Uri(sURI);
            hyper.Click += new System.Windows.RoutedEventHandler(clickMethod);
            return hyper;
        }

        /// <summary> Create an image from a url </summary>
        public static BitmapImage CreateImage(string imageURL) {
            var bi = new BitmapImage();
            bi.BeginInit();
            // Ignore color profile as this can cause an exception on certain images
            bi.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
            bi.UriSource = new Uri(imageURL);
            bi.EndInit();

            return bi;
        }
    }
}
