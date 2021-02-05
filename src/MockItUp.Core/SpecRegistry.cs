using MockItUp.Common;
using MockItUp.Core.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace MockItUp.Core
{
    public class SpecRegistry : ISpecRegistry
    {
        private readonly ISpecLoader _loader;
        public SpecRegistry(ISpecLoader loader)
        {
            _loader = loader;
        }

        public IDictionary<string, IReadOnlyCollection<SpecDeclaration>> GroupedSpecs { get; private set; }

        public IReadOnlyCollection<SpecDeclaration> GetSpecs(string type)
        {
            if (GroupedSpecs?.ContainsKey(type) != true)
                return new List<SpecDeclaration>();

            return GroupedSpecs[type];
        }

        public void RegisterDirectory(string path)
        {
            var specs = _loader.LoadDirectory(path);

            GroupedSpecs = new Dictionary<string, IReadOnlyCollection<SpecDeclaration>>();
            foreach (var g in specs.GroupBy(x => x.Type))
            {
                GroupedSpecs.Add(g.Key.ToLower(), g.ToList());
            }
        }
    }
}
