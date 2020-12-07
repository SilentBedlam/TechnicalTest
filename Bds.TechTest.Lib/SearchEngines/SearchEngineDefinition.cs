using Bds.TechTest.Lib.Results;
using Bds.TechTest.Lib.Scrapers;
using System;

namespace Bds.TechTest.Lib.SearchEngines
{
    /// <summary>
    /// Defines the means of searching using a particular search engine.
    /// </summary>
    /// <typeparam name="T">The Type of the search results returned by the search engine represented by this definition.</typeparam>
    public class SearchEngineDefinition<T> : ISearchEngineDefinition<T>
        where T : ISimpleSearchResult
    {
        /// <summary>
        /// Creates a new SearchEngineDefinition instance.
        /// </summary>
        /// <param name="providerName">The name of the search engine provider (for reference).</param>
        /// <param name="uriFormat">A format string which can be transformed to a URI containing a search term. Values will be used to replace 
        /// tokens in the format "{N}" where [N = 0: search term].</param>
        /// <param name="resultScraper">An <see cref="ISearchResultScraper{T}"/> instance which can extract the search results from the returned HTML page.</param>
        /// <param name="userAgentString">Optionally, the user agent string which should be used to retrieve the data.</param>
        public SearchEngineDefinition(string providerName, string uriFormat, ISearchResultScraper<T> resultScraper, string userAgentString = null)
        {
            if (string.IsNullOrWhiteSpace(providerName))
            {
                throw new ArgumentException("The provider name must be provided.", nameof(providerName));
            }

            if (string.IsNullOrWhiteSpace(uriFormat))
            {
                throw new ArgumentException("The URI format string must be provided.", nameof(uriFormat));
            }

            UriFormat = uriFormat;
            ProviderName = providerName;
            UserAgentString = userAgentString;
            ResultScraper = resultScraper ?? throw new ArgumentNullException(nameof(resultScraper));
        }

        /// <summary>
        /// A format string which can be transformed to a URI containing a search term.
        /// </summary>
        public string UriFormat { get; }

        /// <summary>
        /// The user agent string which should be used to retrieve the data.
        /// </summary>
        public string UserAgentString { get; set; }

        /// <summary>
        /// The name of the search engine provider (for reference).
        /// </summary>
        public string ProviderName { get; }

        /// <summary>
        /// An <see cref="ISearchResultScraper{T}"/> instance which can extract the search results from the returned HTML page.
        /// </summary>
        public ISearchResultScraper<T> ResultScraper { get; }
    }
}
