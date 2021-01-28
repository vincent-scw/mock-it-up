using Xunit;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MockItUp.Core.Tests
{
    public class HostConfigurationTest
    {
        [Fact]
        public void DeserializeFromYaml_ShouldAsExpected()
        {
            var yaml = @"
version: v1
hosts:
  order: http://localhost:1000/
  shipment: http://localhost:1010/
";

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

            var hc = deserializer.Deserialize<Common.Contracts.HostConfiguration>(yaml);
            Assert.Equal("v1", hc.Version);
            Assert.Equal(2, hc.Services.Count);
        }
    }
}
