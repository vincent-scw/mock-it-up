using MockItUp.Common.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MockItUp.Core
{
    public class SpecLoader : ISpecLoader
    {
        private readonly IDeserializer _deserializer;

        public SpecLoader()
        {
            _deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();
        }

        public SpecDeclaration Load(string content)
        {
            var model = _deserializer.Deserialize<SpecDeclaration>(content);
            switch (model.Type.ToLower())
            {
                case "restful":
                    return _deserializer.Deserialize<Restful.Models.RestfulSpecDeclaration>(content);
                default:
                    throw new NotSupportedException($"Spec type {model.Type.ToLower()} is not supported.");
            }
        }

        public IList<SpecDeclaration> LoadDirectory(string directoryPath)
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
                    var spec = Load(content);
                    list.Add(spec);
                }
                catch (Exception ex)
                {
                    //TODO: log
                    throw ex;
                }
            }

            return list;
        }
    }
}
