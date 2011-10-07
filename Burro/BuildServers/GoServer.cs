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
    public class GoServer : BuildServer
    {
        private readonly ITimer _timer;
        private readonly IParser _parser;

        [Inject]
        public GoServer(ITimer timer, IParser parser)
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
