using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using YamlDotNet.RepresentationModel;

namespace Burro.Config
{
    public interface IConfigParser
    {
        IBuildServer Parse(YamlMappingNode config);
    }
}
