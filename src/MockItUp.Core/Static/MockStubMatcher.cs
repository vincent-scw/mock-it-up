using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MockItUp.Core.Static
{
    public class MockStubMatcher : MockStubMatcherBase
    {
        private readonly IReadOnlyDictionary<string, IList<StubItem>> _items;
        public MockStubMatcher(IStubRegistry registry)
        {
            _items = registry.Stubs;
        }

        public override IEnumerable<StubItem> GetCandidates(string service)
        {
            return _items.ContainsKey(service) ? _items[service] : _items.SelectMany(x => x.Value);
        }
    }
}
