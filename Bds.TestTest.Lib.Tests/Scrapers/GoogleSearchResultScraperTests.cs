using Bds.TechTest.Lib.Results;
using Bds.TechTest.Lib.Scrapers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Bds.TechTest.Lib.Tests
{
    [TestFixture]
    public class GoogleSearchResultScraperTests : SearchResultScraperTestBase
    {
        private static readonly IEnumerable<ISimpleSearchResult> FishSearchExpectedResults = new List<ISimpleSearchResult>
        {
            new SimpleSearchResult("The Best Fish Names of 2019 for Bettas, Guppies, Goldfish, and More", new Uri("https://www.rover.com/blog/best-fish-names/")),
            new SimpleSearchResult("Fish - Annotated classification | Britannica", new Uri("https://www.britannica.com/animal/fish/Annotated-classification")),
            new SimpleSearchResult("5 Characteristics That All Fish Have in Common - Sciencing", new Uri("https://sciencing.com/5-characteristics-fish-common-12059701.html")),
            new SimpleSearchResult("Description and Physical Characteristics of Fish - All Other Pets ...", new Uri("https://www.msdvetmanual.com/all-other-pets/fish/description-and-physical-characteristics-of-fish")),
            new SimpleSearchResult("Fishing - Wikipedia", new Uri("https://en.wikipedia.org/wiki/Fishing")),
            new SimpleSearchResult("Fish Music", new Uri("https://fishmusic.scot/")),
            new SimpleSearchResult("fish | Definition, Species, & Facts | Britannica", new Uri("https://www.britannica.com/animal/fish")),
            new SimpleSearchResult("Fish - Home | Facebook", new Uri("https://www.facebook.com/derek.dick/")),
            new SimpleSearchResult("Fish out of water - YouTube", new Uri("https://www.youtube.com/watch?v=mKxRe0hAQmg")),
            new SimpleSearchResult("Fish | Environment | The Guardian", new Uri("https://www.theguardian.com/environment/fish")),
        };

        private static readonly IEnumerable<ISimpleSearchResult> GinSearchExpectedResults = new List<ISimpleSearchResult>
        {
            new SimpleSearchResult("Gin - Tesco Groceries", new Uri("https://www.tesco.com/groceries/en-GB/shop/drinks/spirits/gin")),
            new SimpleSearchResult("Gin - Master of Malt", new Uri("https://www.masterofmalt.com/gin/")),
            new SimpleSearchResult("Gin - British Gin, Popular Gin Brands - Majestic Wine", new Uri("https://www.majestic.co.uk/gin")),
            new SimpleSearchResult("Gin, Flavoured Gin & Dry Gin Selections | ALDI UK", new Uri(" https://www.aldi.co.uk/c/Gin")),
            new SimpleSearchResult("Grocery: Gin - Amazon.co.uk", new Uri("https://www.amazon.co.uk/gin/b?ie=UTF8&node=359896031")),
            new SimpleSearchResult("The Differences Between Vodka and Gin, Explained | Gin Vs. Vodka ...", new Uri("https://vinepair.com/articles/differences-gin-vs-vodka/")),
            new SimpleSearchResult("Health benefits of gin: 8 reasons gin is good for you - GoodtoKnow", new Uri("https://www.goodtoknow.co.uk/wellbeing/reasons-gin-is-good-for-you-103517")),
            new SimpleSearchResult("What Is The Difference Between Gin And Vodka? » 5 Facts!", new Uri("https://homebrewadvice.com/gin-vodka-difference")),
            new SimpleSearchResult("Gin is back: Why it's on the rise and getting a revival - The Typsy Blog", new Uri("https://blog.typsy.com/gin-is-back-why-its-on-the-rise-and-getting-a-revival")),
            new SimpleSearchResult("Best Gin Flavours - 70+ Flavoured Gin Bottles Available To Buy", new Uri("https://www.delish.com/uk/cocktails-drinks/g29069585/flavoured-gin/")),
            new SimpleSearchResult("Gin - ASDA Groceries", new Uri("https://groceries.asda.com/aisle/spirits/gin/_/1896568675")),
        };

        private static readonly IEnumerable<SearchResultScraperTestCase> TestCases = new List<SearchResultScraperTestCase>
        {
            new SearchResultScraperTestCase("GoogleSearchResult-Fish.html", FishSearchExpectedResults),
            new SearchResultScraperTestCase("GoogleSearchResult-Gin.html", GinSearchExpectedResults)
        };

        private GoogleSearchResultScraper testInstance;

        /// <inheritdoc />
        protected override string ResourceNamePrefix => "GoogleSearchResult";

        [SetUp]
        public void Initialize()
        {
            testInstance = new GoogleSearchResultScraper();
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