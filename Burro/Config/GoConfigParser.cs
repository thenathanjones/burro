using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using Burro.Util;
using Ninject;
using YamlDotNet.RepresentationModel;

namespace Burro.Config
{
    public class GoConfigParser : IConfigParser
    {
        private readonly IKernel _kernel;

        [Inject]
        public GoConfigParser(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IBuildServer Parse(YamlMappingNode config)
        {
            var parser = _kernel.Get<GoServer>();
            parser.URL = BurroUtils.ExtractValue(config, "url");
            parser.Username = BurroUtils.ExtractValue(config, "username");
            parser.Password = BurroUtils.ExtractValue(config, "password");
            parser.Pipelines = ParsePipelines(config);

            return parser;
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
