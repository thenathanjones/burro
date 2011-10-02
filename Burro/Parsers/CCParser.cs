using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Burro.Parsers
{
    public class CCParser : ICCParser
    {
        public IEnumerable<Pipeline> Parse(XDocument sampleDocument)
        {
            var projects = sampleDocument.Descendants("Project");

            return projects.Select(p => new Pipeline()
            {
                Name = p.Attribute("name").Value,
                Activity = ParseActivity(p.Attribute("activity").Value),
                BuildState = ParseState(p.Attribute("lastBuildStatus").Value),
                LastBuildTime = ParseLastBuildTime(p.Attribute("lastBuildTime").Value),
                LinkURL = p.Attribute("webUrl").Value
            });
        }

        private DateTime ParseLastBuildTime(string value)
        {
            return DateTime.Parse(value);
        }

        private BuildState ParseState(string value)
        {
            var projectState = BuildState.Unknown;
            switch (value)
            {
                case "Success":
                    projectState = BuildState.Success;
                    break;
                case "Failure":
                    projectState = BuildState.Failure;
                    break;
            }

            return projectState;
        }

        private Activity ParseActivity(string value)
        {
            var projectActivity = Activity.Unknown;
            switch (value)
            {
                case "Sleeping":
                    projectActivity = Activity.Idle;
                    break;
                case "Building":
                    projectActivity = Activity.Busy;
                    break;
            }

            return projectActivity;
        }

        public XDocument LoadStream(Stream inputStream)
        {
            return XDocument.Load(inputStream);
        }
    }

    public enum Activity
    {
        Unknown,
        Idle,
        Busy
    }
}
