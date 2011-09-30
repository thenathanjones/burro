using System;
using System.Linq;
using NUnit.Framework;

namespace Burro.Tests
{
    [TestFixture]
    public class PipelineTest
    {
        [Test]
        public void HasAName()
        {
            var pipeline = new Pipeline() {Name = "Tijuana"};
            Assert.AreEqual("Tijuana", pipeline.Name);
        }

        [Test]
        public void HasALastBuildState()
        {
            var pipeline = new Pipeline() { BuildState = BuildState.Success };
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
            var pipeline = new Pipeline() {LastBuildTime = lastBuildTime};
            Assert.AreEqual(lastBuildTime, pipeline.LastBuildTime);
        }

        [Test]
        public void DefaultActivityIsUnknown()
        {
            var pipeline = new Pipeline();
            Assert.AreEqual(Activity.Unknown, pipeline.Activity);
        }

        [Test]
        public void DefaultStateIsUnknown()
        {
            var pipeline = new Pipeline();   
            Assert.AreEqual(BuildState.Unknown, pipeline.BuildState);
        }
    }
}
