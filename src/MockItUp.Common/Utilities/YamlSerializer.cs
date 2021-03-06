﻿using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MockItUp.Common.Utilities
{
    public sealed class YamlSerializer
    {
        private static IDeserializer _deserializer;
        static YamlSerializer()
        {
            _deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
        }

        public static T Deserialize<T>(string content)
        {
            return _deserializer.Deserialize<T>(content);
        }

        public static T DeserializeFile<T>(string file)
        {
            var content = File.ReadAllText(file);
            return Deserialize<T>(content);
        }
    }
}
