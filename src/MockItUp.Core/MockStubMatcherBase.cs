using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UriTemplate.Core;

namespace MockItUp.Core
{
    abstract class MockStubMatcherBase : IMockStubMatcher
    {
        public abstract IEnumerable<RuleItem> GetCandidates(string service);

        public UriTemplateMatch Match(HttpListenerRequest request, string service, out RuleItem stub)
        {
            stub = null;
            UriTemplateMatch match = null;
            foreach (var c in GetCandidates(service))
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
