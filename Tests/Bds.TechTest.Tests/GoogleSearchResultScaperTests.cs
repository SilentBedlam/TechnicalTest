using Bds.TechTest.Scrapers;
using HtmlAgilityPack;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Bds.TechTest.Tests
{
    public class GoogleSearchResultScaperTests
    {
        private const string ResourceNamePrefix = "GoogleSearchResult";

        private IDictionary<string, HtmlDocument> searchResultDocuments;
        private GoogleSearchResultScraper testInstance;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // Get the resources for this assembly and extract the relevant HTML documents to use as test data.

            searchResultDocuments = new Dictionary<string, HtmlDocument>();

         
            var assembly = Assembly.GetAssembly(typeof(GoogleSearchResultScaperTests));
            var resourceNames = assembly.GetManifestResourceNames();

            foreach (var embeddedResourceName in resourceNames)
            {
                // Ignore resources not relevant to these tests.
                if (!embeddedResourceName.Contains(ResourceNamePrefix, System.StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!embeddedResourceName.EndsWith(".html", System.StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Calculate a helpful short name to use as the key.
                var startPosition = embeddedResourceName.IndexOf(ResourceNamePrefix);
                var shortResourceName = embeddedResourceName.Substring(startPosition);

                // Read the contents of the resource as an HTML document and save to the dictionary.
                using (var stream = assembly.GetManifestResourceStream(embeddedResourceName))
                {
                    using (var streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        var stringContents = streamReader.ReadToEnd();
                        var htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(stringContents);

                        searchResultDocuments.Add(shortResourceName, htmlDocument);
                    }
                }
            }
        }

        [SetUp]
        public void Initialize()
        {
            testInstance = new GoogleSearchResultScraper();
        }


        [Test]
        public void CheckSearchResultsAreExtractedSuccessfully()
        {
            // Check the document was loaded successfully.
            Assert.That(searchResultDocuments.ContainsKey("GoogleSearchResult-Fish.html"));

            var document = searchResultDocuments["GoogleSearchResult-Fish.html"];
            var results = testInstance.ScrapeResults(document);

            Assert.That(results, Is.Not.Empty);
            Assert.That(results.Count(), Is.EqualTo(14));

        }
    }
}