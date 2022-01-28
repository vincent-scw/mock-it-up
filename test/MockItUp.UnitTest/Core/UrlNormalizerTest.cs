using MockItUp.Core;
using Xunit;

namespace MockItUp.UnitTest.Core
{
    public class UrlNormalizerTest
    {
        [Theory]
        [InlineData("HTTP://www.Example.com/", "http://www.example.com/")]
        [InlineData("http://www.example.com/a%c2%b1b", "http://www.example.com/a%C2%B1b")]
        [InlineData("http://www.example.com/%7Eusername/", "http://www.example.com/~username/")]
        [InlineData("http://www.example.com:80/bar.html", "http://www.example.com/bar.html")]
        [InlineData("http://www.example.com/alice", "http://www.example.com/alice/?")]
        [InlineData("http://www.example.com/../a/b/../c/./d.html", "http://www.example.com/a/c/d.html")]
        [InlineData("http://www.example.com/default.asp", "http://www.example.com/")]
        [InlineData("http://www.example.com/default.asp?id=1", "http://www.example.com/default.asp?id=1")]
        [InlineData("http://www.example.com/a/index.html", "http://www.example.com/a/")]
        [InlineData("http://www.example.com/bar.html#section1", "http://www.example.com/bar.html")]
        [InlineData("https://www.example.com/", "http://www.example.com/")]
        [InlineData("http://www.example.com/foo//bar.html", "http://www.example.com/foo/bar.html")]
        [InlineData("http://example.com/", "http://www.example.com")]
        [InlineData("http://site.net/2013/02/firefox-19-released/?utm_source=rss&utm_medium=rss&utm_campaign=firefox-19-released",
            "http://site.net/2013/02/firefox-19-released")]
        [InlineData("http://www.example.com?p1=a&p2=b", "http://www.example.com?p2=b&p1=a")]
        public void Normalized_ShouldBeSame(string url1, string url2)
        {
            Assert.Equal(url1.NormalizeUrl(), url2.NormalizeUrl());
        }
    }
}