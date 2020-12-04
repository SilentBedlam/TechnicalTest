using HtmlAgilityPack;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Bds.TechTest.Tests
{
    /// <summary>
    /// Base class for scraper tests all having a simillar format.
    /// </summary>
    public abstract class SearchResultScraperTestBase
    {
        /// <summary>
        /// The resource name prefix for the test in question.
        /// </summary>
        protected abstract string ResourceNamePrefix { get; }

        /// <summary>
        /// 
        /// </summary>
        protected IDictionary<string, HtmlDocument> SearchResultDocuments { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // Get the resources for this assembly and extract the relevant HTML documents to use as test data.
            SearchResultDocuments = new Dictionary<string, HtmlDocument>();
         
            var assembly = Assembly.GetAssembly(typeof(SearchResultScraperTestBase));
            var resourceNames = assembly.GetManifestResourceNames();

            foreach (var embeddedResourceName in resourceNames)
            {
                // Ignore resources not relevant to these tests.
                if (!embeddedResourceName.Contains(ResourceNamePrefix, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!embeddedResourceName.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
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

                        SearchResultDocuments.Add(shortResourceName, htmlDocument);
                    }
                }
            }
        }

        /// <summary>
        /// Class representing a single test case for the scraper.
        /// </summary>
        public class SearchResultScraperTestCase
        {
            /// <summary>
            /// Creates a new SearchResultScraperTestCase instance.
            /// </summary>
            /// <param name="resourceName">The name of the embedded resource which provides the search result HTML content for the test.</param>
            /// <param name="expectedSearchResults">The search results expected by the test case.</param>
            public SearchResultScraperTestCase(string resourceName, IEnumerable<ISimpleSearchResult> expectedSearchResults)
            {
                ResourceName = resourceName;
                ExpectedSearchResults = expectedSearchResults;
            }

            /// <summary>
            /// The name of the embedded resource which provides the search result HTML content for the test.
            /// </summary>
            public string ResourceName { get; private set; }

            /// <summary>
            /// The search results expected by the test case.
            /// </summary>
            public IEnumerable<ISimpleSearchResult> ExpectedSearchResults { get; private set; }
        }
    }
}