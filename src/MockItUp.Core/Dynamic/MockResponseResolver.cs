using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using UriTemplate.Core;

namespace MockItUp.Core.Dynamic
{
    public class MockResponseResolver : IMockResponseResolver
    {
        public ResponseModel Resolve(RequestModel request, StubItem stub, UriTemplateMatch match)
        {
            var res = new ResponseModel();
            var responseModel = stub.Response;

            res.StatusCode = responseModel.StatusCode;
            res.ContentType = responseModel.ContentType;
            res.Headers = responseModel.Headers;

            // No body, return
            if (string.IsNullOrEmpty(responseModel.Body))
                return res;

            res.Body = responseModel.Body;

            return res;
        }
    }
}
