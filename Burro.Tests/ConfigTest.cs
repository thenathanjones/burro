using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using Moq;
using NUnit.Framework;
using Ninject;

namespace Burro.Tests
{
    [TestFixture]
    public class ConfigTest
    {
        private IKernel _kernel;

        [SetUp]
        public void Setup()
        {
            _kernel = new StandardKernel();
        }

        [Test]
        public void LoadsConfigAsYaml()
        {
            var core = _kernel.Get<BurroCore>();
            core.Initialise("Config\\blank.yml");
            Assert.IsNotNull(core.RawConfig);
        }

        [Test]
        public void LoadsBuildServersFromYaml()
        {
            var core = _kernel.Get<BurroCore>();
            core.Initialise("Config\\buildservers.yml");
            Assert.IsNotNull(core.BuildServers);
            Assert.AreEqual(2, core.BuildServers.Count());
            Assert.IsInstanceOf<GoServer>(core.BuildServers.First());
            Assert.IsInstanceOf<CruiseControlServer>(core.BuildServers.ElementAt(1));
        }
    }
}
