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

        public TestScenario BeginScenario()
        {
            return new TestScenario(_client);
        }

        public void Dispose()
        {
            _channel.ShutdownAsync().Wait();
        }
    }
}
