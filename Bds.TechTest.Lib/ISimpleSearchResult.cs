using System;

namespace Bds.TechTest.Lib
{
    /// <summary>
    /// Contract for a class which expresses a simple search result.
    /// </summary>
    public interface ISimpleSearchResult
    {
        /// <summary>
        /// The title associated with the search result.
        /// </summary>
        string PageTitle { get; set; }

        /// <summary>
        /// The Uri of the search result (i.e. the web resource to which the result points).
        /// </summary>
        Uri Uri { get; set; }
    }
}