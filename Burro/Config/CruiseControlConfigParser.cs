using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using YamlDotNet.RepresentationModel;

namespace Burro.Config
{
    public class CruiseControlConfigParser : IConfigParser
    {
        public IBuildServer Parse(YamlMappingNode config)
        {
            return new CruiseControlServer();
        }
    }
}
