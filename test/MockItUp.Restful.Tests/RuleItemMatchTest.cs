using MockItUp.Restful.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MockItUp.Restful.Tests
{
    public class RuleItemMatchTest
    {
        [Fact]
        public void MatchCases_ShouldReturnTrue()
        {
            var item = new RuleItem
            {
                Request = new RequestModel
                {
                    Method = "GET",
                    Path = "/api/values"
                }
            };

        }
    }
}
