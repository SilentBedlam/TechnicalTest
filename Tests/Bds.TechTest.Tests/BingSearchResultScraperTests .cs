using Bds.TechTest.Results;
using Bds.TechTest.Scrapers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Bds.TechTest.Tests
{
    [TestFixture]
    public class BingSearchResultScraperTests : SearchResultScraperTestBase
    {
        private static readonly IEnumerable<ISimpleSearchResult> FishSearchExpectedResults = new List<ISimpleSearchResult>
        {
            new SimpleSearchResult("Fish - Wikipedia", new Uri("https://en.wikipedia.org/wiki/Fish")),
            new SimpleSearchResult("Fish - Simple English Wikipedia, the free encyclopedia", new Uri("https://simple.wikipedia.org/wiki/Fish")),
            new SimpleSearchResult("fish | Definition, Species, & Facts | Britannica", new Uri("https://www.britannica.com/animal/fish")),
            new SimpleSearchResult("THE 10 BEST Seafood Restaurants in Cambridge, Updated ...", new Uri("https://www.tripadvisor.co.uk/Restaurants-g186225-c33-Cambridge_Cambridgeshire_England.html")),
            new SimpleSearchResult("Fish Pictures & Facts - Animals", new Uri("https://www.nationalgeographic.com/animals/fish/")),
            new SimpleSearchResult("FISH | meaning in the Cambridge English Dictionary", new Uri("https://dictionary.cambridge.org/dictionary/english/fish")),
        };

        private static readonly IEnumerable<ISimpleSearchResult> GinSearchExpectedResults = new List<ISimpleSearchResult>
        {
            new SimpleSearchResult("Grocery: Gin - Amazon.co.uk", new Uri("https://www.amazon.co.uk/gin/b?node=359896031")),
            new SimpleSearchResult("Gin - Tesco Groceries", new Uri("https://www.tesco.com/groceries/en-GB/shop/drinks/spirits/gin")),
            new SimpleSearchResult("Gin, Flavoured Gin & Dry Gin Selections | ALDI UK", new Uri("https://www.aldi.co.uk/c/spirits/Gin")),
            new SimpleSearchResult("Cambridge Distillery | Dedicated to Creating Outstanding Gins", new Uri("https://cambridgedistillery.co.uk/")),
        };

        private static readonly IEnumerable<SearchResultScraperTestCase> TestCases = new List<SearchResultScraperTestCase>
        {
            new SearchResultScraperTestCase("BingSearchResult-Fish.html", FishSearchExpectedResults),
            new SearchResultScraperTestCase("BingSearchResult-Gin.html", GinSearchExpectedResults)
        };

        private BingSearchResultScraper testInstance;

        /// <inheritdoc />
        protected override string ResourceNamePrefix => "BingSearchResult";

        [SetUp]
        public void Initialize()
        {
            testInstance = new BingSearchResultScraper();
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void CheckSearchResultsAreExtractedSuccessfully(SearchResultScraperTestCase testCase)
        {
            // Check the document was loaded successfully.
            Assert.That(SearchResultDocuments.ContainsKey(testCase.ResourceName));

            var document = SearchResultDocuments[testCase.ResourceName];
            var results = testInstance.ScrapeResults(document);

            Assert.That(results, Is.EquivalentTo(testCase.ExpectedSearchResults));
        }
    }
}