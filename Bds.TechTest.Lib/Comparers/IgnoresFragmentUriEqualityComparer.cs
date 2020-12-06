using System;
using System.Collections.Generic;
using System.Threading;

namespace Bds.TechTest.Lib.Comparers
{
    /// <summary>
    /// Equality comparer for <see cref="Uri"/> which ignores the state of any Fragment provided.
    /// </summary>
    public class IgnoresFragmentUriEqualityComparer : EqualityComparer<Uri>
    {
        private static readonly Lazy<IgnoresFragmentUriEqualityComparer> lazyInstance =
            new Lazy<IgnoresFragmentUriEqualityComparer>(() => new IgnoresFragmentUriEqualityComparer(), LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Private constructor for singleton initialization.
        /// </summary>
        private IgnoresFragmentUriEqualityComparer()
        {
        }

        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        public static IgnoresFragmentUriEqualityComparer Instance => lazyInstance.Value;

        /// <inheritdoc />
        public override bool Equals(Uri first, Uri second)
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

            var uriBuilder1 = new UriBuilder(first) { Fragment = null };
            var uriBuilder2 = new UriBuilder(second) { Fragment = null };
            return uriBuilder1.Uri.Equals(uriBuilder2.Uri);
        }

        /// <inheritdoc />
        public override int GetHashCode(Uri uri)
        {
            if (uri is null)
            {
                return 0;
            }

            var uriBuilder = new UriBuilder(uri) { Fragment = null };
            return uriBuilder.Uri.GetHashCode();
        }
    }
}
