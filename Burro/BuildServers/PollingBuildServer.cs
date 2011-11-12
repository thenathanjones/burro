using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.Util;
using Burro.Parsers;
using Burro.Config;

namespace Burro.BuildServers
{
    public abstract class PollingBuildServer : BuildServer
    {
        protected ITimer _timer;
        protected IParser _parser;

        public void Initialise(IConfig config)
        {
            Config = config;

            _parser.Initialise(Config);
            _timer.Tick += () =>
            {
                try
                {
                    PipelineReports = _parser.GetPipelines();
                }
                catch (Exception ex)
                {
                    OnErrorParsing(ex);
                }
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
