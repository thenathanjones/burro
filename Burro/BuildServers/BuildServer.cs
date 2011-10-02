using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burro.BuildServers
{
    public abstract class BuildServer : IBuildServer
    {
        internal void OnPipelinesUpdated()
        {
            if (PipelinesUpdated != null) PipelinesUpdated(PipelineReports);
        }

        public IEnumerable<PipelineReport> PipelineReports { get; internal set; }

        public event Action<IEnumerable<PipelineReport>> PipelinesUpdated;

        public abstract void StartMonitoring();

        public abstract void StopMonitoring();
    }
}
