using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MockItUp.IntegrationTest
{
    class SampleService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _orderUrl;
        private readonly string _shipmentUrl;
        public SampleService()
        {
            _orderUrl = EnvArguments.GetServiceUrl("order");
            _shipmentUrl = EnvArguments.GetServiceUrl("shipment");
            _httpClient = new HttpClient();
        }

        public Task<dynamic> GetOrderAsync(int orderId)
        {
            return GetAsync($"{_orderUrl}/api/orders/{orderId}");
        }

        public Task<dynamic> CreateOrderAsync(dynamic orderObj)
        {
            return PutAsync($"{_orderUrl}/api/orders", orderObj);
        }

        public Task<dynamic> GetShipmentAsync(string shipmentId)
        {
            return GetAsync($"{_shipmentUrl}/api/shipments/{shipmentId}");
        }

        public Task<dynamic> CreateShipmentAsync(dynamic shipmentObj)
        {
            return PutAsync($"{_shipmentUrl}/api/shipments", shipmentObj);
        }

        public async Task<dynamic> GetAsync(string path)
        {
            var response = await _httpClient.GetAsync(path);
            return await ReadResponseAsync<dynamic>(response.Content);
        }

        public async Task<dynamic> PutAsync(string path, dynamic obj)
        {
            var response = await _httpClient.PutAsync(path,
               new StringContent(JsonConvert.SerializeObject(obj)));
           return await ReadResponseAsync<dynamic>(response.Content);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        private async Task<T> ReadResponseAsync<T>(HttpContent httpContent)
        {
            var str = await httpContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
