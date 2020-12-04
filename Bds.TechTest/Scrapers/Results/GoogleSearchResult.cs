using System;

namespace Bds.TechTest.Scrapers.Results
{
    /// <summary>
    /// Represents the data associated with a single Google search result.
    /// </summary>
    public class GoogleSearchResult
    {
        /// <summary>
        /// Creates a new GoogleSearchResult instance.
        /// </summary>
        /// <param name="pageTitle">The title associated with the result.</param>
        /// <param name="uri">The URI to which the search result points.</param>
        public GoogleSearchResult(string pageTitle, Uri uri)
        {
            if (string.IsNullOrWhiteSpace(pageTitle))
            {
                throw new ArgumentException("The page title must be specified.", nameof(pageTitle));
            }

            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            PageTitle = pageTitle;
            Uri = uri;
        }

        /// <summary>
        /// The title associated with the result.
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// The URI to which the search result points.
        /// </summary>
        public Uri Uri { get; set; }
    }
}