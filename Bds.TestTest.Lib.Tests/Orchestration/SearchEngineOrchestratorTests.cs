using Bds.TechTest.Lib.Http;
using Bds.TechTest.Lib.Orchestration;
using Bds.TechTest.Lib.Results;
using Bds.TechTest.Lib.Scrapers;
using Bds.TechTest.Lib.SearchEngines;
using HtmlAgilityPack;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bds.TestTest.Lib.Tests.Orchestration
{
    [TestFixture]
    public class SearchEngineOrchestratorTests
    {
        private HtmlDocument mockHtmlDocument = new HtmlDocument();
        private ISearchEngineDefinition<ISimpleSearchResult> mockSearchEngineDefinition;
        private IHttpClientHelper mockHttpClientHelper;
        private IHttpClientHelperProvider mockHttpClientHelperProvider;

        private SearchEngineOrchestrator<ISimpleSearchResult> testInstance;

        [SetUp]
        public void Initialize()
        {
            mockHtmlDocument.LoadHtml("<html><body><p>Text</p></body></html>");

            var mockResults = new List<SimpleSearchResult>
            {
                new SimpleSearchResult("Test Page 1", new Uri("http://www.google.com"), 1)
            };

            var mockResultScraper = Substitute.For<ISearchResultScraper<ISimpleSearchResult>>();
            mockResultScraper.ScrapeResults(Arg.Any<HtmlDocument>(), Arg.Any<int>()).Returns(mockResults);

            mockSearchEngineDefinition = Substitute.For<ISearchEngineDefinition<ISimpleSearchResult>>();
            mockSearchEngineDefinition.UriFormat.Returns("http://www.google.com/{0}");
            mockSearchEngineDefinition.ResultScraper.Returns(mockResultScraper);

            mockHttpClientHelper = Substitute.For<IHttpClientHelper>();
            
            mockHttpClientHelperProvider = Substitute.For<IHttpClientHelperProvider>();
            mockHttpClientHelperProvider.GetHttpClientHelper().Returns(mockHttpClientHelper);

            testInstance = new SearchEngineOrchestrator<ISimpleSearchResult>(mockSearchEngineDefinition, mockHttpClientHelperProvider);
        }

        [Test]
        public void CheckConstructionWithMissingSearchEngineDefinitionIsRejected()
        {
            Assert.Throws<ArgumentNullException>(() => 
                new SearchEngineOrchestrator<ISimpleSearchResult>(null, mockHttpClientHelperProvider));
        }

        [Test]
        public void CheckConstructionWithMissingHttpClientHelperProviderIsRejected()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SearchEngineOrchestrator<ISimpleSearchResult>(mockSearchEngineDefinition, null));
        }

        [Test]
        public async Task CheckExecuteConstructsUriCorrectly()
        {
            // Cause the HttpClientHelper mock to return successfully.
            var mockResponseInfo = Substitute.For<IHttpResponseInfo<HtmlDocument>>();
            mockResponseInfo.StatusCode.Returns(System.Net.HttpStatusCode.OK);
            mockResponseInfo.HasSuccessStatusCode.Returns(true);
            mockResponseInfo.Response.Returns(mockHtmlDocument);

            mockHttpClientHelper
                .Get<HtmlDocument>(Arg.Any<Uri>(), Arg.Any<IResponseContentConverter<HtmlDocument>>(), Arg.Any<CancellationToken>())
                .Returns(mockResponseInfo);

            var results = await testInstance.Execute("Fish");
            var actualUri = mockHttpClientHelper.ReceivedCalls()
                .Where(c => c.GetMethodInfo().Name == "Get")
                .First()
                .GetArguments()[0] as Uri;
            Assert.That(actualUri, Is.EqualTo(new Uri("http://www.google.com/Fish")));
        }

        // TBC - can we detect whether the headers are set appropriately?

        [Test]
        public void CheckExecuteThrowsOnNonSuccessStatusCode()
        {
            // Cause the HttpClientHelper mock to return an error.
            var mockResponseInfo = Substitute.For<IHttpResponseInfo<HtmlDocument>>();
            mockResponseInfo.StatusCode.Returns(System.Net.HttpStatusCode.NotFound);
            mockResponseInfo.HasSuccessStatusCode.Returns(false);

            mockHttpClientHelper
                .Get<HtmlDocument>(Arg.Any<Uri>(), Arg.Any<IResponseContentConverter<HtmlDocument>>(), Arg.Any<CancellationToken>())
                .Returns(mockResponseInfo);

            // TODO - This should be a more specific exception type.
            Assert.ThrowsAsync<Exception>(async () => await testInstance.Execute("Fish"));
        }

        [Test]
        public async Task CheckExecuteCallsResultsScraperOnSuccessStatusCode()
        {
            // Cause the HttpClientHelper mock to return successfully.
            var mockResponseInfo = Substitute.For<IHttpResponseInfo<HtmlDocument>>();
            mockResponseInfo.StatusCode.Returns(System.Net.HttpStatusCode.OK);
            mockResponseInfo.HasSuccessStatusCode.Returns(true);
            mockResponseInfo.Response.Returns(mockHtmlDocument);

            mockHttpClientHelper
                .Get<HtmlDocument>(Arg.Any<Uri>(), Arg.Any<IResponseContentConverter<HtmlDocument>>(), Arg.Any<CancellationToken>())
                .Returns(mockResponseInfo);

            var results = await testInstance.Execute("Fish");
            mockSearchEngineDefinition.ResultScraper.Received(1).ScrapeResults(Arg.Any<HtmlDocument>(), Arg.Any<int>());
        }

        [Test]
        public async Task CheckExecuteReturnsExpectedSuccessResult()
        {
            // Cause the HttpClientHelper mock to return successfully.
            var mockResponseInfo = Substitute.For<IHttpResponseInfo<HtmlDocument>>();
            mockResponseInfo.StatusCode.Returns(System.Net.HttpStatusCode.OK);
            mockResponseInfo.HasSuccessStatusCode.Returns(true);
            mockResponseInfo.Response.Returns(mockHtmlDocument);

            mockHttpClientHelper
                .Get<HtmlDocument>(Arg.Any<Uri>(), Arg.Any<IResponseContentConverter<HtmlDocument>>(), Arg.Any<CancellationToken>())
                .Returns(mockResponseInfo);

            var results = await testInstance.Execute("Fish");

            // The scraper will have replaced any real result with the mock result defined above. Check for it.
            Assert.That(results.SearchEngineDefinition, Is.SameAs(mockSearchEngineDefinition));
            Assert.That(results.SearchResults.Count(), Is.EqualTo(1));

            var result1 = results.SearchResults.Single();
            Assert.That(result1.PageTitle, Is.EqualTo("Test Page 1"));
            Assert.That(result1.Uri, Is.EqualTo(new Uri("http://www.google.com")));
            Assert.That(result1.Rank, Is.EqualTo(1));
        }
    }
}
