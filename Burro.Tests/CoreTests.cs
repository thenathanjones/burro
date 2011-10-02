using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Burro.Tests
{
    [TestFixture]
    public class CoreTests
    {
        [Test]
        public void InitialisationLoadsBuildServers()
        {
            var core = new BurroCore();
            Assert.IsNull(core.BuildServers);
            core.Initialise("Config\\buildservers.yml");
            Assert.AreEqual(2, core.BuildServers.Count());
        }

        [Test]
        public void StartCallsStartOnAllBuildServers()
        {
            var core = new BurroCore();
            core.Initialise("Config\\buildservers.yml");

            core.StartMonitoring();
        }
    }
}
