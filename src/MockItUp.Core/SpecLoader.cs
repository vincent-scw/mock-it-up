using MockItUp.Common.Contracts;
using System;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MockItUp.Core
{
    public class SpecLoader
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
    }
}
