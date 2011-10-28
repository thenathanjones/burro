using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using Moq;
using NUnit.Framework;
using Ninject;
using Burro.Parsers;

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

        [Test]
        public void ConfiguresDefaultParsers()
        {
            _kernel.Bind<IBuildServer>().ToConstant(_testServer.Object).Named("TestBuildServer");
            var core = _kernel.Get<BurroCore>();
            core.Initialise("Config\\startmonitoring.yml");

            Assert.IsInstanceOf<CruiseControlParser>(_kernel.Get<IParser>("Go"));
        }

        [Test]
        public void AllowsOverrides()
        {
            var parser = new Mock<IParser>();
            _kernel.Bind<IParser>().ToConstant(parser.Object).Named("Go");

            _kernel.Bind<IBuildServer>().ToConstant(_testServer.Object).Named("TestBuildServer");
            var core = _kernel.Get<BurroCore>();
            core.Initialise("Config\\startmonitoring.yml");

            Assert.AreEqual(parser.Object, _kernel.Get<IParser>("Go"));
        }
    }
}
