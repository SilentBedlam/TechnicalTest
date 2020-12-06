using System;
using System.Collections.Generic;
using System.Threading;

namespace Bds.TechTest.Lib.Comparers
{
    /// <summary>
    /// A default implementation of an equality comparer for simple search results.
    /// </summary>
    public class SimpleSearchResultEqualityComparer : EqualityComparer<ISimpleSearchResult>
    {
        private static readonly Lazy<SimpleSearchResultEqualityComparer> lazyInstance =
            new Lazy<SimpleSearchResultEqualityComparer>(() => new SimpleSearchResultEqualityComparer(), LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Private constructor for singleton initialization.
        /// </summary>
        private SimpleSearchResultEqualityComparer()
        {
        }

        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        public static SimpleSearchResultEqualityComparer Instance => lazyInstance.Value;

        /// <inheritdoc />
        public override bool Equals(ISimpleSearchResult first, ISimpleSearchResult second)
        {
            if (first == null && second == null)
            {
                return true;
            }

            if (first == null || second == null)
            {
                return false;
            }

            if (ReferenceEquals(first, second))
            {
                return true;
            }

            return first.Uri.Equals(second.Uri) && StringComparer.OrdinalIgnoreCase.Equals(first.PageTitle, second.PageTitle);
        }

        /// <inheritdoc />
        public override int GetHashCode(ISimpleSearchResult simpleSearchResult)
        {
            if (simpleSearchResult is null)
            {
                return 0;
            }

            unchecked
            {
                var result = 179;
                // N.b. The Uri.GetHashCode() method has a very weird implementation. I'm presumably missing something about
                // what it's doing, but for now, just treat the URLs as identical if they look the same as strings...
                result = (result * 863) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(simpleSearchResult.Uri.OriginalString);
                result = (result * 863) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(simpleSearchResult.PageTitle);
                return result;
            }
        }
    }
}
