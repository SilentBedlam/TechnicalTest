using Bds.TechTest.Lib.Http;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bds.TestTest.Lib.Tests.Http
{
    public class HtmlDocumentResponseContentConverterTests
    {
        private const string TestGuidLocationXPathExpression = "//p[@id = 'paragraph-8']";

        private Guid testGuid;
        private HttpContent testHttpContent;

        [SetUp]
        public void Initialize()
        {
            // Set up an HttpContent instance to contain an HTML page, containing the test Guid at a particular location.
            // We'll use this as a proxy for reading the document correctly. 
            // We'll also count the numbers of elements to make sure that we read the whole document.
            const string headerHtmlString = "<head><title>Test Page 1</title><meta name=\"description\" content=\"Test Page 1\" /></head>";

            var sb = new StringBuilder("<html>");
            sb.Append(headerHtmlString);
            sb.Append("<body>");

            for (var i = 0; i < 10; i++)
            {
                sb.Append($"<p id=\"paragraph-{i}\" class=\"test-class-{i}\">");

                // On the 8th paragraph, include the GUID.
                sb.Append(i == 8 ? testGuid.ToString() : "[No Guid]");

                sb.Append("</p>");
            }

            sb.Append("</body></html>");

            // Assign the HttpContent instance using the contents of the string builder.
            testHttpContent = new StringContent(sb.ToString());
        }

        [Test]
        public async Task CheckHtmlDocumentParsedCorrectly()
        {
            var instance = new HtmlDocumentResponseContentConverter();

            var htmlDocument = await instance.ConvertFrom(testHttpContent);

            // Check that we find the test GUID in the expected place.
            var guidNode = htmlDocument.DocumentNode.SelectNodes(TestGuidLocationXPathExpression).Single();
            Assert.That(Guid.Parse(guidNode.InnerText), Is.EqualTo(testGuid));

            // Check that we get the right number of each element in the document.
            int GetNodeCount(string nodeType)
            {
                var nodes = htmlDocument.DocumentNode.SelectNodes($"//{nodeType}");
                return nodes.Count;
            }

            Assert.That(GetNodeCount("html"), Is.EqualTo(1));
            Assert.That(GetNodeCount("head"), Is.EqualTo(1));
            Assert.That(GetNodeCount("title"), Is.EqualTo(1));
            Assert.That(GetNodeCount("meta"), Is.EqualTo(1));
            Assert.That(GetNodeCount("body"), Is.EqualTo(1));
            Assert.That(GetNodeCount("p"), Is.EqualTo(10));
        }
    }
}
