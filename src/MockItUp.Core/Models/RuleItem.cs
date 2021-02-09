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
            var template = new UriTemplate.Core.UriTemplate(($"{url.Scheme}://{url.Authority}/{Request.Path}"));
            var matchResult = template.Match(url);
            return matchResult;
        }
    }
}
