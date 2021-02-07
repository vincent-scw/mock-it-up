using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MockItUp.Core.Dynamic
{
    public class StubRegistry : IStubRegistry
    {
        private readonly Dictionary<string, IList<StubItem>> _stubDict;
        public StubRegistry()
        {
            _stubDict = new Dictionary<string, IList<StubItem>>();
        }

        public IReadOnlyDictionary<string, IList<StubItem>> Stubs => _stubDict;

        public Guid Register(StubItem stub, string service = "*")
        {
            var id = Guid.NewGuid();
            IList<StubItem> items;
            if (_stubDict.ContainsKey(service))
                items = _stubDict[service];
            else
                items = new List<StubItem>();

            items.Add(stub);
            _stubDict[service] = items;

            return id;
        }
    }
}
