﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using Burro.Config;
using NUnit.Framework;
using Ninject;
using YamlDotNet.RepresentationModel;

namespace Burro.Tests.Config
{
    [TestFixture]
    public class GoConfigParserTest
    {
        [Test]
        public void ParsesGoServerCorrectly()
        {
            var kernel = new StandardKernel();

            var parser = kernel.Get<GoConfigParser>();

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
            Assert.AreEqual("http://goserver.localdomain:8153/go", goServer.URL);
            Assert.AreEqual("ci", goServer.Username);
            Assert.AreEqual("secret", goServer.Password);
            Assert.IsNotNull(goServer.Pipelines);
            Assert.AreEqual(2, goServer.Pipelines.Count());
        }
    }
}