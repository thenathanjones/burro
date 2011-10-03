using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Burro.BuildServers;
using Ninject;
using YamlDotNet.RepresentationModel;

// ReSharper disable CheckNamespace
namespace Burro.Config // Needs this namespace to resolve correctly
// ReSharper restore CheckNamespace
{
    public class TestConfigParser : IConfigParser
    {
        private readonly IKernel _kernel;

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
