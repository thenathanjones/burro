using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.Config;

namespace Burro.BuildServers
{
    public interface IBuildServer
    {
        IEnumerable<PipelineReport> PipelineReports { get; }

        IConfig Config { get; }

        event Action<IEnumerable<PipelineReport>> PipelinesUpdated;

        void StartMonitoring();

        void StopMonitoring();
    }
}
