using System.Collections.Generic;

namespace MockItUp.Core.Contracts
{
    public interface ISpecRegistry
    {
        IDictionary<string, IReadOnlyCollection<SpecDeclaration>> GroupedSpecs { get; }

        IReadOnlyCollection<SpecDeclaration> GetSpecs(string type);
        void RegisterDirectory(string path);
    }
}
