using Burro.BuildServers;
using YamlDotNet.RepresentationModel;
using Ninject;
using System.Collections.Generic;
using Burro.Util;
using System.Linq;

namespace Burro.Config.Parsers
{
    public class CruiseControlConfigParser : IConfigParser
    {
        private readonly IKernel _kernel;

        [Inject]
        public CruiseControlConfigParser(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IBuildServer Parse(YamlMappingNode config)
        {
            var server = _kernel.Get<CruiseControlServer>();
            var serverConfig = new CruiseControlServerConfig()
                                   {
                                       URL = BurroUtils.ExtractValue(config, "url"),
                                       Username = BurroUtils.ExtractValue(config, "username"),
                                       Password = BurroUtils.ExtractValue(config, "password"),
                                       Pipelines = ParsePipelines(config)
                                   };

            server.Initialise(serverConfig);

            return server;
        }

        private IEnumerable<string> ParsePipelines(YamlMappingNode config)
        {
            var pipelines = config.Children[new YamlScalarNode("pipelines")] as YamlSequenceNode;

            return pipelines == null ? new List<string>() : pipelines.Children.Select(c => ParsePipeline(c as YamlMappingNode));
        }

        private string ParsePipeline(YamlMappingNode yamlMappingNode)
        {
            return yamlMappingNode.Children[new YamlScalarNode("name")].ToString();
        }
    }
}
