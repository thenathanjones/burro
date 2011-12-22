using System.Collections.Generic;

namespace Burro.Config
{
    public interface IConfig
    {
        string Type { get; }

        string URL { get; set; }

        string Username { get; set; }

        string Password { get; set; }

        IEnumerable<string> Pipelines { get; set; }
    }
}