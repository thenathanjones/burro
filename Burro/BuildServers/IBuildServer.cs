using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burro.BuildServers
{
    public interface IBuildServer
    {
        IEnumerable<PipelineReport> PipelineReports { get; }

        event Action<IEnumerable<PipelineReport>> PipelinesUpdated;

        void StartMonitoring();

        void StopMonitoring();
    }
}
