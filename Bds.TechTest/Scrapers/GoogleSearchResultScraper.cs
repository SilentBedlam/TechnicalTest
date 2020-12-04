using Bds.TechTest.Scrapers.Results;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Bds.TechTest.Scrapers
{
    /// <summary>
    /// Scrapes search results from a Google search result page and extracts them as an object.
    /// </summary>
    public class GoogleSearchResultScraper : XPathResultScraperBase<GoogleSearchResult>
    {
        private const string SearchResultSelector = "//div[contains(concat(' ', @class, ' '), ' g ')]";

        /// <summary>
        /// Creates a new GoogleSearchResultScraper instance.
        /// </summary>
        public GoogleSearchResultScraper()
            : base(SearchResultSelector)
        {
            // Handled by constructor chaining.    
        }

        /// <inheritdoc />
        protected override GoogleSearchResult ExtractResult(HtmlNode htmlNode)
        {
            // Get the node containing the link to the search result and extract the URI.
            var linkNode = htmlNode.SelectNodes(".//a").First();
            var uri = ExtractUrlFromLinkNode(linkNode);
            var title = ExtractTitleFromLinkNode(linkNode);
            return new GoogleSearchResult(title, uri);
        }

        /// <summary>
        /// Extracts the URI associated with the search result.
        /// </summary>
        /// <param name="linkNode">The link ("a") node associated with the search result.</param>
        /// <returns>A Uri.</returns>
        private Uri ExtractUrlFromLinkNode(HtmlNode linkNode)
        {
            // The link contains only the relative path so add a simple scheme / host / etc. to keep UriBuilder happy.
            var hrefAttributeContent = WebUtility.HtmlDecode(linkNode.Attributes["href"].Value);
            var uriBuilder = new UriBuilder($"http://www.google.com{hrefAttributeContent}");

            // Extract the query string parts and find the one named "url". Extract the value from that part and return as a Uri.
            var queryParts = uriBuilder.Query.Split('&');
            var urlQueryPart = queryParts.Single(p => p.StartsWith("url", StringComparison.OrdinalIgnoreCase));
            var url = WebUtility.UrlDecode(urlQueryPart.Split('=')[1]);
            return new Uri(url);
        }

        /// <summary>
        /// Extracts the title associated with the search result.
        /// </summary>
        /// <param name="linkNode">The link ("a") node associated with the search result.</param>
        /// <returns>A Uri.</returns>
        private string ExtractTitleFromLinkNode(HtmlNode linkNode) => WebUtility.HtmlDecode(linkNode.SelectNodes(".//h3").First().InnerText);
    }
}
