using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using Burro.Config;
using YamlDotNet.RepresentationModel;

namespace Burro
{
    public class BurroCore
    {
        public static string ExtractValue(YamlMappingNode config, string key)
        {
            return config.Children[new YamlScalarNode(key)].ToString();
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

        private BuildServer ParseBuildServer(YamlNode yamlNode)
        {
            var buildServer = yamlNode as YamlMappingNode;

            var serverType = ExtractValue(buildServer, "servertype");
            switch (serverType)
            {
                case "Go":
                    return GoConfigParser.Parse(buildServer);
                case "CruiseControl":
                    return new CCServer();
                default:
                    throw new ArgumentException(serverType + " is not a valid build server type");
            }
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

        public IEnumerable<BuildServer> BuildServers { get; private set; }

        public void StartMonitoring()
        {
            foreach (var buildServer in BuildServers)
            {
                buildServer.StartMonitoring();
            }
        }
    }
}
