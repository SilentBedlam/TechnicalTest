using System;

namespace Bds.TechTest.Lib.Results
{
    /// <summary>
    /// Contract for a class which expresses a simple search result.
    /// </summary>
    public interface ISimpleSearchResult
    {
        /// <summary>
        /// The rank associated with the search result.
        /// </summary>
        int Rank { get; }

        /// <summary>
        /// The title associated with the search result.
        /// </summary>
        string PageTitle { get; }

        /// <summary>
        /// The Uri of the search result (i.e. the web resource to which the result points).
        /// </summary>
        Uri Uri { get; }
    }
}