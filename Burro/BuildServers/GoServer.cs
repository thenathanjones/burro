﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burro.BuildServers
{
    public class GoServer : BuildServer
    {
        public string URL { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}