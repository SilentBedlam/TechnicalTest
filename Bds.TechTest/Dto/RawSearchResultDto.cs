using System;

namespace Bds.TechTest.Dto
{
    /// <summary>
    /// Container class for the raw search result associated with a particular provider.
    /// </summary>
    public class RawSearchResultDto
    {
        /// <summary>
        /// The name of the provider which gave rise to this search result.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// The rank associated with the search result.
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// The title associated with the search result.
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// The Uri of the search result (i.e. the web resource to which the result points).
        /// </summary>
        public Uri Uri { get; set; }
    }
}
