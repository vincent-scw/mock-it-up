using System;
using System.Collections.Generic;
using System.Text;

namespace MockItUp.Common.Contracts
{
    public interface ISpecRegistry
    {
        IDictionary<string, IReadOnlyCollection<SpecDeclaration>> GroupedSpecs { get; }

        IReadOnlyCollection<SpecDeclaration> GetSpecs(string type);
        void RegisterDirectory(string path);
    }
}
