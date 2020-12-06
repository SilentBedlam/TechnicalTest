using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Bds.TechTest.Dto;
using Bds.TechTest.Lib.Http;
using Bds.TechTest.Lib.Results;
using Bds.TechTest.Lib.Scrapers;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace Bds.TechTest
{
    [ApiController]
    [Route("api/[controller]")]
    public class CombinedSearchController : ControllerBase
    {
        private const string Firefox83UserAgentString = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:83.0) Gecko/20100101 Firefox/83.0";

        private static readonly HttpClientHelper HttpClientHelper = new HttpClientHelper(NullLogger<HttpClientHelper>.Instance);
        private static readonly IResponseContentConverter<HtmlDocument> HttpContentConverter = new HtmlDocumentResponseContentConverter();

        /// <summary>
        /// 
        /// </summary>
        private static readonly IList<SearchEngineDefinition> SearchEngineDefinitions = new List<SearchEngineDefinition>
        {
            new SearchEngineDefinition 
            { 
                ProviderName = "Google", 
                UriFormat = "https://www.google.com/search?client=firefox-b-d&q={0}" ,
                ResultScraper = new GoogleSearchResultScraper(),
                UserAgentString = Firefox83UserAgentString
            },
            new SearchEngineDefinition 
            { 
                ProviderName = "Bing", 
                UriFormat = "https://www.bing.com/search?q={0}",
                ResultScraper = new BingSearchResultScraper(),
                UserAgentString = Firefox83UserAgentString
            }
        };
        
        /// <summary>
        /// POST: api/CombinedSearch
        /// Runs a search across all the relevant search engines and returns the combined result set.
        /// </summary>
        /// <param name="searchTerm">The term for which the search engines should be queried.</param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] CombinedSearchRequestDto searchRequestDto)
        {
            if (string.IsNullOrWhiteSpace(searchRequestDto.SearchTerm))
            {
                return BadRequest("The search term must be provided and may not be composed entirely of white space.");
            }

            // Compile a collection of tasks representing the requests for each search engine.
            var tasks = new List<Task>();
            
            foreach(var searchEngineDefinition in SearchEngineDefinitions)
            {
                var task = ExecuteSearchRequestAndScrapeResults(searchEngineDefinition, searchRequestDto.SearchTerm);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            // Check whether the tasks were successful. If they were, match the results and link.
            // TBC.


            // For now:
            var results = new CombinedSearchResultsDto();

            if (tasks.All(t => t.IsCompletedSuccessfully))
            {
                return new OkObjectResult(results);
            }

            return new StatusCodeResult(500);
        }

        /// <summary>
        /// Executes a search for a single search engine, retrieves the response and scrapes the results from the relevant document.
        /// </summary>
        /// <param name="searchEngineDefinition"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ISimpleSearchResult>> ExecuteSearchRequestAndScrapeResults(SearchEngineDefinition searchEngineDefinition, string searchTerm)
        {
            // Try to obtain the search results. Allow a maximum execution time of 5s per GET request.
            HtmlDocument searchResultsHtml;

            using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                // Get the HTML document containing the search results.
                var uriString = string.Format(searchEngineDefinition.UriFormat, WebUtility.UrlEncode(searchTerm));
                var uri = new UriBuilder(uriString).Uri;
                var response = await HttpClientHelper.Get(uri, HttpContentConverter, cancellationTokenSource.Token);

                if (!response.HasSuccessStatusCode)
                {
                    var errorMessage = $"A result set could not be obtained from the provider \"{searchEngineDefinition.ProviderName}\".";
                    throw new Exception(errorMessage);
                }

                searchResultsHtml = response.Response;
            }

            // Scrape the results from the response.
            var searchResults = searchEngineDefinition.ResultScraper.ScrapeResults(searchResultsHtml);
            return searchResults;
        }

        /// <summary>
        /// Defines the means of searching using a particular search engine.
        /// </summary>
        private class SearchEngineDefinition
        {
            /// <summary>
            /// A format string which can be transformed to a URI containing a search term.
            /// </summary>
            public string UriFormat { get; set; }

            /// <summary>
            /// The user agent string which should be used to retrieve the data.
            /// </summary>
            public string UserAgentString { get; set; }

            /// <summary>
            /// The name of the search engine provider (for reference).
            /// </summary>
            public string ProviderName { get; set; }

            /// <summary>
            /// An <see cref="ISearchResultScraper{ISimpleSearchResult}"/> instance which can extract the search results from the returned HTML page.
            /// </summary>
            public ISearchResultScraper<ISimpleSearchResult> ResultScraper { get; set; }
        }
    }
}
