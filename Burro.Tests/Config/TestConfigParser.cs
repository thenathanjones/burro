using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Burro.BuildServers;
using Ninject;
using YamlDotNet.RepresentationModel;

namespace Burro.Config
{
    public class TestConfigParser : IConfigParser
    {
        private IKernel _kernel;

        [Inject]
        public TestConfigParser(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IBuildServer Parse(YamlMappingNode config)
        {
            return _kernel.Get<IBuildServer>("TestBuildServer");
        }
    }
}
