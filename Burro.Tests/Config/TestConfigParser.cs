using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Burro.BuildServers;
using YamlDotNet.RepresentationModel;

namespace Burro.Config
{
    public class TestConfigParser : IConfigParser
    {
        public IBuildServer Parse(YamlMappingNode config)
        {
            var serverMock = new Mock<IBuildServer>();

            return serverMock.Object;
        }
    }
}
