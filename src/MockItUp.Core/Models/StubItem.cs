using System;
using UriTemplate.Core;

namespace MockItUp.Core.Models
{
    public class StubItem
    {
        public string ID { get; private set; }
        public RequestModel Request { get; set; }
        public ResponseModel Response { get; set; }

        public void SetID(string id)
        {
            ID = id;
        }

        public UriTemplateMatch Match(string method, Uri url)
        {
            // Check http method
            if (!Request.Method.Equals(method, StringComparison.InvariantCultureIgnoreCase))
                return null;

            // Check path
            // Use requested host to match template
            if (!Uri.TryCreate(UrlNormalizer.NormalizeUrl($"{url.Scheme}://{url.Authority}/{Request.Path}"), UriKind.Absolute, out Uri formatted))
                return null;

            var normalizedCandidate = url.NormalizeUrl();
            if (!Uri.TryCreate(normalizedCandidate, UriKind.Absolute, out Uri candidateUrl))
                return null;

            var template = new UriTemplate.Core.UriTemplate(formatted.OriginalString);
            var matchResult = template.Match(candidateUrl);
            // TODO: in match result, the url bindings will be all in lower case
            return matchResult;
        }
    }
}
