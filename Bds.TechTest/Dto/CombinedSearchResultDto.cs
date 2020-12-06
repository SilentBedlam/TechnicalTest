using System;
using System.Collections.Generic;

namespace Bds.TechTest.Dto
{
    /// <summary>
    /// Container class for a combined search result, associated with the "raw" results which gave rise to it.
    /// </summary>
    public class CombinedSearchResultDto
    {
        /// <summary>
        /// The title associated with the search result.
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// The primary Uri for the search result.
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// The average rank associated with the result.
        /// </summary>
        public double AverageRank { get; set; }

        /// <summary>
        /// A collection of "raw" search results which gave rise to this result.
        /// </summary>
        public IEnumerable<RawSearchResultDto> RawSearchResults { get; set; }
    }
}
