using Bds.TechTest.Lib.Http;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace Bds.TestTest.Lib.Tests.Http
{
    [TestFixture]
    public class HttpRequestInfoTests
    {
        [Test]
        public void CheckConstructionWithoutUriIsProhibited()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpRequestInfo(null, HttpMethod.Get));
        }     
    }
}
