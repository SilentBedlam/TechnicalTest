using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace Bds.TechTest.Scrapers
{
    /// <summary>
    /// Base class for a class which can identify search results in an HTML page using an XPath expression and extract them to an object representing their content.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class XPathResultScraperBase<T> : ISearchResultScraper<T>
        where T : class
    {
        private readonly string resultXPathSelector;

        /// <summary>
        /// Protected constructor to enforce the provision of required resources.
        /// </summary>
        /// <param name="resultXPathSelector">A string containing an XPath selector which will extract the search results.</param>
        protected XPathResultScraperBase(string resultXPathSelector)
        {
            if (string.IsNullOrWhiteSpace(resultXPathSelector))
            {
                throw new ArgumentException("The page title must be specified.", nameof(resultXPathSelector));
            }

            this.resultXPathSelector = resultXPathSelector;
        }

        /// <inheritdoc />
        public IEnumerable<T> ScrapeResults(HtmlDocument htmlDocument)
        {
            var resultNodes = htmlDocument.DocumentNode.SelectNodes(resultXPathSelector);
            var list = new List<T>(resultNodes.Count);

            foreach (var resultNode in resultNodes)
            {
                var extractionSuccessful = TryExtractResult(resultNode, out T result);

                if (extractionSuccessful)
                {
                    list.Add(result);
                }
            }

            return list;
        }

        /// <summary>
        /// Extracts the content of the result to an instance
        /// </summary>
        /// <param name="htmlNode">The HTML node containing the search result.</param>
        /// <param name="result">Output parameter which will contain the result if extraction was successful.</param>
        /// <returns>True if the extraction of the result was successful; otherwise false.</returns>
        protected abstract bool TryExtractResult(HtmlNode htmlNode, out T result);
    }
}
