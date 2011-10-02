using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using NUnit.Framework;

namespace Burro.Tests
{
    [TestFixture]
    public class ConfigTest
    {
        [Test]
        public void LoadsConfigAsYaml()
        {
            var core = new BurroCore();
            core.Initialise("Config\\blank.yml");
            Assert.IsNotNull(core.RawConfig);
        }

        [Test]
        public void LoadsBuildServersFromYaml()
        {
            var core = new BurroCore();
            core.Initialise("Config\\buildservers.yml");
            Assert.IsNotNull(core.BuildServers);
            Assert.AreEqual(2, core.BuildServers.Count());
            Assert.IsAssignableFrom<GoServer>(core.BuildServers.First());
            Assert.IsAssignableFrom<CCServer>(core.BuildServers.ElementAt(1));
        }

        [Test]
        public void ParsesGoServerCorrectly()
        {
            var core = new BurroCore();
            core.Initialise("Config\\goserver.yml");
            Assert.IsNotNull(core.BuildServers);
            Assert.AreEqual(1, core.BuildServers.Count());

            var goServer = core.BuildServers.First() as GoServer;
            Assert.IsNotNull(goServer);
            Assert.AreEqual("http://goserver.localdomain:8153/go", goServer.URL);
            Assert.AreEqual("ci", goServer.Username);
            Assert.AreEqual("secret", goServer.Password);
            Assert.IsNotNull(goServer.Pipelines);
            Assert.AreEqual(2, goServer.Pipelines.Count());
        }
    }
}
