﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burro.BuildServers
{
    public abstract class BuildServer
    {
        public IEnumerable<PipelineReport> Pipelines { get; internal set; } 

        public event Action<IEnumerable<PipelineReport>> PipelinesUpdated;

        internal void OnPipelinesUpdated()
        {
            if (PipelinesUpdated != null) PipelinesUpdated(Pipelines);
        }
    }
}
