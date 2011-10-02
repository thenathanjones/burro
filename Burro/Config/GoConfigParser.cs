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
                Password = BurroCore.ExtractValue(config, "password"),
                Pipelines = ParsePipelines(config)
            };
        }

        private static IEnumerable<string> ParsePipelines(YamlMappingNode config)
        {
            var pipelines = config.Children[new YamlScalarNode("pipelines")] as YamlSequenceNode;

            return pipelines == null ? new List<string>() : pipelines.Children.Select(c => ParsePipeline(c as YamlMappingNode));
        }

        private static string ParsePipeline(YamlMappingNode yamlMappingNode)
        {
            return yamlMappingNode.Children[new YamlScalarNode("name")].ToString();
        }
    }
}
