using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burro.Config
{
    public class GoServerConfig : IConfig
    {
        public string Type
        {
            get { return "Go"; }
        }

        public string URL { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public IEnumerable<string> Pipelines { get; set; }
    }
}
