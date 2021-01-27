using System;
using System.Collections.Generic;
using System.Text;

namespace MockItUp.Common.Contracts
{
    public interface ISpecLoader
    {
        SpecDeclaration Load(string content);
        IList<SpecDeclaration> LoadDirectory(string directoryPath);
    }
}
