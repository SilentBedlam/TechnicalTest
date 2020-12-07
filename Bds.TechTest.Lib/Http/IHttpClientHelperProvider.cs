namespace Bds.TechTest.Lib.Http
{
    /// <summary>
    /// Contract for a class which can provide IHttpClientHelper instances.
    /// </summary>
    public interface IHttpClientHelperProvider
    {
        /// <summary>
        /// Returns an <see cref="IHttpClientHelper"/> instance.
        /// </summary>
        /// <returns>An <see cref="IHttpClientHelper"/> instance.</returns>
        IHttpClientHelper GetHttpClientHelper();
    }
}
