using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Burro.Parsers;
using NUnit.Framework;
using Ninject;
using Burro.Util;
using Moq;
using Burro.Config;
using System.Collections.Generic;

namespace Burro.Tests.Parsers
{
    [TestFixture]
    public class TeamCityParserTest
    {
        private XDocument _testDocument;

        private IKernel _kernel;

        private Mock<IWebRequest> _webRequest;

        [SetUp]
        public void CreateDocument()
        {
            _kernel = new StandardKernel();
            _webRequest = new Mock<IWebRequest>();
            _kernel.Bind<IWebRequest>().ToConstant(_webRequest.Object);
        }

        [Test, Ignore]
        public void ReadsStreamAsXML()
        {
            
        }

        [Test, Ignore]
        public void ExtractPipelines()
        {
            
        }

        [Test, Ignore]
        public void ParsesNameCorrectly()
        {
            
        }

        [Test, Ignore]
        public void ParsesActivityCorrectly()
        {
            
        }

        [Test, Ignore]
        public void ParsesLastBuildStatusCorrectly()
        {
           
        }

        [Test, Ignore]
        public void ParsesLinkURLCorrectly()
        {
            
        }

        [Test, Ignore]
        public void ParsesTimeDateCorrectly()
        {
           
        }

        [Test, Ignore]
        public void OnlyReturnsMatchingPipelines()
        {
            
        }
    }
}
