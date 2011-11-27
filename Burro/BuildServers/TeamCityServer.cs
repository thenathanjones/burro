using Burro.Parsers;
using Burro.Util;
using Ninject;

namespace Burro.BuildServers
{
    public class TeamCityServer : PollingBuildServer
    {
        [Inject]
        public TeamCityServer(ITimer timer, [Named("TeamCity")] IParser parser)
        {
            _timer = timer;
            _parser = parser;
        }
    }
}