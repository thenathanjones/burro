using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using Moq;
using NUnit.Framework;
using Ninject;

namespace Burro.Tests
{
    [TestFixture]
    public class CoreTests
    {
        private IKernel _kernel;
        private Mock<IBuildServer> _testServer;

        [SetUp]
        public void Setup()
        {
            _testServer = new Mock<IBuildServer>();
            _kernel = new StandardKernel();
        }

        [Test]
        public void InitialisationLoadsBuildServers()
        {
            var core = _kernel.Get<BurroCore>();
            Assert.IsNull(core.BuildServers);
            core.Initialise("Config\\buildservers.yml");
            Assert.AreEqual(2, core.BuildServers.Count());
        }

        [Test]
        public void StartCallsStartOnAllBuildServers()
        {
            var core = _kernel.Get<BurroCore>();
            core.Initialise("Config\\buildservers.yml");

            core.StartMonitoring();
        }
    }
}
