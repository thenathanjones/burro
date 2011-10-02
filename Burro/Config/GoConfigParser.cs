using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using YamlDotNet.RepresentationModel;

namespace Burro.Config
{
    internal class GoConfigParser : IConfigParser
    {
        public IBuildServer Parse(YamlMappingNode config)
        {
            return new GoServer()
            {
                URL = BurroCore.ExtractValue(config, "url"),
                Username = BurroCore.ExtractValue(config, "username"),
                Password = BurroCore.ExtractValue(config, "password"),
                Pipelines = ParsePipelines(config)
            };
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
