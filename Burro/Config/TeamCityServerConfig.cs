using System.Collections.Generic;

namespace Burro.Config
{
    public class TeamCityServerConfig : IConfig
    {
        public string URL { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public IEnumerable<string> Pipelines { get; set; }
    }
}