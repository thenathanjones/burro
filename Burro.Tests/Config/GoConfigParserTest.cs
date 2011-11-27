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
    [TestFixture, Ignore("This logic really applies to Cruise Control and something more Go specific should go here")]
    public class GoConfigParserTest
    {
        [Test]
        public void ParsesGoServerCorrectly()
        {
            var kernel = new StandardKernel();

            var parser = kernel.Get<GoConfigParser>();

            kernel.Bind<ITimer>().ToConstant(new Mock<ITimer>().Object);
            kernel.Bind<IParser>().ToConstant(new Mock<IParser>().Object).Named("Go");

            var config = new YamlMappingNode
                             {
                                 {"url", "http://goserver.localdomain:8153/go"},
                                 {"username", "ci"},
                                 {"password", "secret"}
                             };
            var pipeline1 = new YamlMappingNode {{"name", "Cosby-Kid"}};
            var pipeline2 = new YamlMappingNode { { "name", "Family-Tieman" } };
            var pipelines = new YamlSequenceNode {pipeline1, pipeline2};
            config.Add("pipelines",pipelines);

            var goServer = parser.Parse(config) as GoServer;
            Assert.IsNotNull(goServer);
            Assert.IsNotNull(goServer.Config);
            var goServerConfig = goServer.Config;
            Assert.AreEqual("http://goserver.localdomain:8153/go", goServerConfig.URL);
            Assert.AreEqual("ci", goServerConfig.Username);
            Assert.AreEqual("secret", goServerConfig.Password);
            Assert.IsNotNull(goServerConfig.Pipelines);
            Assert.AreEqual(2, goServerConfig.Pipelines.Count());
        }
    }
}
