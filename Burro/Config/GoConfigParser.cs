using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using YamlDotNet.RepresentationModel;

namespace Burro.Config
{
    internal static class GoConfigParser
    {
        public static BuildServer Parse(YamlMappingNode config)
        {
            return new GoServer()
            {
                URL = BurroCore.ExtractValue(config, "url"),
                Username = BurroCore.ExtractValue(config, "username"),
                Password = BurroCore.ExtractValue(config, "password")
            };
        }
    }
}
