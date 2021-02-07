using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UriTemplate.Core;

namespace MockItUp.Core.Dynamic
{
    public class MockStubMatcher : IMockStubMatcher
    {
        private readonly IStubRegistry _stubRegistry;
        public MockStubMatcher(IStubRegistry stubRegistry)
        {
            _stubRegistry = stubRegistry;
        }

        public UriTemplateMatch Match(HttpListenerRequest request, string service, out StubItem stub)
        {
            stub = null;
            return null;
        }
    }
}
