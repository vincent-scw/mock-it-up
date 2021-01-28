using System;
using UriTemplate.Core;

namespace MockItUp.Restful.Models
{
    public class RuleItem
    {
        public RequestModel Request { get; set; }
        public ResponseModel Response { get; set; }

        public UriTemplateMatch Matches(System.Net.HttpListenerRequest request)
        {
            // Check http method
            if (!Request.Method.Equals(request.HttpMethod, StringComparison.InvariantCultureIgnoreCase))
                return null;

            // Check path
            // Use requested host to match template
            var template = new UriTemplate.Core.UriTemplate($"{request.Url.Scheme}://{request.Url.Authority}/{Request.Path}");
            var matchResult = template.Match(request.Url);
            return matchResult;
        }
    }
}
