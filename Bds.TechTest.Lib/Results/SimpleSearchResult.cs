using Bds.TechTest.Lib.Comparers;
using System;

namespace Bds.TechTest.Lib.Results
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
        public SimpleSearchResult(string pageTitle, Uri uri, int rank)
        {
            if (string.IsNullOrWhiteSpace(pageTitle))
            {
                throw new ArgumentException("The page title must be specified.", nameof(pageTitle));
            }

            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (rank < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rank), "The rank must be a positive integer.");
            }

            PageTitle = pageTitle;
            Uri = uri;
            Rank = rank;
        }

        /// <summary>
        /// The title associated with the result.
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// The URI to which the search result points.
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// The rank associated with this search result.
        /// </summary>
        public int Rank { get; }

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