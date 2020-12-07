using Bds.TechTest.Lib.Results;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;

namespace Bds.TestTest.Lib.Tests.Results
{
    [TestFixture]
    public class SimpleSearchResultTests
    {
        private static readonly Uri TestUri = new Uri("http://www.google.com/");

        [Test]
        public void CheckConstructionWithMissingPageTitleIsRejected()
        {
            Assert.Throws<ArgumentException>(() => new SimpleSearchResult(null, TestUri, 1));
        }

        [Test]
        public void CheckConstructionWithEmptyPageTitleIsRejected()
        {
            Assert.Throws<ArgumentException>(() => new SimpleSearchResult(string.Empty, TestUri, 1));
        }

        [Test]
        public void CheckConstructionWithWhitespacePageTitleIsRejected()
        {
            Assert.Throws<ArgumentException>(() => new SimpleSearchResult("  \r\n\t  ", TestUri, 1));
        }

        [Test]
        public void CheckConstructionWithMissingUriIsRejected()
        {
            Assert.Throws<ArgumentNullException>(() => new SimpleSearchResult("Page Title 1", null, 1));
        }

        [Test]
        public void CheckConstructionWithNegativeRankIsRejected()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SimpleSearchResult("Page Title 1", TestUri, -42));
        }

        [Test]
        public void CheckConstructionWithZeroRankIsRejected()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SimpleSearchResult("Page Title 1", TestUri, 0));
        }
    }
}
