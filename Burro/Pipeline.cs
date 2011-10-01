﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burro
{
    public class Pipeline
    {
        public string Name { get; set; }

        public BuildState BuildState { get; set; }

        public DateTime LastBuildTime { get; set; }

        public Activity Activity { get; set; }

        public string LinkURL { get; set; }
    }

    public enum BuildState
    {
        Unknown,
        Success,
        Failure
    }
}