using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MockItUp.Core.Static
{
    public class StaticMockResponseResolver : MockResponseResolverBase
    {
        private const string WILDCARD = "*";

        private readonly IReadOnlyDictionary<string, IList<RuleItem>> _items;

        public StaticMockResponseResolver(ISpecRegistry registry)
        {
            _items = BuildItemDictionary(registry.Specs);
        }

        public override IEnumerable<RuleItem> GetCandidates(string service)
        {
            return _items.ContainsKey(service) ? _items[service] : _items.SelectMany(x => x.Value);
        }

        private static IReadOnlyDictionary<string, IList<RuleItem>> BuildItemDictionary(IList<SpecDeclaration> specs)
        {
            var ret = new Dictionary<string, IList<RuleItem>>();
            foreach (RestfulSpecDeclaration spec in specs)
            {
                var key = string.IsNullOrEmpty(spec.Service) ? WILDCARD : spec.Service;
                if (!ret.ContainsKey(key))
                    ret.Add(key, new List<RuleItem>());

                ret[key] = ret[key].Concat(spec.Rules).ToList();
            }

            return ret;
        }
    }
}
