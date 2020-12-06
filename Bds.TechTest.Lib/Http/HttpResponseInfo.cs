using System;
using System.Net;

namespace Bds.TechTest.Lib.Http
{
    /// <summary>
    /// A container class which packages certain useful data pertaining to an HTTP response into an easy-to-manipulate form.
    /// </summary>
    public class HttpResponseInfo : IHttpResponseInfo
    {
        /// <summary>
        /// Creates a new HttpResponseInfo instance.
        /// </summary>
        /// <param name="statusCode">The status code associated with the response.</param>
        /// <param name="requestInfo">Information describing the request which gave rise to this response.</param>
        internal HttpResponseInfo(HttpStatusCode statusCode, IHttpRequestInfo requestInfo)
        {
            StatusCode = statusCode;
            RequestInfo = requestInfo ?? throw new ArgumentNullException(nameof(requestInfo));
        }

        /// <inheritdoc />
        public HttpStatusCode StatusCode { get; }

        /// <inheritdoc />
        public IHttpRequestInfo RequestInfo { get; }

        /// <inheritdoc />
        public bool HasSuccessStatusCode => (int)StatusCode >= 200 && (int)StatusCode <= 299;
    }

    /// <sumamry>
    /// Generic version of the HttpResponseInfo which allows a response object to be returned.
    /// </sumamry>
    /// <typeparam name="T">The Type of the deserialized response.</typeparam>
    public sealed class HttpResponseInfo<T> : HttpResponseInfo, IHttpResponseInfo<T>
        where T : class
    {
        /// <summary>
        /// Creates a new HttpResponseInfo instance.
        /// </summary>
        /// <param name="statusCode">The status code associated with the response.</param>
        /// <param name="requestInfo">Information describing the request which gave rise to this response.</param>
        /// <param name="response">Optionally, the deserialized response of the response.</param>
        internal HttpResponseInfo(HttpStatusCode statusCode, IHttpRequestInfo requestInfo, T response = null)
            : base(statusCode, requestInfo)
        {
            Response = response;
        }

        /// <summary>
        /// The deserialized response of the response.
        /// </summary>
        public T Response { get; set; }
    }
}
