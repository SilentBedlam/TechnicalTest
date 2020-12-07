using Bds.TechTest.Lib.Http;
using Bds.TechTest.Lib.Results;
using Bds.TechTest.Lib.SearchEngines;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Bds.TechTest.Lib.Orchestration
{
    /// <summary>
    /// Orchestrates search requests for the specified search engine, returning the results.
    /// </summary>
    /// <typeparam name="T">The Type of the search results returned by this instance.</typeparam>
    public class SearchEngineOrchestrator<T> : ISearchEngineOrchestrator<T>
        where T : ISimpleSearchResult
    {
        private static readonly IResponseContentConverter<HtmlDocument> HttpContentConverter = new HtmlDocumentResponseContentConverter();

        private readonly ISearchEngineDefinition<T> searchEngineDefinition;
        private readonly IHttpClientHelper httpClientHelper;

        /// <summary>
        /// Creates a new SearchEngineOrchestrator instance.
        /// </summary>
        /// <param name="searchEngineDefinition">The ISearchEngineDefinition with which this orchestrator is associated.</param>
        /// <param name="httpClientHelperProvider">An IHttpClientHelperProvider to be used by this instance.</param>
        public SearchEngineOrchestrator(ISearchEngineDefinition<T> searchEngineDefinition, IHttpClientHelperProvider httpClientHelperProvider)
        {
            this.searchEngineDefinition = searchEngineDefinition ?? throw new ArgumentNullException(nameof(searchEngineDefinition));

            // Create a properly-configured HttpClientHelper instance.
            var defaultHeaders = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(searchEngineDefinition.UserAgentString))
            {
                defaultHeaders.Add("User-Agent", searchEngineDefinition.UserAgentString);
            }

            httpClientHelper = httpClientHelperProvider.GetHttpClientHelper();
            httpClientHelper.SetDefaultRequestHeaders(defaultHeaders);
        }

        /// <inheritdoc />
        public async Task<SearchResultsSet<T>> Execute(string searchTerm)
        {
            // Try to obtain the search results. Allow a maximum execution time of 5s per GET request.
            HtmlDocument searchResultsHtml;

            using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                // Get the HTML document containing the search results.
                var uriString = string.Format(searchEngineDefinition.UriFormat, WebUtility.UrlEncode(searchTerm));
                var uri = new UriBuilder(uriString).Uri;
                var response = await httpClientHelper.Get(uri, HttpContentConverter, cancellationTokenSource.Token);

                if (!response.HasSuccessStatusCode)
                {
                    var errorMessage = $"A result set could not be obtained from the provider \"{searchEngineDefinition.ProviderName}\".";
                    throw new Exception(errorMessage);
                }

                searchResultsHtml = response.Response;
            }

            // Scrape the results from the response.
            var searchResults = searchEngineDefinition.ResultScraper.ScrapeResults(searchResultsHtml);
            return new SearchResultsSet<T>(searchEngineDefinition, searchResults);
        }
    }
}
