using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Burro.Config;

namespace Burro.Parsers
{
    public class CruiseControlParser : IParser
    {
        private IConfig _config;

        public void Initialise(IConfig config)
        {
            _config = config;
        }

        public IEnumerable<PipelineReport> GetPipelines()
        {
            var stream = GetStream(_config.URL, _config.Username, _config.Password);

            var streamDoc = LoadStream(stream);

            return Parse(streamDoc);
        }

        private Stream GetStream(string url, string username, string password)
        {
            var request = WebRequest.Create(url);
            
            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                request.Credentials = new NetworkCredential(username, password);
            }

            return request.GetResponse().GetResponseStream();
        }

        public IEnumerable<PipelineReport> Parse(XDocument sampleDocument)
        {
            var projects = sampleDocument.Descendants("Project");

            var observedProjects = projects.Where(p => _config.Pipelines.Contains(p.Attribute("name").Value));

            return observedProjects.Select(p => new PipelineReport()
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
