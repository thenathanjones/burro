using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using Burro.Parsers;
using Burro.Util;
using Moq;
using NUnit.Framework;
using Ninject;

namespace Burro.Tests.BuildServers
{
    [TestFixture]
    public class GoServerTest
    {
        private IKernel _kernel;

        [SetUp]
        public void Setup()
        {
            _kernel = new StandardKernel();
        }

        [Test]
        public void StartsTimer()
        {
            var timer = new Mock<ITimer>();
            var parser = new Mock<IParser>();
            _kernel.Bind<ITimer>().ToConstant(timer.Object);
            _kernel.Bind<IParser>().ToConstant(parser.Object).Named("Go");
            var goServer = _kernel.Get<GoServer>();
            timer.Verify(t => t.Start(), Times.Never());
            goServer.StartMonitoring();
            timer.Verify(t => t.Start(), Times.Once());
        }
    }
}
