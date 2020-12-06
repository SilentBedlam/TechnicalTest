using Bds.TechTest.Lib.Http;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Net;

namespace Bds.TestTest.Lib.Tests.Http
{
    [TestFixture]
    public class HttpResponseInfoTests
    {
        private static readonly IHttpRequestInfo MockRequestInfo = Substitute.For<IHttpRequestInfo>();

        [Test]
        public void CheckConstructionWithoutRquestInfoIsProhibited()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpResponseInfo(HttpStatusCode.NotAcceptable, null));
        }

        [Test]
        [TestCase(HttpStatusCode.OK, ExpectedResult = true)]
        [TestCase(HttpStatusCode.BadRequest, ExpectedResult = false)]
        [TestCase(HttpStatusCode.InternalServerError, ExpectedResult = false)]
        [TestCase(HttpStatusCode.Conflict, ExpectedResult = false)]
        [TestCase(HttpStatusCode.Accepted, ExpectedResult = true)]
        [TestCase(257, ExpectedResult = true)] // Doesn't exist.
        [TestCase(429, ExpectedResult = false)] // "I'm a tea pot".
        public bool CheckHasSuccessStatusCodeCalculatedCorrectly(HttpStatusCode statusCode)
        {
            var instance = new HttpResponseInfo(statusCode, MockRequestInfo);
            return instance.HasSuccessStatusCode;
        }        
    }
}
