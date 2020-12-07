using Bds.TechTest.Lib.Http;
using Bds.TechTest.Lib.Orchestration;
using Bds.TechTest.Lib.Results;
using Bds.TechTest.Lib.SearchEngines;
using NSubstitute;
using NUnit.Framework;
using System;

namespace Bds.TestTest.Lib.Tests.Orchestration
{
    [TestFixture]
    public class SearchEngineOrchestratorTests
    {
        private ISearchEngineDefinition<ISimpleSearchResult> mockSearchEngineDefinition;
        private IHttpClientHelper mockHttpClientHelper;
        private IHttpClientHelperProvider mockHttpClientHelperProvider;

        [SetUp]
        public void Initialize()
        {
            mockSearchEngineDefinition = Substitute.For<ISearchEngineDefinition<ISimpleSearchResult>>();

            mockHttpClientHelper = Substitute.For<IHttpClientHelper>();
            mockHttpClientHelperProvider = Substitute.For<IHttpClientHelperProvider>();
            mockHttpClientHelperProvider.GetHttpClientHelper().Returns(mockHttpClientHelper);
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

        // TBC - Rearrange class and allow injection of the HttpClientHelper instance. Test methods.
    }
}
