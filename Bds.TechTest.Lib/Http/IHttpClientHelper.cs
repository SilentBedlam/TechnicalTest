using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bds.TechTest.Lib.Http
{
    /// <summary>
    /// Contract for a helper class which wraps an HttpClient instance and performs useful functions.
    /// </summary>
    public interface IHttpClientHelper
    {
        /// <summary>
        /// Performs a GET request to the specified URL.
        /// </summary>
        /// <param name="uri">The Uri to which the request should be sent.</param>
        /// <param name="cancellationToken">A CancellationToken to associate with the request.</param>
        /// <returns>An instance of TResponse or throws.</returns>
        Task<IHttpResponseInfo> Get(Uri uri, CancellationToken cancellationToken);

        /// <summary>
        /// Performs a GET request expecting response content to the specified URL.
        /// </summary>
        /// <typeparam name="TResponse">The type into which the response should be deserialized.</typeparam>
        /// <param name="uri">The Uri to which the request should be sent.</param>
        /// <param name="responseContentConverter">A response converter which can convert the raw response into a useful class.</param>
        /// <param name="cancellationToken">A CancellationToken to associate with the request.</param>
        /// <returns>An instance of TResponse or throws.</returns>
        Task<IHttpResponseInfo<TResponse>> Get<TResponse>(Uri uri, IResponseContentConverter<TResponse> responseContentConverter, CancellationToken cancellationToken) where TResponse : class;

        /// <summary>
        /// Sets the default request headers to be used 
        /// </summary>
        /// <param name="defaultHeaders">The default headers to be used by requests made to this class. If called without an argument specified, 
        /// the header collection is cleared.</param>
        void SetDefaultRequestHeaders(IDictionary<string, string> defaultHeaders = null);
    }
}