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
            StubMatcher = new MockStubMatcher();
            ResponseResolver = new MockResponseResolver();
        }

        public IMockStubMatcher StubMatcher { get; }
        public IMockResponseResolver ResponseResolver { get; }
    }
}
