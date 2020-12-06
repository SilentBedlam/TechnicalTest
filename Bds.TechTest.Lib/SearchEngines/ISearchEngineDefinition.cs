using Bds.TechTest.Lib.Scrapers;

namespace Bds.TechTest.Lib.SearchEngines
{
    /// <summary>
    /// Contract for a class which defines a search engine and its behaviours.
    /// </summary>
    /// <typeparam name="T">The Type of the search results returned by the search engine represented by this definition.</typeparam>
    public interface ISearchEngineDefinition<T>
    {
        /// <summary>
        /// A format string which can be transformed to a URI containing a search term.
        /// </summary>
        string UriFormat { get; }

        /// <summary>
        /// The user agent string which should be used to retrieve the data.
        /// </summary>
        string UserAgentString { get; }

        /// <summary>
        /// The name of the search engine provider (for reference).
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// An <see cref="ISearchResultScraper{T}"/> instance which can extract the search results from the returned HTML page.
        /// </summary>
        ISearchResultScraper<T> ResultScraper { get; }
    }
}
