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
            _orderUrl = EnvArguments.GetServiceUrl("order");
            _shipmentUrl = EnvArguments.GetServiceUrl("shipment");
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
            response = await httpClient.GetAsync($"{_orderUrl}/{orderId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var order = await ReadResponseAsync<dynamic>(response.Content);
            Assert.Equal(10000, order.Id);
            Assert.Equal(shipmentId, order.ShipmentId);

            response = await httpClient.GetAsync($"{_shipmentUrl}/{shipmentId}");
            var shipment = await ReadResponseAsync<dynamic>(response.Content);
            Assert.Equal(shipmentId, shipment.ShipmentId);
        }

        private async Task<T> ReadResponseAsync<T>(HttpContent httpContent)
        {
            var str = await httpContent.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
    }
}
