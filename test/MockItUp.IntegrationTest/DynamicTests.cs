using MockItUp.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MockItUp.IntegrationTest
{
    [Collection("Dynamic")]
    public class DynamicTests
    {
        [Fact]
        public async Task DynamicStub_ShouldWork()
        {
            var client = new MockClient(EnvArguments.GetCtlService());
            var result = await client.RegisterDynamicStubAsync(new DynamicStub());

            Assert.NotNull(result);
            Console.WriteLine(result.StubID);
        }
    }
}
