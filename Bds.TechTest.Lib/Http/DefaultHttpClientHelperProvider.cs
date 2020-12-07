using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Bds.TechTest.Lib.Http
{
    /// <summary>
    /// Default implementation of an IHttpClientHelperProvider.
    /// </summary>
    public class DefaultHttpClientHelperProvider : IHttpClientHelperProvider
    {
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// Creates a new DefaultHttpClientHelperProvider instance.
        /// </summary>
        /// <param name="loggerFactory">Optionally an ILoggerFactory instance which will be used to provide loggers to the HttpClientHelper instances.</param>
        public DefaultHttpClientHelperProvider(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
        }

        /// <inheritdoc />
        public IHttpClientHelper GetHttpClientHelper()
        {
            var logger = loggerFactory.CreateLogger<HttpClientHelper>();
            return new HttpClientHelper(logger);
        }
    }
}
