using System;
using System.Linq;
using System.Web;

namespace MockItUp.Core
{
    public static class UrlNormalizer
    {
        public static string[] DefaultDirectoryIndexes = new[]
            {
                "default.asp",
                "default.aspx",
                "index.htm",
                "index.html",
                "index.php"
            };

        public static string NormalizeUrl(this Uri uri)
        {
            var url = UrlToLower(uri);
            url = LimitProtocols(url);
            url = RemoveDefaultDirectoryIndexes(url);
            url = RemoveTheFragment(url);
            url = RemoveDuplicateSlashes(url);
            url = AddWww(url);
            url = RemoveFeedburnerPart(url);
            url = RemoveTrailingSlashAndEmptyQuery(url);
            url = SortQueryString(url);

            return url;
        }

        public static string NormalizeUrl(this string url)
        {
            return NormalizeUrl(new Uri(url));
        }

        private static string RemoveFeedburnerPart(string url)
        {
            var idx = url.IndexOf("utm_source=", StringComparison.Ordinal);
            return idx == -1 ? url : url.Substring(0, idx - 1);
        }

        private static string AddWww(string url)
        {
            if (new Uri(url).Host.Split('.').Length == 2 && !url.Contains("://www."))
            {
                return url.Replace("://", "://www.");
            }
            return url;
        }

        private static string RemoveDuplicateSlashes(string url)
        {
            return url.Replace("//", "/")
                .Replace(":/", "://");
        }

        private static string LimitProtocols(string url)
        {
            return new Uri(url).Scheme == "https" ? url.Replace("https://", "http://") : url;
        }

        private static string RemoveTheFragment(string url)
        {
            var fragment = new Uri(url).Fragment;
            return string.IsNullOrWhiteSpace(fragment) ? url : url.Replace(fragment, string.Empty);
        }

        private static string UrlToLower(Uri uri)
        {
            return HttpUtility.UrlDecode(uri.AbsoluteUri.ToLowerInvariant());
        }

        private static string RemoveTrailingSlashAndEmptyQuery(string url)
        {
            return url
                    .TrimEnd(new[] { '?' })
                    .TrimEnd(new[] { '/' });
        }

        private static string RemoveDefaultDirectoryIndexes(string url)
        {
            foreach (var index in DefaultDirectoryIndexes)
            {
                if (url.EndsWith(index))
                {
                    url = url.TrimEnd(index.ToCharArray());
                    break;
                }
            }
            return url;
        }

        private static string SortQueryString(string url)
        {
            var queryStart = url.IndexOf("?", StringComparison.InvariantCultureIgnoreCase);
            if (queryStart < 0)
                return url;

            var queries = url.Substring(queryStart + 1)
                .Split('&')
                .ToList();

            queries.Sort((a, b) =>
            {
                var ka = a.Split('=').First();
                var kb = b.Split('=').First();
                return String.Compare(ka, kb, StringComparison.InvariantCultureIgnoreCase);
            });

            return url.Substring(0, queryStart + 1) + string.Join("&", queries.ToArray());
        }
    }
}
