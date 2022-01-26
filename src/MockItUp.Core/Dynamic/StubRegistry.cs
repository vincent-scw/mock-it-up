using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MockItUp.Core.Dynamic
{
    public class StubRegistry : IStubRegistry
    {
        private readonly ConcurrentDictionary<string, IList<StubItem>> _stubDict;
        public StubRegistry()
        {
            _stubDict = new ConcurrentDictionary<string, IList<StubItem>>();
        }

        public IReadOnlyDictionary<string, IList<StubItem>> Stubs => _stubDict;

        public string Register(StubItem stub, string service)
        {
            var key = service;
            if (string.IsNullOrEmpty(service))
                key = "*";

            var id = IdGen.Generate();
            stub.SetID(id);

            IList<StubItem> items;
            if (_stubDict.ContainsKey(key))
                items = _stubDict[key];
            else
                items = new List<StubItem>();

            // Insert the new stub to be the first one
            items.Insert(0, stub);
            _stubDict[key] = items;

            return id;
        }

        public void Remove(IList<string> ids)
        {
            var keys = new string[_stubDict.Keys.Count];
            _stubDict.Keys.CopyTo(keys, 0);
            foreach (var key in keys)
            {
                _stubDict[key] = _stubDict[key].Where(x => !ids.Contains(x.ID.ToString())).ToList();
            }
        }
    }
}
