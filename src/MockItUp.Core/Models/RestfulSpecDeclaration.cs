using MockItUp.Common;
using System.Collections.Generic;

namespace MockItUp.Core.Models
{
    public class RestfulSpecDeclaration : SpecDeclaration
    {
        public IList<RuleItem> Rules { get; set; }
    }
}
