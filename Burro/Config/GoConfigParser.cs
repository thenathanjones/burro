using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using Ninject;
using YamlDotNet.RepresentationModel;

namespace Burro.Config
{
    internal class GoConfigParser : IConfigParser
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
            parser.URL = BurroCore.ExtractValue(config, "url");
            parser.Username = BurroCore.ExtractValue(config, "username");
            parser.Password = BurroCore.ExtractValue(config, "password");
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
