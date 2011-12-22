using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro.BuildServers;
using Moq;
using Ninject;
using YamlDotNet.RepresentationModel;

// ReSharper disable CheckNamespace
// Required for dynamic loading of class
namespace Burro.Config.Parsers
// ReSharper restore CheckNamespace
{
    public class TestConfigParser : IConfigParser
    {
        private IKernel _kernel;

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
