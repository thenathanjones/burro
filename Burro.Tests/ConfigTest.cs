using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using YamlDotNet.RepresentationModel;

namespace Burro.Tests
{
    [TestFixture]
    public class ConfigTest
    {
        [Test]
        public void LoadsConfigAsYaml()
        {
            var core = new BurroCore();
            core.Initialise("Config\\blank.yml");
            Assert.IsNotNull(core.RawConfig);
        }

        [Test]
        public void LoadsBuildServersFromYaml()
        {
            var core = new BurroCore();
            core.Initialise("Config\\buildservers.yml");
            Assert.IsNotNull(core.BuildServers);
            Assert.AreEqual(2, core.BuildServers.Count());
            Assert.IsAssignableFrom<GoBuildServer>(core.BuildServers.First());
            Assert.IsAssignableFrom<CruiseControlBuildServer>(core.BuildServers.ElementAt(1));
        }

        [Test]
        public void ParsesGoServerCorrectly()
        {
            var core = new BurroCore();
            core.Initialise("Config\\goserver.yml");
            Assert.IsNotNull(core.BuildServers);
            Assert.AreEqual(1, core.BuildServers.Count());
            Assert.IsAssignableFrom<GoBuildServer>(core.BuildServers.First());
        }
    }

    public class CruiseControlBuildServer : BuildServer
    {
    }

    public class GoBuildServer : BuildServer
    {

    }

    public class BurroCore
    {
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

            var serverType = buildServer.Children.First(s => s.Key.ToString() == "servertype").Value.ToString();
            switch (serverType)
            {
                case "Go":
                    return new GoBuildServer();
                case "CruiseControl":
                    return new CruiseControlBuildServer();
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
    }

    public abstract class BuildServer
    {
    }
}
