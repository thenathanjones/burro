using Burro.BuildServers;
using YamlDotNet.RepresentationModel;

namespace Burro.Config.Parsers
{
    public class CruiseControlConfigParser : IConfigParser
    {
        public IBuildServer Parse(YamlMappingNode config)
        {
            return new CruiseControlServer();
        }
    }
}
