using MockItUp.Core.PlaceHolder;
using Xunit;

namespace MockItUp.Core.Tests
{
    public class PlaceHolderConverterTest
    {
        [Theory]
        [InlineData("${id}")]
        public void Convert_ShouldAsExpected(string placeHolder)
        {
            var converter = new PlaceHolderConverter();
            var converted = converter.Convert(placeHolder);
        }
    }
}
