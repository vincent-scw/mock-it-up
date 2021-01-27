using MockItUp.Common.Contracts;
using System.Collections.Generic;

namespace MockItUp.Restful.Models
{
    public class RestfulSpecDeclaration : SpecDeclaration
    {
        public IList<RuleItem> Rules { get; set; }
    }
}
