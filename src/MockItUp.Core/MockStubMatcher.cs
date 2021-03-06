﻿using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UriTemplate.Core;

namespace MockItUp.Core
{
    public class MockStubMatcher : IMockStubMatcher
    {
        private readonly IStubRegistry _stubRegistry;
        public MockStubMatcher(IStubRegistry stubRegistry)
        {
            _stubRegistry = stubRegistry;
        }

        private IEnumerable<StubItem> GetCandidates(string service)
        {
            if (_stubRegistry.Stubs == null)
                return null;

            return _stubRegistry.Stubs.ContainsKey(service) ? _stubRegistry.Stubs[service] : _stubRegistry.Stubs.SelectMany(x => x.Value);
        }

        public UriTemplateMatch Match(HttpListenerRequest request, string service, out StubItem stub)
        {
            stub = null;
            UriTemplateMatch match = null;
            var candidates = GetCandidates(service);
            if (candidates == null)
                return null;
            foreach (var c in candidates)
            {
                match = c.Match(request.HttpMethod, request.Url);
                if (match != null)
                {
                    stub = c;
                    return match;
                }
            }

            return match;
        }
    }
}
