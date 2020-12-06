using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bds.TechTest.Lib.Http
{
    /// <summary>
    /// Response converter class which can produce <see cref="HtmlDocument"/> instances from an HTTP response.
    /// </summary>
    public class HtmlDocumentResponseContentConverter : IResponseContentConverter<HtmlDocument>
    {
        /// <inheritdoc />
        public async Task<HtmlDocument> ConvertFrom(HttpContent httpContent)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(await httpContent.ReadAsStreamAsync());
            return htmlDocument;
        }
    }
}
