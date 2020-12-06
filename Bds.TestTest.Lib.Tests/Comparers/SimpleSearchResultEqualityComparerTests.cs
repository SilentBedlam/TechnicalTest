using Bds.TechTest.Lib;
using Bds.TechTest.Lib.Comparers;
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

        private static readonly List<(ISimpleSearchResult simpleSearchResult, int expectedHash)> GetHashCodeTestCases =
            new List<(ISimpleSearchResult simpleSearchResultItem, int expectedHash)>();


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

            GetHashCodeTestCases.AddRange(new (ISimpleSearchResult simpleSearchResultItem, int expected)[]
            {
                (null, 0),
                (TestSimpleSearchResult, 297914221),
                //(ExactCopy, 1000204158),
                //(DiffersByPageTitle, -870807701),
                //(DiffersByPageTitleCasing, -870807701),
                //(DiffersByUri, -1032615386)
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

        [Test]
        public void CheckGetHashCodeReturnsExpectedValues()
        {
            // N.b. This is required because the contents of the collection haven't been initialized by the time a TestCaseSourceAttribute is evaluated.
            foreach (var testCase in GetHashCodeTestCases)
            {
                Assert.That(
                    SimpleSearchResultEqualityComparer.Instance.GetHashCode(testCase.simpleSearchResult),
                    Is.EqualTo(testCase.expectedHash));
            }
        }

        [Test]
        public void CheckUriHashCodes()
        {
            var first = TestUri1.GetHashCode();

            for (int i = 0; i < 10; i++)
            {
                Assert.That(TestUri1.GetHashCode(), Is.EqualTo(first));
            }
        }

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
