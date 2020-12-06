using System.Net;

namespace Bds.TechTest.Lib.Http
{
    /// <summary>
    /// Contract for a class which packages certain useful data pertaining to an HTTP response into an easy-to-manipulate form.
    /// </summary>
    public interface IHttpResponseInfo
    {
        /// <summary>
        /// The status code associated with the response.
        /// </summary>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Information describing the request which gave rise to this response.
        /// </summary>
        IHttpRequestInfo RequestInfo { get; }

        /// <summary>
        /// Flag indicating whether the status code indicates a successful response.
        /// </summary>
        bool HasSuccessStatusCode { get; }
    }

    /// <summary>
    /// Generic version of the IHttpResponseInfo interface which allows a response object to be returned.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHttpResponseInfo<T> : IHttpResponseInfo
        where T : class
    {
        /// <summary>
        /// The deserialized response.
        /// </summary>
        T Response { get; set; }
    }
}
