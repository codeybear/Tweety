using System.Windows.Documents;
using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Test
{
    [TestClass]
    public class WPFHelperTest
    {
        [TestMethod]
        public void CreateInlineTextWithLinks() {
            string search = @"A bit of text here www.google.co.uk. and more.https://www.google.co.uk oh 
                              wow here's another one http://www.pjcsoftware.co.uk/. here's some more";

            Inline[] lines = WPFHelper.CreateInlineTextWithLinks(search, (o, e) => { });
            Assert.IsTrue(lines[0].GetType().FullName == "System.Windows.Documents.Run");
            Assert.IsTrue(lines[1].GetType().FullName == "System.Windows.Documents.Hyperlink");
            Assert.IsTrue(lines[2].GetType().FullName == "System.Windows.Documents.Run");
            Assert.IsTrue(lines[3].GetType().FullName == "System.Windows.Documents.Hyperlink");
            Assert.IsTrue(lines[4].GetType().FullName == "System.Windows.Documents.Run");
            Assert.IsTrue(lines[5].GetType().FullName == "System.Windows.Documents.Hyperlink");
            Assert.IsTrue(lines[6].GetType().FullName == "System.Windows.Documents.Run");
            Assert.AreEqual(7, lines.Length);
        }
    }
}
