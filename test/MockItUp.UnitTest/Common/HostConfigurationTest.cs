using MockItUp.Core.Models;
using Xunit;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MockItUp.UnitTest.Core
{
    public class HostConfigurationTest
    {
        [Fact]
        public void DeserializeFromYaml_ShouldAsExpected()
        {
            var yaml = @"
version: v1
services:
  order: 1000
  shipment: 1010
";

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

            var hc = deserializer.Deserialize<HostConfiguration>(yaml);
            Assert.Equal("v1", hc.Version);
            Assert.Equal(2, hc.Services.Count);
        }
    }
}
