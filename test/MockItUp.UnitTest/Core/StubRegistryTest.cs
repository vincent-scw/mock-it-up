using MockItUp.Core.Dynamic;
using MockItUp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MockItUp.UnitTest.Core
{
    public class StubRegistryTest
    {
        [Fact]
        public void RegisterAndMove_ShouldAsExpected()
        {
            var registry = new StubRegistry();
            registry.Register(new StubItem(), "");

            Assert.Equal(1, registry.Stubs.Count);
            Assert.Equal(1, registry.Stubs["*"].Count);

            registry.Remove(registry.Stubs["*"].Select(x => x.ID.ToString()).ToList());
            Assert.Equal(0, registry.Stubs["*"].Count);
        }
    }
}
