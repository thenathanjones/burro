using System.Collections.Generic;
using Burro.Config;

namespace Burro.Parsers
{
    public interface IParser
    {
        void Initialise(IConfig config);
        IEnumerable<PipelineReport> GetPipelines();
    }
}