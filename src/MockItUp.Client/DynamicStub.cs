using System.Collections.Generic;

namespace MockItUp.Client
{
    public class DynamicStub
    {
        internal Mockctl.DynamicStub InternalStub { get; private set; }
        
        public DynamicStub()
        {
            InternalStub = new Mockctl.DynamicStub();
        }

        public DynamicStub WhenRequest(string method, string uriTemplate, string service = "*")
        {
            InternalStub.Request = new Mockctl.Request
            {
                Method = method, UriTemplate = uriTemplate,
                Service = service
            };
            return this;
        }

        public DynamicStub RespondWith(string body, int statusCode = 200, string contentType = "application/json", IDictionary<string, string> headers = null)
        {
            InternalStub.Response = new Mockctl.Response
            {
                Body = body,
                StatusCode = statusCode,
                ContentType = contentType,
            };

            if (headers != null)
            {
                InternalStub.Response.Headers.Add(headers);
            }

            return this;
        }
    }
}
