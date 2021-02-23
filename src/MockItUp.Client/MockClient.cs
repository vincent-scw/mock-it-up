using Grpc.Core;
using System;

namespace MockItUp.Client
{
    public class MockClient : IDisposable
    {
        private readonly Channel _channel;
        private readonly Mockctl.MockController.MockControllerClient _client;
        /// <summary>
        /// Create a mock client
        /// </summary>
        /// <param name="address">Mock server address (eg, mockserver:30000)</param>
        public MockClient(string address)
        {
            _channel = new Channel(address, ChannelCredentials.Insecure);
            _client = new Mockctl.MockController.MockControllerClient(_channel);
        }

        /// <summary>
        /// Create a test scenario
        /// </summary>
        /// <returns></returns>
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
