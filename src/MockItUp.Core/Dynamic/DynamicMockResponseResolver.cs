using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MockItUp.Core.Dynamic
{
    public class DynamicMockResponseResolver : IMockResponseResolver
    {
        public IReadOnlyDictionary<string, IList<RuleItem>> Stubs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
