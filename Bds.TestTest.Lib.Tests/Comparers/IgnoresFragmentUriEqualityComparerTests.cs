using Bds.TechTest.Lib.Comparers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Bds.TestTest.Lib.Tests.Comparers
{
    [TestFixture]
    public class IgnoresFragmentUriEqualityComparerTests
    {
        private static readonly Uri TestUri = new Uri("http://www.google.com:80/some/path?something=somethingElse#fragment1");
        private static readonly Uri ExactCopy = new Uri("http://www.google.com:80/some/path?something=somethingElse#fragment1");
        private static readonly Uri DiffersByScheme = new Uri("bob://www.google.com:80/some/path?something=somethingElse#fragment1");
        private static readonly Uri DiffersByHost = new Uri("http://www.goggle.com:80/some/path?something=somethingElse#fragment1");
        private static readonly Uri DiffersByPort = new Uri("http://www.google.com:42/some/path?something=somethingElse#fragment1");
        private static readonly Uri DiffersByPath = new Uri("http://www.google.com:80/some/other/path?something=somethingElse#fragment1");
        private static readonly Uri DiffersByQuery = new Uri("http://www.google.com:80/some/path?something=notSomethingElse#fragment1");
        private static readonly Uri DiffersByFragment = new Uri("http://www.google.com:80/some/path?something=somethingElse#fragment2");

        private static readonly List<IgnoresFragmentUriEqualityComparerTestCase> EqualsTestCases =
            new List<IgnoresFragmentUriEqualityComparerTestCase>();

        [OneTimeSetUp]
        public void InitializeObjects()
        {
            EqualsTestCases.AddRange(new[]
            {
                new IgnoresFragmentUriEqualityComparerTestCase(TestUri, TestUri, true),
                new IgnoresFragmentUriEqualityComparerTestCase(TestUri, ExactCopy, true),
                new IgnoresFragmentUriEqualityComparerTestCase(TestUri, DiffersByScheme, false),
                new IgnoresFragmentUriEqualityComparerTestCase(TestUri, DiffersByHost, false),
                new IgnoresFragmentUriEqualityComparerTestCase(TestUri, DiffersByPort, false),
                new IgnoresFragmentUriEqualityComparerTestCase(TestUri, DiffersByPath, false),
                new IgnoresFragmentUriEqualityComparerTestCase(TestUri, DiffersByQuery, false),
                new IgnoresFragmentUriEqualityComparerTestCase(TestUri, DiffersByFragment, true),
                new IgnoresFragmentUriEqualityComparerTestCase(null, TestUri, false),
                new IgnoresFragmentUriEqualityComparerTestCase(TestUri, null, false),
            });
        }

        [Test]
        public void CheckEqualsReturnsExpectedValues()
        {
            // N.b. This is required because the contents of the collection haven't been initialized by the time a TestCaseSourceAttribute is evaluated.
            foreach (var testCase in EqualsTestCases)
            {
                Assert.That(
                    IgnoresFragmentUriEqualityComparer.Instance.Equals(testCase.First, testCase.Second),
                    Is.EqualTo(testCase.ExpectedEqual));
            }
        }

        // N.b. In .NET Core hash codes are only consistent in a single program execution, so we can't test for expected hash codes.

        /// <summary>
        /// Represents a single test case.
        /// </summary>
        public class IgnoresFragmentUriEqualityComparerTestCase
        {
            public IgnoresFragmentUriEqualityComparerTestCase(Uri first, Uri second, bool expectedEqual)
            {
                First = first;
                Second = second;
                ExpectedEqual = expectedEqual;
            }

            public Uri First { get; }

            public Uri Second { get; }

            public bool ExpectedEqual { get; }
        }
    }
}
