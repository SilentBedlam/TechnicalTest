using Bds.TechTest.Lib.Results;
using Bds.TechTest.Lib.SearchEngines;
using System;
using System.Collections.Generic;

namespace Bds.TechTest.Lib.Orchestration
{
    /// <summary>
    /// Class representing the set of search results returned by a single search.
    /// </summary>
    /// <typeparam name="T">The Type of the search results returned by this instance.</typeparam>
    public class SearchResultsSet<T>
        where T : ISimpleSearchResult
    {
        /// <summary>
        /// Creates a new SearchResultsSet instance.
        /// </summary>
        /// <param name="searchEngineDefinition">The definition of the search engine which produced the results.</param>
        /// <param name="searchResults">The search results returned from the search engine.</param>
        public SearchResultsSet(ISearchEngineDefinition<T> searchEngineDefinition, IEnumerable<T> searchResults)
        {
            SearchEngineDefinition = searchEngineDefinition ?? throw new ArgumentNullException(nameof(searchEngineDefinition));
            SearchResults = searchResults ?? throw new ArgumentNullException(nameof(searchResults));
        }

        /// <summary>
        /// The definition of the search engine which produced the results.
        /// </summary>
        public ISearchEngineDefinition<T> SearchEngineDefinition { get; }

        /// <summary>
        /// The search results returned from the search engine.
        /// </summary>
        public IEnumerable<T> SearchResults { get; }
    }
}
