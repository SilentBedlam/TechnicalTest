using Bds.TechTest.Comparers;
using System;

namespace Bds.TechTest.Results
{
    /// <summary>
    /// Represents the data associated with a simple search result.
    /// </summary>
    public class SimpleSearchResult : ISimpleSearchResult, IEquatable<ISimpleSearchResult>
    {
        /// <summary>
        /// Creates a new GoogleSearchResult instance.
        /// </summary>
        /// <param name="pageTitle">The title associated with the result.</param>
        /// <param name="uri">The URI to which the search result points.</param>
        public SimpleSearchResult(string pageTitle, Uri uri)
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

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is ISimpleSearchResult otherSimpleSearchResult))
            {
                return false;
            }

            // Use the IEquatable implementation once we've established the type.
            return Equals(otherSimpleSearchResult);
        }

        /// <inheritdoc />
        public bool Equals(ISimpleSearchResult other)
        {
            return SimpleSearchResultEqualityComparer.Instance.Equals(this, other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return SimpleSearchResultEqualityComparer.Instance.GetHashCode(this);
        }

        /// <inheritdoc />
        public override string ToString() => $"{PageTitle}: {Uri}";
    }
}