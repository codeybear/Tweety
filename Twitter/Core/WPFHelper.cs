using System;
using System.Windows.Documents;

namespace Core
{
    static class WPFHelper
    {
        /// <summary> Convert a string into an array of inline containing plain text and hyperlinks </summary>
        public static Inline[] CreateInlineTextWithLinks(string sText, EventHandler<System.Windows.RoutedEventArgs> ClickMethod) {
            Paragraph para = new Paragraph();
            int iURLPos = 0;
            char[] EndOfURL = new char[] { ' ', ',' };

            do {
                iURLPos = sText.IndexOf("http://", StringComparison.CurrentCultureIgnoreCase);

                if (iURLPos > -1)
                    para.Inlines.Add(new Run(sText.Substring(0, iURLPos)));
                else
                    para.Inlines.Add(sText);

                if (iURLPos > -1) {
                    int iEndOfURLPos = sText.IndexOfAny(EndOfURL, iURLPos) - iURLPos;
                    if (iEndOfURLPos < 0) iEndOfURLPos = sText.Length - iURLPos;
                    
                    string sHyper = sText.Substring(iURLPos, iEndOfURLPos);
                    para.Inlines.Add(CreateHyperLink(sHyper, sHyper, ClickMethod));

                    sText = sText.Substring(iURLPos + iEndOfURLPos);
                }
            } while (iURLPos != -1);

            Inline[] lines = new Inline[para.Inlines.Count];
            para.Inlines.CopyTo(lines, 0);

            return lines;
        }

		/// <summary> Create a WPF Hyperlink class </summary>
        public static Hyperlink CreateHyperLink(string sURI, string sDescription, EventHandler<System.Windows.RoutedEventArgs> ClickMethod)
        {
            Hyperlink hyper = new Hyperlink();
            hyper.Inlines.Add(sDescription);
            hyper.NavigateUri = new System.Uri(sURI);
            hyper.Click += new System.Windows.RoutedEventHandler(ClickMethod);
            return hyper;
        }
    }	
}
