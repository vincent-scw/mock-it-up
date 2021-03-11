using MockItUp.Client;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MockItUp.IntegrationTest
{
    [Collection("IntegrationTest")]
    public class DynamicTests : IDisposable
    {
        private readonly SampleService _service;
        private readonly MockClient _client;
        public DynamicTests()
        {
            _service = new SampleService();
            _client = new MockClient(EnvArguments.GetCtlService());
        }

        public void Dispose()
        {
            _service.Dispose();
            _client.Dispose();
        }

        [Fact]
        public async Task DynamicStub_ShouldWork()
        {
            using (var scenario = _client.BeginScenario())
            {
                var orderId = 215;
                var regResult = scenario.RegisterDynamicStub(stub =>
                    stub.WhenRequest("GET", "api/orders/{id}")
                        .RespondWith(JsonConvert.SerializeObject(new
                        {
                            id = orderId,
                            title = "this is a test"
                        }))
                );

                Assert.NotNull(regResult);
                Assert.True(regResult.Succeed);

                var order = await _service.GetOrderAsync(orderId);

                Assert.Equal(orderId, (int)order.id);
                Assert.Equal("this is a test", (string)order.title);
            }
        }

        [Fact]
        public async Task Re_registerDynamicStub_ShouldWork()
        {
            using (var scenario = _client.BeginScenario())
            {
                var orderId = 215;
                await scenario.RegisterDynamicStubAsync(stub =>
                    stub.WhenRequest("GET", "api/orders/{id}")
                        .RespondWith(JsonConvert.SerializeObject(new
                        {
                            status = "Created"
                        }))
                );

                var order = await _service.GetOrderAsync(orderId);
                Assert.Equal("Created", (string)order.status);

                await scenario.RegisterDynamicStubAsync(stub =>
                    stub.WhenRequest("GET", "api/orders/{id}")
                        .RespondWith(JsonConvert.SerializeObject(new
                        {
                            status = "Removed"
                        }))
                );

                var updated = await _service.GetOrderAsync(orderId);
                Assert.Equal("Removed", (string)updated.status);
            }

            var records = await _client.GetLastNRecordsAsync();
            Assert.True(records.Count > 0);
        }
    }
}
