using System;
using System.Net.Http;

namespace Bds.TechTest.Lib.Http
{
    /// <summary>
    /// Contract for a class which contains information about a request.
    /// </summary>
    public interface IHttpRequestInfo
    {
        /// <summary>
        /// The Uri associated with the request.
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        /// The HttpMethod associated with the request.
        /// </summary>
        HttpMethod HttpMethod { get; }
    }
}
