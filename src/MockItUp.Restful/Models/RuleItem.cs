namespace MockItUp.Restful.Models
{
    public class RuleItem
    {
        public RequestModel Request { get; set; }
        public ResponseModel Response { get; set; }

        public bool Matches(System.Net.HttpListenerRequest request, System.Uri baseUri)
        {
            // Check http method
            if (!Request.Method.Equals(request.HttpMethod, System.StringComparison.InvariantCultureIgnoreCase))
                return false;

            // Check path
            var template = new UriTemplate.Core.UriTemplate(Request.Path);
            var matchResult = template.Match(baseUri, request.Url);
            if (matchResult == null)
                return false;

            // Check headers

            // Check body

            return true;
        }
    }
}
