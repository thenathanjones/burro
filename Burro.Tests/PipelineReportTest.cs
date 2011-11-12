using System;
using System.Linq;
using Burro.Parsers;
using NUnit.Framework;

namespace Burro.Tests
{
    [TestFixture]
    public class PipelineReportTest
    {
        [Test]
        public void HasAName()
        {
            var pipeline = new PipelineReport() {Name = "Tijuana"};
            Assert.AreEqual("Tijuana", pipeline.Name);
        }

        [Test]
        public void HasALastBuildState()
        {
            var pipeline = new PipelineReport() { BuildState = BuildState.Success };
            Assert.AreEqual(BuildState.Success, pipeline.BuildState);
        }

        [Test]
        public void LastBuildHasThreeStatesIncludingUnknown()
        {
            Assert.AreEqual(3, Enum.GetValues(typeof (BuildState)).Length);
            Assert.IsTrue(new [] {BuildState.Success, BuildState.Failure, BuildState.Unknown}.All(state => Enum.GetValues(typeof(BuildState)).Cast<BuildState>().Contains(state)));
        }

        [Test]
        public void HasALastBuildTime()
        {
            var lastBuildTime = DateTime.Now;
            var pipeline = new PipelineReport() {LastBuildTime = lastBuildTime};
            Assert.AreEqual(lastBuildTime, pipeline.LastBuildTime);
        }

        [Test]
        public void DefaultActivityIsUnknown()
        {
            var pipeline = new PipelineReport();
            Assert.AreEqual(Activity.Unknown, pipeline.Activity);
        }

        [Test]
        public void DefaultStateIsUnknown()
        {
            var pipeline = new PipelineReport();   
            Assert.AreEqual(BuildState.Unknown, pipeline.BuildState);
        }

        [Test]
        public void ActivityHasFourStatesIncludingUnknown()
        {
            Assert.AreEqual(4, Enum.GetValues(typeof(Activity)).Length);
            Assert.IsTrue(new[] { Activity.Busy, Activity.Idle, Activity.Unknown, Activity.Paused }.All(state => Enum.GetValues(typeof(Activity)).Cast<Activity>().Contains(state)));
        }
    }
}
