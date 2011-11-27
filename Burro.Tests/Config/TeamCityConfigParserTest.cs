using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using Burro.Config.Parsers;
using Burro.Parsers;
using Burro.Util;
using Moq;
using NUnit.Framework;
using Ninject;
using YamlDotNet.RepresentationModel;

namespace Burro.Tests.Config
{
    [TestFixture]
    public class TeamCityConfigParserTest
    {
        [Test]
        public void ParsesTeamCityServerCorrectly()
        {
            var kernel = new StandardKernel();

            var parser = kernel.Get<TeamCityConfigParser>();

            kernel.Bind<ITimer>().ToConstant(new Mock<ITimer>().Object);
            kernel.Bind<IParser>().ToConstant(new Mock<IParser>().Object).Named("TeamCity");

            var config = new YamlMappingNode
                             {
                                 {"url", "http://localhost"},
                                 {"username", "ci"},
                                 {"password", "secret"}
                             };
            var pipeline1 = new YamlMappingNode {{"name", "bt1"}};
            var pipeline2 = new YamlMappingNode { { "name", "bt2" } };
            var pipelines = new YamlSequenceNode {pipeline1, pipeline2};
            config.Add("pipelines",pipelines);

            var teamCityServer = parser.Parse(config) as TeamCityServer;
            Assert.IsNotNull(teamCityServer);
            Assert.IsNotNull(teamCityServer.Config);
            var teamCityServerConfig = teamCityServer.Config;
            Assert.AreEqual("http://localhost", teamCityServerConfig.URL);
            Assert.AreEqual("ci", teamCityServerConfig.Username);
            Assert.AreEqual("secret", teamCityServerConfig.Password);
            Assert.IsNotNull(teamCityServerConfig.Pipelines);
            Assert.AreEqual(2, teamCityServerConfig.Pipelines.Count());
        }
    }
}
