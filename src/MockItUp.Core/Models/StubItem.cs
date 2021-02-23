using System;
using UriTemplate.Core;

namespace MockItUp.Core.Models
{
    public class StubItem
    {
        public Guid ID { get; private set; }
        public RequestModel Request { get; set; }
        public ResponseModel Response { get; set; }

        public void SetID(Guid id)
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
            if (!Uri.TryCreate(new Uri($"{url.Scheme}://{url.Authority}"), Request.Path, out Uri formatted))
                return null;

            var template = new UriTemplate.Core.UriTemplate(formatted.OriginalString);
            var matchResult = template.Match(url);
            return matchResult;
        }
    }
}
