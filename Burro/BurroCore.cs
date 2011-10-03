using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using Burro.Config;
using Ninject;
using Ninject.Parameters;
using YamlDotNet.RepresentationModel;

namespace Burro
{
    public class BurroCore
    {
        public static string ExtractValue(YamlMappingNode config, string key)
        {
            return config.Children[new YamlScalarNode(key)].ToString();
        }

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

            var serverType = ExtractValue(config, "servertype");
            var typeName = "Burro.Config." + serverType + "ConfigParser";
            var parserType = GetTypeFromName(typeName);

            return ((IConfigParser) _kernel.Get(parserType)).Parse(config);
        }

        private Type GetTypeFromName(string typeName)
        {
            return AppDomain.CurrentDomain.GetAssemblies().AsEnumerable().SelectMany(a => a.GetTypes()).Single(t => t.FullName == typeName);
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
