using System.Collections.Generic;

namespace MockItUp.Common
{
    public interface ISpecLoader
    {
        SpecDeclaration Load(string content);
        IList<SpecDeclaration> LoadDirectory(string directoryPath);
    }
}
