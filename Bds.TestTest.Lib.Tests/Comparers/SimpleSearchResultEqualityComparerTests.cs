using Bds.TechTest.Lib.Comparers;
using Bds.TechTest.Lib.Results;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Bds.TestTest.Lib.Tests.Comparers
{
    [TestFixture]
    public class SimpleSearchResultEqualityComparerTests
    {
        private static readonly string TestPageTitle1 = "Test Page 1";
        private static readonly string TestPageTitle1ChangedCasing = "tEsT pAgE 1";
        private static readonly string TestPageTitle2 = "Test Page 2";
        private static readonly Uri TestUri1 = new Uri("http://www.google.com/");
        private static readonly Uri TestUri2 = new Uri("http://www.differs-by-google.com/");

        private static readonly ISimpleSearchResult TestSimpleSearchResult = Substitute.For<ISimpleSearchResult>();
        private static readonly ISimpleSearchResult ExactCopy = Substitute.For<ISimpleSearchResult>();
        private static readonly ISimpleSearchResult DiffersByPageTitle = Substitute.For<ISimpleSearchResult>();
        private static readonly ISimpleSearchResult DiffersByPageTitleCasing = Substitute.For<ISimpleSearchResult>();
        private static readonly ISimpleSearchResult DiffersByUri = Substitute.For<ISimpleSearchResult>();

        private static readonly List<SimpleSearchResultEqualityComparerTestCase> EqualsTestCases =
            new List<SimpleSearchResultEqualityComparerTestCase>();

        [OneTimeSetUp]
        public void InitializeObjects()
        {
            TestSimpleSearchResult.PageTitle.Returns(TestPageTitle1);
            TestSimpleSearchResult.Uri.Returns(TestUri1);

            ExactCopy.PageTitle.Returns(TestPageTitle1);
            ExactCopy.Uri.Returns(TestUri1);

            DiffersByPageTitle.PageTitle.Returns(TestPageTitle2);
            DiffersByPageTitle.Uri.Returns(TestUri1);

            DiffersByPageTitleCasing.PageTitle.Returns(TestPageTitle1ChangedCasing);
            DiffersByPageTitleCasing.Uri.Returns(TestUri1);

            DiffersByUri.PageTitle.Returns(TestPageTitle1);
            DiffersByUri.Uri.Returns(TestUri2);

            EqualsTestCases.AddRange(new[]
            {
                new SimpleSearchResultEqualityComparerTestCase(TestSimpleSearchResult, TestSimpleSearchResult, true),
                new SimpleSearchResultEqualityComparerTestCase(TestSimpleSearchResult, ExactCopy, true),
                new SimpleSearchResultEqualityComparerTestCase(TestSimpleSearchResult, DiffersByPageTitle, false),
                new SimpleSearchResultEqualityComparerTestCase(TestSimpleSearchResult, DiffersByPageTitleCasing, true),
                new SimpleSearchResultEqualityComparerTestCase(TestSimpleSearchResult, DiffersByUri, false),
                new SimpleSearchResultEqualityComparerTestCase(null, TestSimpleSearchResult, false),
                new SimpleSearchResultEqualityComparerTestCase(TestSimpleSearchResult, null, false),
            });
        }

        [Test]
        public void CheckEqualsReturnsExpectedValues()
        {
            // N.b. This is required because the contents of the collection haven't been initialized by the time a TestCaseSourceAttribute is evaluated.
            foreach (var testCase in EqualsTestCases)
            {
                Assert.That(
                    SimpleSearchResultEqualityComparer.Instance.Equals(testCase.First, testCase.Second),
                    Is.EqualTo(testCase.ExpectedEqual));
            }
        }

        // N.b. In .NET Core hash codes are only consistent in a single program execution, so we can't test for expected hash codes.

        /// <summary>
        /// Represents a single test case.
        /// </summary>
        public class SimpleSearchResultEqualityComparerTestCase
        {
            public SimpleSearchResultEqualityComparerTestCase(ISimpleSearchResult first, ISimpleSearchResult second, bool expectedEqual)
            {
                First = first;
                Second = second;
                ExpectedEqual = expectedEqual;
            }

            public ISimpleSearchResult First { get; }

            public ISimpleSearchResult Second { get; }

            public bool ExpectedEqual { get; }
        }

    }
}
