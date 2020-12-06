﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bds.TechTest.Dto;
using Bds.TechTest.Lib.Http;
using Bds.TechTest.Lib.Orchestration;
using Bds.TechTest.Lib.Results;
using Bds.TechTest.Lib.Scrapers;
using Bds.TechTest.Lib.SearchEngines;
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

        /// <summary>
        /// Definitions for the search engines supported by this controller.
        /// </summary>
        private static readonly IList<ISearchEngineDefinition<ISimpleSearchResult>> SearchEngineDefinitions = new List<ISearchEngineDefinition<ISimpleSearchResult>>
        {
            new SearchEngineDefinition<ISimpleSearchResult>(
                "Google", "https://www.google.com/search?client=firefox-b-d&q={0}", new GoogleSearchResultScraper(), Firefox83UserAgentString),
            new SearchEngineDefinition<ISimpleSearchResult>("Bing", "https://www.bing.com/search?q={0}", new BingSearchResultScraper())
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
            var searchEngineOrchestrators = SearchEngineDefinitions.Select(d => new SearchEngineOrchestrator<ISimpleSearchResult>(d, NullLoggerFactory.Instance));
            var tasks = searchEngineOrchestrators
                .Select(o => o.Execute(searchRequestDto.SearchTerm))
                // N.b. ToList() forces immediate evaluation of the iterator here (i.e. runs the tasks).
                .ToList();
            await Task.WhenAll(tasks);

            var results = new CombinedSearchResultsDto();

            // Check whether the tasks were successful. If they were, match the results and link.
            if (!tasks.All(t => t.IsCompletedSuccessfully))
            {
                // Unpack the exceptions and return.
                var exceptionMessages = tasks
                    .Where(t => t.IsFaulted && t.Exception != null)
                    .Select(t =>
                    {
                        var exceptionInner = t.Exception;

                        if (exceptionInner is AggregateException aggregateException)
                        {
                            exceptionInner = aggregateException.Flatten();
                        }

                        return exceptionInner;
                    })
                    .Aggregate(new List<string>(), (list, ex) => 
                    {
                        list.Add(ex.Message);
                        return list;
                    })
                    .ToList();

                results.Messages = exceptionMessages;
                return new ObjectResult(results) { StatusCode = 500 };
            }

            // Extract and match the results from each of the collections.
            var combinedSearchResultDtos = new List<CombinedSearchResultDto>();
            var searchResultSets = tasks.Select(t => t.Result as SearchResultsSet<ISimpleSearchResult>).ToList();
            var searchResultsByUriLookup = searchResultSets
                .SelectMany(srs => 
                    srs.SearchResults.Select(r => new { Result = r, SearchEngineDefinition = srs.SearchEngineDefinition }))
                .ToLookup(p => p.Result.Uri);
            
            foreach (var kvp in searchResultsByUriLookup)
            {
                var orderedSearchResults = searchResultsByUriLookup[kvp.Key].OrderBy(r => r.Result.Rank).ToArray();

                var combinedSearchResultDto = new CombinedSearchResultDto
                {
                    AverageRank = orderedSearchResults.Average(r => Convert.ToDouble(r.Result.Rank)),
                    RawSearchResults = orderedSearchResults.Select(r =>
                    {
                        return new RawSearchResultDto
                        {
                            ProviderName = r.SearchEngineDefinition.ProviderName,
                            PageTitle = r.Result.PageTitle,
                            Rank = r.Result.Rank,
                            Uri = r.Result.Uri
                        };
                    }),
                    PageTitle = orderedSearchResults.First().Result.PageTitle,
                    Uri = kvp.Key
                };

                combinedSearchResultDtos.Add(combinedSearchResultDto);
            }

            results.SearchResults = combinedSearchResultDtos.OrderBy(dto => dto.AverageRank).ToList();
            results.Messages = new List<string> { "All search engines were queried successfully." };
            return new OkObjectResult(results);
        }
    }
}
