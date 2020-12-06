using System;
using System.Net.Http;

namespace Bds.TechTest.Lib.Http
{
    /// <summary>
    /// Helper class which contains information about a request. Used by <see cref="HttpClientHelper" />.
    /// </summary>
    public sealed class HttpRequestInfo : IHttpRequestInfo
    {
        /// <summary>
        /// Creates a new HttpRequestInfo instance.
        /// </summary>
        /// <param name="uri">The Uri associated with the request.</param>
        /// <param name="httpMethod">The HttpMethod associated with the request.</param>
        internal HttpRequestInfo(Uri uri, HttpMethod httpMethod)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            HttpMethod = httpMethod;
        }

        /// <inheritdoc />
        public Uri Uri { get; }

        /// <inheritdoc />
        public HttpMethod HttpMethod { get; }
    }
}
