using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public Guid Register(StubItem stub, string service)
        {
            var key = service;
            if (string.IsNullOrEmpty(service))
                key = "*";

            var id = Guid.NewGuid();
            stub.SetID(id);

            IList<StubItem> items;
            if (_stubDict.ContainsKey(key))
                items = _stubDict[key];
            else
                items = new List<StubItem>();

            items.Add(stub);
            _stubDict[key] = items;

            return id;
        }

        public void Remove(IList<string> ids)
        {
            foreach (var d in _stubDict)
            {
                _stubDict[d.Key] = d.Value.Where(x => !ids.Contains(x.ID.ToString())).ToList();
            }
        }
    }
}
