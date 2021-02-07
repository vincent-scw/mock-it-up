using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MockItUp.IntegrationTest
{
    public class DynamicTests
    {
        [Fact]
        public async Task DynamicStub_ShouldWork()
        {
            var client = new Client.MockClient("127.0.0.1:30000");
            var result = await client.RegisterDynamicStubAsync(new DynamicStub());

            Assert.NotNull(result);
            Console.WriteLine(result.StubID);
        }
    }
}
