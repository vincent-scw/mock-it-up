using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace MockItUp.Console
{
    class MockControllerImpl : MockController.MockControllerBase
    {
        public override Task<RegisterResult> RegisterDynamicStub(DynamicStub request, ServerCallContext context)
        {
            var stubId = Guid.NewGuid().ToString();
            var result = new RegisterResult { StubID = stubId };
            return Task.FromResult(result);
        }
    }
}
