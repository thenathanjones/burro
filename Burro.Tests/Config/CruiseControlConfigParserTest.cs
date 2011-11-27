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
    public class CruiseControlConfigParserTest
    {
        [Test]
        public void ParsesCruiseControlServerCorrectly()
        {
            var kernel = new StandardKernel();

            var parser = kernel.Get<CruiseControlConfigParser>();

            kernel.Bind<ITimer>().ToConstant(new Mock<ITimer>().Object);
            kernel.Bind<IParser>().ToConstant(new Mock<IParser>().Object).Named("CruiseControl");

            var config = new YamlMappingNode
                             {
                                 {"url", "http://goserver.localdomain:8153/go/cctray.xml"},
                                 {"username", "ci"},
                                 {"password", "secret"}
                             };
            var pipeline1 = new YamlMappingNode {{"name", "Cosby-Kid"}};
            var pipeline2 = new YamlMappingNode { { "name", "Family-Tieman" } };
            var pipelines = new YamlSequenceNode {pipeline1, pipeline2};
            config.Add("pipelines",pipelines);

            var cruiseControlServer = parser.Parse(config) as CruiseControlServer;
            Assert.IsNotNull(cruiseControlServer);
            Assert.IsNotNull(cruiseControlServer.Config);
            var cruiseControlServerconfig = cruiseControlServer.Config;
            Assert.AreEqual("http://goserver.localdomain:8153/go/cctray.xml", cruiseControlServerconfig.URL);
            Assert.AreEqual("ci", cruiseControlServerconfig.Username);
            Assert.AreEqual("secret", cruiseControlServerconfig.Password);
            Assert.IsNotNull(cruiseControlServerconfig.Pipelines);
            Assert.AreEqual(2, cruiseControlServerconfig.Pipelines.Count());
        }
    }
}
