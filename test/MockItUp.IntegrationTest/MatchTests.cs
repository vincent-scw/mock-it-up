using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MockItUp.IntegrationTest
{
    [Collection("IntegrationTest")]
    public class MatchTests : IDisposable
    {
        private readonly HttpClient _client;
        public MatchTests()
        {
            _client = new HttpClient();
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        [Fact]
        public async Task NoMatches_ShouldFailed()
        {
            var orderUrl = EnvArguments.GetServiceUrl("order");
            var response = await _client.GetAsync($"{orderUrl}/api/nomatches");
            var result = await response.Content.ReadAsStringAsync();

            Assert.Equal("Cannot find matched rule. Request ignored.", result);
        }
    }
}
