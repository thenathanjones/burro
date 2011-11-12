using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using NUnit.Framework;
using Burro.Util;
using Burro.Parsers;
using Moq;

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

        //[Test]
        //public void RaisesErrorIfProblemParsing()
        //{
        //    var timer = new Mock<ITimer>();
        //    var parser = new Mock<IParser>();

        //    parser.Setup(p => p.GetPipelines()).Throws(new Exception());

        //    var updated = false;
        //    var testServer = new TestBuildServer(timer.Object, parser.Object);
        //    testS

        //    timer.Raise(t => t.Tick += null);

        //    Assert.IsTrue(updated);
        //}

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
