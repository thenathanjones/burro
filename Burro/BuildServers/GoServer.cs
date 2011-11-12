using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.Config;
using Burro.Parsers;
using Burro.Util;
using Ninject;

namespace Burro.BuildServers
{
    public class GoServer : PollingBuildServer
    {
        [Inject]
        public GoServer(ITimer timer, [Named("Go")] IParser parser)
        {
            _timer = timer;
            _parser = parser;
        }
    }
}
