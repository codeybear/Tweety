using System.Windows.Documents;
using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Assert.IsTrue(lines[0] is Run);
            Assert.IsFalse(lines[1] is Run);
            Assert.IsTrue(lines[2] is Run);
            Assert.IsFalse(lines[3] is Run);
            Assert.IsTrue(lines[4] is Run);
            Assert.IsFalse(lines[5] is Run);
            Assert.IsTrue(lines[6] is Run);
            Assert.AreEqual(7, lines.Length);
        }
    }
}
