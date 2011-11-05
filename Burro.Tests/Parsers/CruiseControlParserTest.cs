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
    public class CruiseControlParserTest
    {
        private XDocument _testDocument;
        private XElement _testProjects;
        private XElement _testProject;

        private IKernel _kernel;

        private Mock<IWebRequest> _webRequest;
        private CruiseControlServerConfig _config;

        [SetUp]
        public void CreateDocument()
        {
            _kernel = new StandardKernel();
            _webRequest = new Mock<IWebRequest>();
            _kernel.Bind<IWebRequest>().ToConstant(_webRequest.Object);

            _testDocument = new XDocument();    
            _testProjects = new XElement("Projects");
            _testDocument.Add(_testProjects);

            _testProject = new XElement("Project");
            _testProject.SetAttributeValue("name", "CI-ProductA :: Build");
            _testProject.SetAttributeValue("activity", "Sleeping");
            _testProject.SetAttributeValue("lastBuildStatus", "Success");
            _testProject.SetAttributeValue("lastBuildLabel", "61");
            _testProject.SetAttributeValue("lastBuildTime", "2011-09-16T01:34:36");
            _testProject.SetAttributeValue("webUrl", "http://goserver.localdomain:8153/go/pipelines/CI-ProductA/61/Build/1");

            var stream = new MemoryStream();
            _testDocument.Save(stream);
            _webRequest.Setup(wr => wr.GetResponseStream()).Returns(stream);

            _config = new CruiseControlServerConfig() { URL = "blah", Pipelines = (new string[] { "CI-ProductA :: Build" }).AsEnumerable<string>() };
        }

        [Test]
        public void ReadsStreamAsXML()
        {
            var testInput = File.Open("Parsers\\testcc.xml", FileMode.Open);

            var parser = _kernel.Get<CruiseControlParser>();
            var loadedDocument = parser.LoadStream(testInput);

            Assert.IsNotNull(loadedDocument);
            Assert.IsInstanceOf<XDocument>(loadedDocument);
        }

        [Test]
        public void ExtractPipelines()
        {
            for (var i=0;i<3;i++)
            {
                _testProjects.Add(_testProject);
            }

            var parser = _kernel.Get<CruiseControlParser>();
            parser.Initialise(_config);
            var pipelines = parser.Parse(_testDocument);

            Assert.AreEqual(3, pipelines.Count());
        }

        [Test]
        public void ParsesNameCorrectly()
        {
            _testProjects.Add(_testProject);

            var parser = _kernel.Get<CruiseControlParser>();
            parser.Initialise(_config);
            var pipeline = parser.Parse(_testDocument).First();

            Assert.AreEqual("CI-ProductA :: Build", pipeline.Name);
        }

        [Test]
        public void ParsesActivityCorrectly()
        {
            _testProjects.Add(_testProject);
            var parser = _kernel.Get<CruiseControlParser>();
            parser.Initialise(_config);

            _testProject.SetAttributeValue("activity", "Sleeping");
            var pipeline = parser.Parse(_testDocument).First();
            Assert.AreEqual(Activity.Idle, pipeline.Activity);

            _testProject.SetAttributeValue("activity", "Building");
            pipeline = parser.Parse(_testDocument).First();
            Assert.AreEqual(Activity.Busy, pipeline.Activity);
        }

        [Test]
        public void ParsesLastBuildStatusCorrectly()
        {
            _testProjects.Add(_testProject);
            var parser = _kernel.Get<CruiseControlParser>();
            parser.Initialise(_config);
            
            _testProject.SetAttributeValue("lastBuildStatus", "Success");
            var pipeline = parser.Parse(_testDocument).First();
            Assert.AreEqual(BuildState.Success, pipeline.BuildState);

            _testProject.SetAttributeValue("lastBuildStatus", "Failure");
            pipeline = parser.Parse(_testDocument).First();
            Assert.AreEqual(BuildState.Failure, pipeline.BuildState);
        }

        [Test]
        public void ParsesLinkURLCorrectly()
        {
            _testProjects.Add(_testProject);
            var parser = _kernel.Get<CruiseControlParser>();
            parser.Initialise(_config);

            var pipeline = parser.Parse(_testDocument).First();
            Assert.AreEqual("http://goserver.localdomain:8153/go/pipelines/CI-ProductA/61/Build/1", pipeline.LinkURL);
        }

        [Test]
        public void ParsesTimeDateCorrectly()
        {
            _testProjects.Add(_testProject);
            var parser = _kernel.Get<CruiseControlParser>();
            parser.Initialise(_config);

            var pipeline = parser.Parse(_testDocument).First();

            Assert.AreEqual(new DateTime(2011, 9, 16, 1, 34, 36), pipeline.LastBuildTime);
        }

        [Test]
        public void OnlyReturnsMatchingPipelines()
        {
            _testProjects.Add(_testProject);
            var parser = _kernel.Get<CruiseControlParser>();
            parser.Initialise(new CruiseControlServerConfig() { URL = "blah", Pipelines = new List<string>() });

            var pipelines = parser.Parse(_testDocument);

            Assert.AreEqual(0, pipelines.Count());
        }
    }

    
}
