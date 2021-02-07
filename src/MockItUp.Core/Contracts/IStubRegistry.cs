using MockItUp.Core.Models;
using System.Collections.Generic;

namespace MockItUp.Core.Contracts
{
    public interface IStubRegistry
    {
        IReadOnlyDictionary<string, IList<StubItem>> Stubs { get; }
    }
}
