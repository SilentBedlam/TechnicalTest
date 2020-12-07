using Bds.TechTest.Lib.Orchestration;
using Bds.TechTest.Lib.Results;
using Bds.TechTest.Lib.SearchEngines;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;

namespace Bds.TestTest.Lib.Tests.Orchestration
{
    [TestFixture]
    public class SearchResultsSetTests
    {
        private static readonly ISearchEngineDefinition<ISimpleSearchResult> MockSearchEngineDefinition =
            Substitute.For<ISearchEngineDefinition<ISimpleSearchResult>>();

        [Test]
        public void CheckConstructionWithMissingSearchEngineDefinitionIsRejected()
        {
            Assert.Throws<ArgumentNullException>(() => 
                new SearchResultsSet<ISimpleSearchResult>(null, Enumerable.Empty<ISimpleSearchResult>()));
        }

        [Test]
        public void CheckConstructionWithMissingSearchResultsIsRejected()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SearchResultsSet<ISimpleSearchResult>(MockSearchEngineDefinition, null));
        }

        // TBC - Rearrange class and allow injection of the HttpClientHelper instance. Test methods.
    }
}
