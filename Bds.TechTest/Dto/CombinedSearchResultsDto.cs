using System.Collections.Generic;

namespace Bds.TechTest.Dto
{
    /// <summary>
    /// Container class representing a combined search results set, along with any messages pertaining to the response.
    /// </summary>
    public class CombinedSearchResultsDto
    {
        /// <summary>
        /// A collection of messages associated with the response.
        /// </summary>
        public IEnumerable<string> Messages { get; set; }

        /// <summary>
        /// A collection of the combined search results.
        /// </summary>
        public IEnumerable<CombinedSearchResultDto> SearchResults { get; set; }
    }
}
