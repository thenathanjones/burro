using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using Burro.Config;
using Burro.Config.Parsers;
using Burro.Util;
using Ninject;
using YamlDotNet.RepresentationModel;
using Burro.Parsers;

namespace Burro
{
    public class BurroCore : IBurroCore
    {
        [Inject]
        public BurroCore(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Initialise(string pathToConfig)
        {
            ConfigureBindings();
            LoadConfig(pathToConfig);
            ParseConfig();
        }

        private void ConfigureBindings()
        {
            var parserBindings = _kernel.GetBindings(typeof(IParser));

            if (!parserBindings.Any(p => p.Metadata.Name == "Go"))
            {
                _kernel.Bind<IParser>().To<CruiseControlParser>().Named("Go");
            }

            if (!parserBindings.Any(p => p.Metadata.Name == "CruiseControl"))
            {
                _kernel.Bind<IParser>().To<CruiseControlParser>().Named("CruiseControl");
            }

            if (!_kernel.GetBindings(typeof(IWebRequest)).Any())
            {
                _kernel.Bind<IWebRequest>().To<WebRequestAdapter>();
            }
        }

        private void ParseConfig()
        {
            if (RawConfig.Children.Any(c => c.ToString() != ""))
            {
                BuildServers = RawConfig.Children.Select(ParseBuildServer).ToArray();
            }
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
