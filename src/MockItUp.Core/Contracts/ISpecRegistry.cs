using System.Collections.Generic;

namespace MockItUp.Core.Contracts
{
    public interface ISpecRegistry
    {
        IList<SpecDeclaration> Specs { get; }

        void RegisterDirectory(string path);
    }
}
