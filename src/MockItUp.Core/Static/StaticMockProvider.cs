using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using System;

namespace MockItUp.Core.Static
{
    public class StaticMockProvider : IMockProvider
    {
        public StaticMockProvider(IServiceProvider serviceProvider)
        {
            var config = serviceProvider.GetService(typeof(HostConfiguration)) as HostConfiguration;
            var stubRegistry = new StubRegistry(config.SpecDirectory);

            StubMatcher = new MockStubMatcher(stubRegistry);
            ResponseResolver = new MockResponseResolver(config);
        }

        public IMockStubMatcher StubMatcher { get; }
        public IMockResponseResolver ResponseResolver { get; }
    }
}
