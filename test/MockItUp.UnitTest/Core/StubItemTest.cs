using MockItUp.Core.Models;
using System;
using Xunit;

namespace MockItUp.UnitTest.Core
{
    public class StubItemTest
    {
        [Theory]
        [InlineData("/api/orders", "https://test.com/api/orders")]
        //[InlineData("/api/orders", "https://test.com/api/orders/")]
        public void Url_ShouldMatch(string urlTemplate, string urlCandidate)
        {
            var stub = new StubItem() { Request = new RequestModel { Method = "get", Path = urlTemplate } };
            var matched = stub.Match("get", new Uri(urlCandidate));
            Assert.NotNull(matched);
        }
    }
}
