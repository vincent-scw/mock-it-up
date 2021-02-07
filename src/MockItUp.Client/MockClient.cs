using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace MockItUp.Client
{
    public class MockClient : IDisposable
    {
        private readonly Channel _channel;
        private readonly MockController.MockControllerClient _client;
        public MockClient(string address)
        {
            _channel = new Channel(address, ChannelCredentials.Insecure);
            _client = new MockController.MockControllerClient(_channel);
        }

        public async Task<RegisterResult> RegisterDynamicStubAsync(DynamicStub stub)
        {
            var result = await _client.RegisterDynamicStubAsync(stub);
            return result;
        }

        public void Dispose()
        {
            _channel.ShutdownAsync().Wait();
        }
    }
}
