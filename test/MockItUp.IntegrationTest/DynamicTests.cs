using MockItUp.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MockItUp.IntegrationTest
{
    [Collection("Dynamic")]
    public class DynamicTests : IDisposable
    {
        private readonly string _orderUrl;
        private readonly MockClient _client;
        public DynamicTests()
        {
            _orderUrl = EnvArguments.GetServiceUrl("order");
            _client = new MockClient(EnvArguments.GetCtlService());
        }

        public void Dispose()
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }

        [Fact]
        public async Task DynamicStub_ShouldWork()
        {
            using var httpClient = new HttpClient();

            using (var scenario = _client.BeginScenario())
            {
                var orderId = 215;
                var regResult = scenario.RegisterDynamicStub(new DynamicStub
                {
                    Request = new Request { Method = "GET", UriTemplate = "api/orders/{id}" },
                    Response = new Response
                    {
                        StatusCode = 200,
                        Body = JsonConvert.SerializeObject(new
                        {
                            id = orderId,
                            title = "this is a test"
                        })
                    }
                });

                Assert.NotNull(regResult);
                Assert.True(regResult.Succeed);

                var response = await httpClient.GetAsync($"{_orderUrl}/api/orders/{orderId}");
                var order = await ReadResponseAsync<dynamic>(response.Content);

                Assert.Equal(orderId, (int)order.id);
                Assert.Equal("this is a test", (string)order.title);
            }
        }

        private async Task<T> ReadResponseAsync<T>(HttpContent httpContent)
        {
            var str = await httpContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
