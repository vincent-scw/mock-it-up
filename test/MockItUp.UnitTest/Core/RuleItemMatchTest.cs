using MockItUp.Core.Models;
using System;
using Xunit;

namespace MockItUp.UnitTest.Core
{
    public class RuleItemMatchTest
    {
        [Theory]
        [InlineData("api/values", "http://localhost:5000/api/values", true)]
        [InlineData("api/values{?v}", "http://localhost:5000/api/values?v=1", true)]
        [InlineData("api/values{?v,c}", "http://localhost:5000/api/values?c=acc&v=1", true)]
        [InlineData("api/values/{id}", "http://localhost:5000/api/values/1", true)]
        [InlineData("/api/values/{id}", "http://localhost:5000/api/values/1", true)]
        [InlineData("api/values/{id}/{sid}", "http://localhost:5000/api/values/1/go", true)]
        [InlineData("api/values", "http://localhost:5000/api/orders", false)]
        [InlineData("api/values{?v}", "http://localhost:5000/api/values?c=acc", false)]
        [InlineData("api/values", "HTTP://LOCALHOST:5000/API/VALUES", false)]
        public void PathMatch_ShouldSucceed(string pattern, string candidate, bool match)
        {
            var item = new StubItem
            {
                Request = new RequestModel
                {
                    Method = "GET",
                    Path = pattern
                }
            };

            var result = item.Match("get", new Uri(candidate));
            if (match)
                Assert.NotNull(result);
            else
                Assert.Null(result);
        }
    }
}
