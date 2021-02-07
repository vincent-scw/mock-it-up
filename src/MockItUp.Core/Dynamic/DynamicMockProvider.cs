using MockItUp.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MockItUp.Core.Dynamic
{
    public class DynamicMockProvider : IMockProvider
    {
        public DynamicMockProvider()
        {
            Registry = new StubRegistry();

            StubMatcher = new MockStubMatcher(Registry);
            ResponseResolver = new MockResponseResolver();
        }

        public StubRegistry Registry { get; }

        public IMockStubMatcher StubMatcher { get; }
        public IMockResponseResolver ResponseResolver { get; }
    }
}
