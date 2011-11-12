using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using NUnit.Framework;
using Burro.Util;
using Burro.Parsers;
using Moq;
using Burro.Config;

namespace Burro.Tests.BuildServers
{
    [TestFixture]
    public class PollingBuildServerTest
    {
        [Test]
        public void StartsTimer()
        {
            var timer = new Mock<ITimer>();
            var parser = new Mock<IParser>();
            var testServer = new TestBuildServer(timer.Object, parser.Object);
            timer.Verify(t => t.Start(), Times.Never());
            testServer.StartMonitoring();
            timer.Verify(t => t.Start(), Times.Once());
        }

        [Test]
        public void RaisesErrorIfProblemParsing()
        {
            var timer = new Mock<ITimer>();
            var parser = new Mock<IParser>();
            var config = new Mock<IConfig>();

            var testException = new Exception();
            parser.Setup(p => p.GetPipelines()).Throws(testException);

            var testServer = new TestBuildServer(timer.Object, parser.Object);
            testServer.Initialise(config.Object);

            var errorThrown = false;
            testServer.ErrorParsing += (thrownException) =>
            {
                Assert.AreSame(testException, thrownException);
                errorThrown = true;
            };

            timer.Raise(t => t.Tick += null);

            Assert.IsTrue(errorThrown);
        }

        private class TestBuildServer : PollingBuildServer
        {
            public TestBuildServer(ITimer timer, IParser parser)
            {
                _timer = timer;
                _parser = parser;
            }
        }
    }
}
