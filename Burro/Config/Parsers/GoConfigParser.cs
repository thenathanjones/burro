﻿using System.Collections.Generic;
using System.Linq;
using Burro.BuildServers;
using Burro.Util;
using Ninject;
using YamlDotNet.RepresentationModel;

namespace Burro.Config.Parsers
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
            var server = _kernel.Get<GoServer>();
            var serverConfig = new GoServerConfig()
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
