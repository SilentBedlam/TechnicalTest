using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bds.TechTest.Lib.Http;
using Bds.TechTest.Lib.Results;
using Bds.TechTest.Lib.Scrapers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace Bds.TechTest
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombinedSearchController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly IList<SearchEngineDefinition> Searches = new List<SearchEngineDefinition>
        {
            new SearchEngineDefinition 
            { 
                ProviderName = "Google", 
                UriTemplate = "https://www.google.com/search?q={searchTerm}" ,
                ResultScraper = new GoogleSearchResultScraper()
            },
            new SearchEngineDefinition 
            { 
                ProviderName = "Bing", 
                UriTemplate = "https://www.bing.com/search?q={searchTerm}",
                ResultScraper = new BingSearchResultScraper()
            }
        };

        // POST: api/CombinedSearch
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string searchTerm)
        {
            var httpClientHelper = new HttpClientHelper(NullLogger<HttpClientHelper>.Instance);

            
        }


        private class SearchEngineDefinition
        {

            public string UriTemplate { get; set; }

            public string ProviderName { get; set; }

            public ISearchResultScraper<ISimpleSearchResult> ResultScraper { get; set; }
        }
    }
}
