using Bds.TechTest.Lib.Results;
using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net;

namespace Bds.TechTest.Lib.Scrapers
{
    /// <summary>
    /// Scrapes search results from a Google search result page and extracts them as an object.
    /// </summary>
    public class BingSearchResultScraper : XPathResultScraperBase<ISimpleSearchResult>
    {
        private const string SearchResultSelector = "//ol[@id='b_results']/li[contains(concat(' ', @class, ' '), ' b_algo ')]";
        private const string LinkNodeSelector = ".//h2/a";

        /// <summary>
        /// Creates a new GoogleSearchResultScraper instance.
        /// </summary>
        public BingSearchResultScraper()
            : base(SearchResultSelector)
        {
            // Handled by constructor chaining.    
        }

        /// <inheritdoc />
        protected override bool TryExtractResult(HtmlNode htmlNode, int currentIndex, out ISimpleSearchResult result)
        {
            result = null;

            try
            {
                // Get the node containing the link to the search result and extract the URI.
                var linkNode = htmlNode.SelectNodes(LinkNodeSelector).Single();

                // Extract the details.
                var hrefAttributeContent = WebUtility.HtmlDecode(linkNode.Attributes["href"].Value);
                var uri = new Uri(hrefAttributeContent);
                var title = WebUtility.HtmlDecode(linkNode.InnerText);
                result = new SimpleSearchResult(title, uri, currentIndex);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
