using MockItUp.Core;
using Xunit;

namespace MockItUp.UnitTest.Core
{
    public class UrlNormalizerTest
    {
        [Fact]
        public void Test1ConvertingTheSchemeAndHostToLowercase()
        {
            var url1 = "HTTP://www.Example.com/".NormalizeUrl();
            var url2 = "http://www.example.com/".NormalizeUrl();

            Assert.Equal(url1, url2);
        }

        [Fact]
        public void Test2CapitalizingLettersInEscapeSequences()
        {
            var url1 = "http://www.example.com/a%c2%b1b".NormalizeUrl();
            var url2 = "http://www.example.com/a%C2%B1b".NormalizeUrl();

            Assert.Equal(url1, url2);
        }

        [Fact]
        public void Test3DecodingPercentEncodedOctetsOfUnreservedCharacters()
        {
            var url1 = "http://www.example.com/%7Eusername/".NormalizeUrl();
            var url2 = "http://www.example.com/~username/".NormalizeUrl();

            Assert.Equal(url1, url2);
        }

        [Fact]
        public void Test4RemovingTheDefaultPort()
        {
            var url1 = "http://www.example.com:80/bar.html".NormalizeUrl();
            var url2 = "http://www.example.com/bar.html".NormalizeUrl();

            Assert.Equal(url1, url2);
        }

        [Fact]
        public void Test5AddingTrailing()
        {
            var url1 = "http://www.example.com/alice".NormalizeUrl();
            var url2 = "http://www.example.com/alice/?".NormalizeUrl();

            Assert.Equal(url1, url2);
        }

        [Fact]
        public void Test6RemovingDotSegments()
        {
            var url1 = "http://www.example.com/../a/b/../c/./d.html".NormalizeUrl();
            var url2 = "http://www.example.com/a/c/d.html".NormalizeUrl();

            Assert.Equal(url1, url2);
        }

        [Fact]
        public void Test7RemovingDirectoryIndex1()
        {
            var url1 = "http://www.example.com/default.asp".NormalizeUrl();
            var url2 = "http://www.example.com/".NormalizeUrl();

            Assert.Equal(url1, url2);
        }

        [Fact]
        public void Test7RemovingDirectoryIndex2()
        {
            var url1 = "http://www.example.com/default.asp?id=1".NormalizeUrl();
            var url2 = "http://www.example.com/default.asp?id=1".NormalizeUrl();

            Assert.Equal(url1, url2);
        }

        [Fact]
        public void Test7RemovingDirectoryIndex3()
        {
            var url1 = "http://www.example.com/a/index.html".NormalizeUrl();
            var url2 = "http://www.example.com/a/".NormalizeUrl();

            Assert.Equal(url1, url2);
        }

        [Fact]
        public void Test8RemovingTheFragment()
        {
            var url1 = "http://www.example.com/bar.html#section1".NormalizeUrl();
            var url2 = "http://www.example.com/bar.html".NormalizeUrl();

            Assert.Equal(url1, url2);
        }

        [Fact]
        public void Test9LimitingProtocols()
        {
            var url1 = "https://www.example.com/".NormalizeUrl();
            var url2 = "http://www.example.com/".NormalizeUrl();

            Assert.Equal(url1, url2);
        }

        [Fact]
        public void Test10RemovingDuplicateSlashes()
        {
            var url1 = "http://www.example.com/foo//bar.html".NormalizeUrl();
            var url2 = "http://www.example.com/foo/bar.html".NormalizeUrl();

            Assert.Equal(url1, url2);
        }

        [Fact]
        public void Test11AddWww()
        {
            var url1 = "http://example.com/".NormalizeUrl();
            var url2 = "http://www.example.com".NormalizeUrl();

            Assert.Equal(url1, url2);
        }

        [Fact]
        public void Test12RemoveFeedburnerPart()
        {
            var url1 =
                "http://site.net/2013/02/firefox-19-released/?utm_source=rss&utm_medium=rss&utm_campaign=firefox-19-released"
                    .NormalizeUrl();
            var url2 = "http://site.net/2013/02/firefox-19-released".NormalizeUrl();

            Assert.Equal(url1, url2);
        }
    }
}