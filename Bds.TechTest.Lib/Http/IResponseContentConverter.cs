using System.Net.Http;
using System.Threading.Tasks;

namespace Bds.TechTest.Lib.Http
{
    /// <summary>
    /// Contract for a class which can convert the response from an HTTP request into a usable form.
    /// </summary>
    /// <typeparam name="T">The Type to which this converter will convert a response.</typeparam>
    public interface IResponseContentConverter<T> 
        where T : class
    {
        /// <summary>
        /// Converts the specified HTTP content instance to an instance of <see cref="T"/>.
        /// </summary>
        /// <param name="httpContent"></param>
        /// <returns></returns>
        Task<T> ConvertFrom(HttpContent httpContent);
    }
}
