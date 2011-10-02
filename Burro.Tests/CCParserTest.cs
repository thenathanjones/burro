using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Burro.Parsers;
using NUnit.Framework;

namespace Burro.Tests
{
    [TestFixture]
    public class CCParserTest
    {
        private XDocument _testDocument;
        private XElement _testProjects;
        private XElement _testProject;

        [SetUp]
        public void CreateDocument()
        {
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
        }

        [Test]
        public void ReadsStreamAsXML()
        {
            var testInput = File.Open("testcc.xml", FileMode.Open);

            var parser = new CCParser();
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

            var parser = new CCParser();
            var pipelines = parser.Parse(_testDocument);

            Assert.AreEqual(3, pipelines.Count());
        }

        [Test]
        public void ParsesNameCorrectly()
        {
            _testProjects.Add(_testProject);

            _testProject.SetAttributeValue("name", "TestProject");

            var parser = new CCParser();
            var pipeline = parser.Parse(_testDocument).First();

            Assert.AreEqual("TestProject", pipeline.Name);
        }

        [Test]
        public void ParsesActivityCorrectly()
        {
            _testProjects.Add(_testProject);
            var parser = new CCParser();

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
            var parser = new CCParser();
            
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
            var parser = new CCParser();

            var pipeline = parser.Parse(_testDocument).First();
            Assert.AreEqual("http://goserver.localdomain:8153/go/pipelines/CI-ProductA/61/Build/1", pipeline.LinkURL);
        }

        [Test]
        public void ParsesTimeDateCorrectly()
        {
            _testProjects.Add(_testProject);
            var parser = new CCParser();

            var pipeline = parser.Parse(_testDocument).First();

            Assert.AreEqual(new DateTime(2011, 9, 16, 1, 34, 36), pipeline.LastBuildTime);
        }
    }

    
}
