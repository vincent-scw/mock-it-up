using System.Collections.Generic;

namespace MockItUp.Core.Contracts
{
    public interface ISpecLoader
    {
        SpecDeclaration Load(string content);
        IList<SpecDeclaration> LoadDirectory(string directoryPath);
    }
}
