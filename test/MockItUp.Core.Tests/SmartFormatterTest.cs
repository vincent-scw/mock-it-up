using MockItUp.Core.Utilities;
using Xunit;

namespace MockItUp.Core.Tests
{
    public class SmartFormatterTest
    {
        [Theory]
        [InlineData("{ id:${id} }", "{ id:123 }")]
        [InlineData("{ id:${id}, idStr:\"${id}\" }", "{ id:123, idStr:\"123\" }")]
        [InlineData("{ id:1 }", "{ id:1 }")]
        public void ReplacePlaceHolder_ShouldAsExpected(string template, string expected)
        {
            var formatted = template.Format((s) => "123");

            Assert.Equal(expected, formatted);
        }
    }
}
