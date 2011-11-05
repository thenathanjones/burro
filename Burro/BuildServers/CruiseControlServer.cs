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
    public class CruiseControlServer : BuildServer
    {
        private readonly ITimer _timer;
        private readonly IParser _parser;

        [Inject]
        public CruiseControlServer(ITimer timer, [Named("CruiseControl")] IParser parser)
        {
            _timer = timer;
            _parser = parser;
        }

        public void Initialise(IConfig config)
        {
            Config = config;

            _parser.Initialise(Config);
            _timer.Tick += () =>
                               {
                                   PipelineReports = _parser.GetPipelines();
                                   OnPipelinesUpdated();
                               };
        }

        public override void StartMonitoring()
        {
            _timer.Start();
        }

        public override void StopMonitoring()
        {
            _timer.Stop();
        }
    }
}
