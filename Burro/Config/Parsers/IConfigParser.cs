using Burro.BuildServers;
using YamlDotNet.RepresentationModel;

namespace Burro.Config.Parsers
{
    public interface IConfigParser
    {
        IBuildServer Parse(YamlMappingNode config);
    }
}
