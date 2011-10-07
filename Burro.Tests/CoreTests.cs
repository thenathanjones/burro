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
            _kernel.Bind<IBuildServer>().ToConstant(_testServer.Object).Named("TestBuildServer");
            var core = _kernel.Get<BurroCore>();
            Assert.IsNull(core.BuildServers);
            core.Initialise("Config\\testbuildservers.yml");
            Assert.AreEqual(2, core.BuildServers.Count());
        }

        [Test]
        public void StartCallsStartOnBuildServers()
        {
            _kernel.Bind<IBuildServer>().ToConstant(_testServer.Object).Named("TestBuildServer");
            var core = _kernel.Get<BurroCore>();
            core.Initialise("Config\\startmonitoring.yml");

            _testServer.Verify(ts => ts.StartMonitoring(), Times.Never());
            core.StartMonitoring();
            _testServer.Verify(ts => ts.StartMonitoring(), Times.Once());
        }

        [Test]
        public void StopCallsStopOnBuildServers()
        {
            _kernel.Bind<IBuildServer>().ToConstant(_testServer.Object).Named("TestBuildServer");
            var core = _kernel.Get<BurroCore>();
            core.Initialise("Config\\startmonitoring.yml");

            _testServer.Verify(ts => ts.StopMonitoring(), Times.Never());
            core.StopMonitoring();
            _testServer.Verify(ts => ts.StopMonitoring(), Times.Once());
        }
    }
}
