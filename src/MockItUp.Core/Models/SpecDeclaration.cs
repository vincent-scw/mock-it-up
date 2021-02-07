using MockItUp.Core.Models;
using System.Collections.Generic;

namespace MockItUp.Core
{
    public class SpecDeclaration
    {
        public string Version { get; set; }
        public string Service { get; set; }
        public IList<StubItem> Stubs { get; set; }
    }
}
