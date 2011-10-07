﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using Burro.Config;
using Burro.Config.Parsers;
using Burro.Util;
using Ninject;
using Ninject.Parameters;
using YamlDotNet.RepresentationModel;

namespace Burro
{
    public class BurroCore
    {
        [Inject]
        public BurroCore(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Initialise(string pathToConfig)
        {
            LoadConfig(pathToConfig);
            ParseConfig();
        }

        private void ParseConfig()
        {
            BuildServers = RawConfig.Children.Select(ParseBuildServer);
        }

        private IBuildServer ParseBuildServer(YamlNode yamlNode)
        {
            var config = yamlNode as YamlMappingNode;

            var serverType = BurroUtils.ExtractValue(config, "servertype");
            var typeName = "Burro.Config.Parsers." + serverType + "ConfigParser";
            var parserType = BurroUtils.GetTypeFromName(typeName);

            return ((IConfigParser) _kernel.Get(parserType)).Parse(config);
        }

        private void LoadConfig(string pathToConfig)
        {
            StreamReader configFile = null;
            try
            {
                configFile = new StreamReader(pathToConfig);
                var configStream = new YamlStream();
                configStream.Load(configFile);

                RawConfig = configStream.Documents.First().RootNode as YamlSequenceNode;
            }
            finally
            {
                if (configFile != null) configFile.Close();
            }
        }

        public YamlSequenceNode RawConfig { get; private set; }

        public IEnumerable<IBuildServer> BuildServers { get; private set; }

        public void StartMonitoring()
        {
            foreach (var buildServer in BuildServers)
            {
                buildServer.StartMonitoring();
            }
        }

        private readonly IKernel _kernel;

        public void StopMonitoring()
        {
            foreach (var buildServer in BuildServers)
            {
                buildServer.StopMonitoring();
            }
        }
    }
}
