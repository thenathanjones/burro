using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.Config;

namespace Burro.BuildServers
{
    public abstract class BuildServer : IBuildServer
    {
        internal void OnPipelinesUpdated()
        {
            if (PipelinesUpdated != null) PipelinesUpdated(PipelineReports);
        }

        internal void OnErrorParsing(Exception ex)
        {
            if (ErrorParsing != null) ErrorParsing(ex);
        }

        public IEnumerable<PipelineReport> PipelineReports { get; protected set; }

        public IConfig Config { get; protected set; }

        public event Action<IEnumerable<PipelineReport>> PipelinesUpdated;

        public event Action<Exception> ErrorParsing;

        public abstract void StartMonitoring();

        public abstract void StopMonitoring();
    }
}
