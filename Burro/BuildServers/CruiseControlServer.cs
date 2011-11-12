using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Burro.Util;
using Burro.Parsers;
using Burro.Config;

namespace Burro.BuildServers
{
    public class CruiseControlServer : PollingBuildServer
    {
        [Inject]
        public CruiseControlServer(ITimer timer, [Named("CruiseControl")] IParser parser)
        {
            _timer = timer;
            _parser = parser;
        }
    }
}
