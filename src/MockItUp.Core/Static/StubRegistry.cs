using MockItUp.Common;
using MockItUp.Common.Utilities;
using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MockItUp.Core.Static
{
    class StubRegistry : IStubRegistry
    {
        public StubRegistry(string directryPath)
        {
            if (!Directory.Exists(directryPath))
                return;

            var stubs = new Dictionary<string, IList<StubItem>>();
            var specs = LoadDirectory(directryPath);
            foreach (var g in specs.GroupBy(x => x.Service))
            {
                stubs.Add(g.Key, g.SelectMany(s => s.Stubs).ToList());
            }
            Stubs = stubs;
        }

        public IReadOnlyDictionary<string, IList<StubItem>> Stubs { get; private set; }

        private IList<SpecDeclaration> LoadDirectory(string directoryPath)
        {
            var list = new List<SpecDeclaration>();

            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                if (!".yml|.yaml".Contains(Path.GetExtension(file)))
                    continue;

                var content = File.ReadAllText(file);
                try
                {
                    var spec = YamlSerializer.Deserialize<SpecDeclaration>(content);
                    list.Add(spec);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Load spec error: {ex.Message}", ex);
                    throw ex;
                }
            }

            return list;
        }
    }
}
