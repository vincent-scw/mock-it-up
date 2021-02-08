using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MockItUp.Core.Dynamic;
using MockItUp.Core.Models;
using System;
using System.Threading.Tasks;

namespace MockItUp.Console
{
    class MockControllerImpl : MockController.MockControllerBase
    {
        private readonly DynamicMockProvider _mockProvider;
        public MockControllerImpl(IServiceProvider serviceProvider)
        {
            _mockProvider = serviceProvider.GetService(typeof(DynamicMockProvider)) as DynamicMockProvider;
        }

        public override Task<RegisterResult> RegisterDynamicStub(DynamicStub request, ServerCallContext context)
        {
            var req = request.Request;
            var res = request.Response;
            var stubId = _mockProvider.Registry.Register(new StubItem 
            {
                Request = new RequestModel
                {
                    Method = req.Method,
                    Path = req.UriTemplate,
                    BodyType = BodyType.Direct
                },
                Response = new ResponseModel
                {
                    Body = res.Body,
                    StatusCode = res.StatusCode
                }
            }, req.Service);

            var result = new RegisterResult { Succeed = true, StubID = stubId.ToString() };
            return Task.FromResult(result);
        }

        public override Task<RemoveResult> RemoveDynamicStubs(StubIDs request, ServerCallContext context)
        {
            _mockProvider.Registry.Remove(request.IdList);
            return Task.FromResult(new RemoveResult { Succeed = true });
        }
    }
}
