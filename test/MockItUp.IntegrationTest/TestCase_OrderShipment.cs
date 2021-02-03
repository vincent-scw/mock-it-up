using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MockItUp.IntegrationTest
{
    public class TestCase_OrderShipment
    {
        private readonly string _orderUrl;
        private readonly string _shipmentUrl;
        public TestCase_OrderShipment()
        {
            _orderUrl = EnvArguments.GetServiceUrl("order") + "/api";
            _shipmentUrl = EnvArguments.GetServiceUrl("shipment") + "/api";
        }

        [Fact]
        public async Task ViewShipment()
        {
            var orderId = 10000;
            var shipmentId = "SH20210101LA";

            var httpClient = new HttpClient();

            // Create order
            var response = await httpClient.PutAsync($"{_orderUrl}/orders", new StringContent("{orderId: 10000}"));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            // Create shipment
            response = await httpClient.PutAsync($"{_shipmentUrl}/shipments", new StringContent("{orderId: 10000, from:\"CNSHA\", to:\"LA\"}"));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            // Get order
            response = await httpClient.GetAsync($"{_orderUrl}/orders/{orderId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var order = await ReadResponseAsync<dynamic>(response.Content);
            Assert.Equal(10000, (int) order.orderId);
            Assert.Equal(shipmentId, (string) order.shipmentId);

            response = await httpClient.GetAsync($"{_shipmentUrl}/shipments/{shipmentId}");
            var shipment = await ReadResponseAsync<dynamic>(response.Content);
            Assert.Equal(shipmentId, (string) shipment.shipmentId);
        }

        private async Task<T> ReadResponseAsync<T>(HttpContent httpContent)
        {
            var str = await httpContent.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
    }
}
