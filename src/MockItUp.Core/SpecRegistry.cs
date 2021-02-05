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

        public IList<SpecDeclaration> Specs { get; private set; }

        public void RegisterDirectory(string path)
        {
            Specs = _loader.LoadDirectory(path);
        }
    }
}
