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
    public class GoogleSearchResultScraper : XPathResultScraperBase<ISimpleSearchResult>
    {
        private const string SearchResultSelector = "//div[@id='search']//div[contains(concat(' ', @class, ' '), ' g ')]";
        private const string LinkNodeSelector = ".//div[contains(concat(' ', @class, ' '), ' rc ')]/div/a";

        /// <summary>
        /// Creates a new GoogleSearchResultScraper instance.
        /// </summary>
        public GoogleSearchResultScraper()
            : base(SearchResultSelector)
        {
            // Handled by constructor chaining.    
        }

        /// <inheritdoc />
        protected override bool TryExtractResult(HtmlNode htmlNode, out ISimpleSearchResult result)
        {
            result = null;

            try
            {
                // Get the node containing the link to the search result and extract the URI.
                var linkNode = htmlNode.SelectNodes(LinkNodeSelector).Single();

                // Ignore irrelevant nodes (easier than a super-complicated XPath expression):
                if (linkNode.Attributes["data-hveid"] != null)
                {
                    return false;
                }

                // Extract the details.
                var uri = ExtractUrlFromLinkNode(linkNode);
                var title = ExtractTitleFromLinkNode(linkNode);
                result = new SimpleSearchResult(title, uri);

                return true;
            }
            catch
            {
                return false;
            }
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

            // If Google are tracking the link, it will start with "/url", otherwise, it's plain.
            string url;

            if (hrefAttributeContent.StartsWith("/url?", StringComparison.OrdinalIgnoreCase))
            {
                var uriBuilder = new UriBuilder(hrefAttributeContent);

                // Extract the query string parts and find the one named "url". Extract the value from that part and return as a Uri.
                var queryParts = uriBuilder.Query.Split('&');
                var urlQueryPart = queryParts.Single(p => p.StartsWith("url", StringComparison.OrdinalIgnoreCase));
                url = WebUtility.UrlDecode(urlQueryPart.Split('=')[1]);
            }
            else
            {
                url = WebUtility.UrlDecode(hrefAttributeContent);
            }
            
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
