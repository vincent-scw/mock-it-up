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
        public UriTemplateMatch Match(HttpListenerRequest request, string service, out StubItem stub)
        {
            stub = null;
            return null;
        }
    }
}
