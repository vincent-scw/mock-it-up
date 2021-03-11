using System.Collections.Generic;

namespace MockItUp.Client
{
    public class DynamicStub
    {
        internal Mockctl.DynamicStub InternalStub { get; private set; }
        
        internal DynamicStub()
        {
            InternalStub = new Mockctl.DynamicStub();
        }

        /// <summary>
        /// Setup request
        /// </summary>
        /// <param name="method">Http method</param>
        /// <param name="uriTemplate">Uri template</param>
        /// <param name="service">The service name</param>
        /// <returns></returns>
        public DynamicStub WhenRequest(string method, string uriTemplate, string service = "*")
        {
            InternalStub.Request = new Mockctl.RequestDef
            {
                Method = method ?? "", // Grpc.Tools doesn't support optional null, need to parse empty string here
                UriTemplate = uriTemplate ?? "",
                Service = service ?? ""
            };
            return this;
        }

        /// <summary>
        /// Setup expected response
        /// </summary>
        /// <param name="body">Body</param>
        /// <param name="statusCode">Status code</param>
        /// <param name="contentType">Content type</param>
        /// <param name="headers">Headers</param>
        /// <returns></returns>
        public DynamicStub RespondWith(string body, int statusCode = 200, string contentType = "application/json", IDictionary<string, string> headers = null)
        {
            InternalStub.Response = new Mockctl.ResponseDef
            {
                Body = body ?? "",
                StatusCode = statusCode,
                ContentType = contentType ?? "",
            };

            if (headers != null)
            {
                InternalStub.Response.Headers.Add(headers);
            }

            return this;
        }

        /// <summary>
        /// Setup expected response without body
        /// </summary>
        /// <param name="statusCode">Status code</param>
        /// <param name="contentType">Content type</param>
        /// <param name="headers">Headers</param>
        /// <returns></returns>
        public DynamicStub RespondWith(int statusCode, string contentType = "application/json", IDictionary<string, string> headers = null)
        {
            return RespondWith(null, statusCode, contentType, headers);
        }
    }
}
