using Bds.TechTest.Lib.Results;
using System.Threading.Tasks;

namespace Bds.TechTest.Lib.Orchestration
{
    /// <summary>
    /// Contract for a class which can orchestrate a search.
    /// </summary>
    /// <typeparam name="T">The Type of the search results returned by this instance.</typeparam>
    public interface ISearchEngineOrchestrator<T>
        where T : ISimpleSearchResult
    {
        /// <summary>
        /// Executes a search for the specified search term, retrieves the response and scrapes the results from the relevant document.
        /// </summary>
        /// <param name="searchTerm">The search term for which to execute the search.</param>
        /// <returns>A Task returning a collection of search results.</returns>
        Task<SearchResultsSet<T>> Execute(string searchTerm);
    }
}