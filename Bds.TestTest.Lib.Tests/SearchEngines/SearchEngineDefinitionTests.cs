using Bds.TechTest.Lib.Results;
using Bds.TechTest.Lib.Scrapers;
using Bds.TechTest.Lib.SearchEngines;
using NSubstitute;
using NUnit.Framework;
using System;

namespace Bds.TestTest.Lib.Tests.SearchEngines
{
    [TestFixture]
    public class SearchEngineDefinitionTests
    {
        private const string MockUserAgentString = "[Some UA String]";

        private static readonly ISearchResultScraper<ISimpleSearchResult> MockSearchResultScraper =
            Substitute.For<ISearchResultScraper<ISimpleSearchResult>>();

        [Test]
        public void CheckConstructionWithMissingProviderNameIsRejected()
        {
            Assert.Throws<ArgumentException>(() =>
                new SearchEngineDefinition<ISimpleSearchResult>(null, "http://www.test-search-engine.com/?q={0}", MockSearchResultScraper, MockUserAgentString));
        }

        [Test]
        public void CheckConstructionWithEmptyProviderNameIsRejected()
        {
            Assert.Throws<ArgumentException>(() =>
                new SearchEngineDefinition<ISimpleSearchResult>(string.Empty, "http://www.test-search-engine.com/?q={0}", MockSearchResultScraper, MockUserAgentString));
        }

        [Test]
        public void CheckConstructionWithWhitespaceProviderNameIsRejected()
        {
            Assert.Throws<ArgumentException>(() =>
                new SearchEngineDefinition<ISimpleSearchResult>("  \t\r\n  ", "http://www.test-search-engine.com/?q={0}", MockSearchResultScraper, MockUserAgentString));
        }

        [Test]
        public void CheckConstructionWithMissingUriFormatIsRejected()
        {
            Assert.Throws<ArgumentException>(() =>
                new SearchEngineDefinition<ISimpleSearchResult>("TestSearchEngine", null, MockSearchResultScraper, MockUserAgentString));
        }

        [Test]
        public void CheckConstructionWithEmptUriFormatIsRejected()
        {
            Assert.Throws<ArgumentException>(() =>
                new SearchEngineDefinition<ISimpleSearchResult>("TestSearchEngine", string.Empty, MockSearchResultScraper, MockUserAgentString));
        }

        [Test]
        public void CheckConstructionWithWhitespaceIsRejected()
        {
            Assert.Throws<ArgumentException>(() =>
                new SearchEngineDefinition<ISimpleSearchResult>("TestSearchEngine", "  \t\r\n  ", MockSearchResultScraper, MockUserAgentString));
        }

        [Test]
        public void CheckConstructionWithMissingScraperIsRejected()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SearchEngineDefinition<ISimpleSearchResult>("TestSearchEngine", "http://www.test-search-engine.com/?q={0}", null, MockUserAgentString));
        }

        [Test]
        public void CheckConstructionWithMissingUserAgentStringIsPermitted()
        {
            Assert.DoesNotThrow(() =>
                new SearchEngineDefinition<ISimpleSearchResult>("TestSearchEngine", "http://www.test-search-engine.com/?q={0}", MockSearchResultScraper, null));
        }
    }
}
