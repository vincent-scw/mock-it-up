using System;
using System.Threading.Tasks;
using Xunit;

namespace MockItUp.IntegrationTest
{
    public class StaticTests : IDisposable
    {
        private readonly SampleService _service;
        public StaticTests()
        {
            _service = new SampleService();
        }

        public void Dispose()
        {
            _service.Dispose();
        }

        [Fact]
        public async Task ViewShipment()
        {
            var orderId = 10000;
            var shipmentId = "SH20210101LA";

            // Create order
            var orderRes = await _service.CreateOrderAsync(new
            {
                customer = new
                {
                    id = "C100",
                    name = "somebody"
                }
            });
            Assert.Equal("C100", (string)orderRes.customerId);

            // Create shipment
            var shipmentRes = await _service.CreateShipmentAsync(new
            {
                shipmentId,
                routing = new string[] { "CNSHA", "KRPUS", "USLAX" }
            });
            Assert.Equal(shipmentId, (string)shipmentRes.shipment.id);

            // Get order
            var orderGet = await _service.GetOrderAsync(orderId);
            Assert.Equal(orderId, (int)orderGet.orderId);
            Assert.Equal(shipmentId, (string)orderGet.shipmentId);

            var shipmentGet = await _service.GetShipmentAsync(shipmentId);
            Assert.Equal(shipmentId, (string)shipmentGet.shipmentId);
        }
    }
}
