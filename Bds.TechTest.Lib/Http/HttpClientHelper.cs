using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Bds.TechTest.Lib.Http
{
    /// <summary>
    /// Helper class which wraps an HttpClient instance and performs useful functions.
    /// </summary>
    public class HttpClientHelper
    {
        /// <summary>
        /// Single instance shared across the application.
        /// </summary>
        private static readonly HttpClient httpClient = new HttpClient();

        private readonly ILogger<HttpClientHelper> logger;

        /// <summary>
        /// Creates a new HttpClientHelper instance.
        /// </summary>
        /// <param name="logger">Logger instance for the class.</param>
        public HttpClientHelper(ILogger<HttpClientHelper> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Performs a GET request to the specified URL.
        /// </summary>
        /// <param name="uri">The Uri to which the request should be sent.</param>
        /// <param name="cancellationToken">A CancellationToken to associate with the request.</param>
        /// <returns>An instance of TResponse or throws.</returns>
        public async Task<IHttpResponseInfo> Get(Uri uri, CancellationToken cancellationToken)
        {
            return await SendRequest(uri, HttpMethod.Get, cancellationToken);
        }

        /// <summary>
        /// Performs a GET request expecting response content to the specified URL.
        /// </summary>
        /// <typeparam name="TResponse">The type into which the response should be deserialized.</typeparam>
        /// <param name="uri">The Uri to which the request should be sent.</param>
        /// <param name="responseContentConverter">A response converter which can convert the raw response into a useful class.</param>
        /// <param name="cancellationToken">A CancellationToken to associate with the request.</param>
        /// <returns>An instance of TResponse or throws.</returns>
        public async Task<IHttpResponseInfo<TResponse>> Get<TResponse>(Uri uri, IResponseContentConverter<TResponse> responseContentConverter, 
            CancellationToken cancellationToken)
            where TResponse : class
        {
            return await SendRequestExpectingResponseContent<TResponse>(uri, HttpMethod.Get, responseContentConverter, cancellationToken);
        }

        /// <summary>
        /// Performs a request to the specified URL using the specified method.
        /// </summary>
        /// <param name="uri">The Uri to which the request should be sent.</param>
        /// <param name="httpMethod">The HTTP method to send with the request.</param>
        /// <param name="cancellationToken">A CancellationToken to associate with the request.</param>
        /// <returns>An HttpResponseInfo instance.</returns>
        private async Task<IHttpResponseInfo> SendRequest(Uri uri, HttpMethod httpMethod, CancellationToken cancellationToken)
        {
            TraceRequestIfRequired(uri, httpMethod);

            using (var request = new HttpRequestMessage(httpMethod, uri))
            {
                using (var response = await httpClient.SendAsync(request, cancellationToken))
                {
                    return new HttpResponseInfo(response.StatusCode, new HttpRequestInfo(uri, httpMethod));
                }
            }
        }

        /// <summary>
        /// Performs a request to the specified URL using the specified method, expecting response content.
        /// </summary>
        /// <typeparam name="TResponse">The type into which the response should be deserialized.</typeparam>
        /// <param name="uri">The Uri to which the request should be sent.</param>
        /// <param name="httpMethod">The HTTP method to send with the request.</param>
        /// <param name="cancellationToken">A CancellationToken to associate with the request.</param>
        /// <returns>An HttpResponseInfo instance of type TResponse.</returns>
        private async Task<IHttpResponseInfo<TResponse>> SendRequestExpectingResponseContent<TResponse>(Uri uri, HttpMethod httpMethod, 
            IResponseContentConverter<TResponse> responseContentConverter, CancellationToken cancellationToken)
            where TResponse : class
        {
            TraceRequestIfRequired(uri, httpMethod);

            using (var request = new HttpRequestMessage(httpMethod, uri))
            {
                using (var response = await httpClient.SendAsync(request, cancellationToken))
                {
                    return await ParseHttpContentAndConstructResponse(
                        new HttpRequestInfo(uri, httpMethod), response.StatusCode, responseContentConverter, response.Content);
                }
            }
        }

        /// <summary>
        /// Attempts to parse the supplied HttpContent instance and constructs an IHttpResponseInfo response object.
        /// If parsing fails on a successful response, an exception is thrown.
        /// </summary>
        /// <typeparam name="TResponse">The expected Type of the converted HttpContent.</typeparam>
        /// <param name="httpRequestInfo">The IHttpRequestInfo instance describing the request.</param>
        /// <param name="statusCode">The status code associated with the HTTP response.</param>
        /// <param name="content">The HttpContent object which should be parsed.</param>
        /// <returns>A Task returning the expected response object Type.</returns>
        private async Task<IHttpResponseInfo<TResponse>> ParseHttpContentAndConstructResponse<TResponse>(IHttpRequestInfo httpRequestInfo, HttpStatusCode statusCode,
            IResponseContentConverter<TResponse> responseContentConverter, HttpContent content)
            where TResponse : class
        {
            var is2xxCode = (int)statusCode >= 200 && (int)statusCode <= 299;

            // If this isn't a success code, we don't try to handle the response content and so return an empty error response object.
            if (!is2xxCode)
            {
                return new HttpResponseInfo<TResponse>(statusCode, httpRequestInfo);
            }

            // Otherwise, we try to parse the content. Failures to parse in 2xx cases here are considered fatal so we let any exceptions bubble up.
            var responseContent = await responseContentConverter.ConvertFrom(content);
            return new HttpResponseInfo<TResponse>(statusCode, httpRequestInfo, responseContent);
        }

        /// <summary>
        /// Traces the details of a request, if trace logging is enabled.
        /// </summary>
        /// <param name="uri">The Uri to which the request will be sent.</param>
        /// <param name="httpMethod">The HTTP method with which the request will be sent.</param>
        /// <param name="contentType">Optionally, the Type of the content of the request.</param>
        private void TraceRequestIfRequired(Uri uri, HttpMethod httpMethod, Type contentType = null)
        {
            if (!logger.IsEnabled(LogLevel.Trace))
            {
                return;
            }

            var contentDescriptor = contentType != null
                ? $"having content of type {contentType.Name}"
                : "having no content";
            logger.LogTrace("Attempting HTTP {method} request {contentDescriptor} to the endpoint \"{uri}\".",
                httpMethod, contentDescriptor, uri);
        }
    }
}
