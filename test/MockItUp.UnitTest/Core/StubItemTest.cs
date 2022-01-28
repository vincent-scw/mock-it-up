using MockItUp.Core.Models;
using System;
using Xunit;

namespace MockItUp.UnitTest.Core
{
    public class StubItemTest
    {
        [Theory]
        [InlineData("/api/orders", "https://test.com/api/orders")] // Should ignore slash
        [InlineData("/api/orders", "https://test.com//api//orders")] // Duplicated slash doesn't matter
        [InlineData("api/orders/{orderId}", "https://test.com/api/orders/123")]  // Should match template
        [InlineData("api/orders?status={status}", "https://test.com/api/orders?status=created")] // Should match template in query string
        [InlineData("api/orders?status={status}&createdBy=me", "https://test.com/api/orders?createdBy=me&status=created")] // Query sequence doesn't matter
        [InlineData("/api/orders", "https://test.com/api/orders/?")] // Redundant tail doesn't matter
        [InlineData("api/values", "HTTP://test.com/API/VALUES")] // Case insensitive

        // Support RFC 6570 URI Templates (Level 4). Using https://github.com/pierewoj/dotnet-uritemplate
        [InlineData("api/values{?v}", "http://test.com/api/values?v=1")]
        [InlineData("api/values{?v,c}", "http://test.com/api/values?c=acc&v=1")]
        [InlineData("api/values/{id}", "http://test.com/api/values/1")]
        [InlineData("/api/values/{id}", "http://test.com/api/values/1")]
        [InlineData("api/values/{id}/{sid}", "http://test.com/api/values/1/go")]
        
        // Support Unicode
        [InlineData("api/中文/测试", "http://test.com/api/中文/测试")]
        public void Url_ShouldMatch(string urlTemplate, string urlCandidate)
        {
            var stub = new StubItem() { Request = new RequestModel { Method = "get", Path = urlTemplate } };
            var matched = stub.Match("get", new Uri(urlCandidate));
            Assert.NotNull(matched);
        }
    }
}
