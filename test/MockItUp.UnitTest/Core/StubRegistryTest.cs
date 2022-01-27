using MockItUp.Core.Dynamic;
using MockItUp.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MockItUp.UnitTest.Core
{
    public class StubRegistryTest
    {
        [Fact]
        public void RegisterAndMove_ShouldAsExpected()
        {
            var registry = new StubRegistry();

            var firstStub = new StubItem();
            var secondStub = new StubItem();

            registry.Register(firstStub, "");

            Assert.Equal(1, registry.Stubs.Count);
            Assert.Equal(1, registry.Stubs["*"].Count);

            registry.Register(secondStub, "");
            // Verify new registered stub always be the first
            Assert.Equal(secondStub, registry.Stubs["*"].First());

            registry.Remove(registry.Stubs["*"].Select(x => x.ID.ToString()).ToList());
            Assert.Equal(0, registry.Stubs["*"].Count);
        }

        [Fact]
        public async Task Register_ShouldWorkInMultiThreading()
        {
            var registry = new StubRegistry();

            var t1 = Task.Run(async () => await Register(registry));
            var t2 = Task.Run(async () => await Register(registry));
            var t3 = Task.Run(async () => await Register(registry));
            await Task.WhenAll(t1, t2, t3);

            await Task.Delay(3000);
            Assert.Equal(100 * 3, registry.Stubs.SelectMany(x => x.Value).Count());
        }

        private async Task Register(StubRegistry registry)
        {
            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                await Task.Delay(random.Next(10));
                registry.Register(new StubItem(), "");
            }
        }
    }
}
