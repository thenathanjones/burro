using System.Collections.Generic;
using System.Xml.Linq;

namespace Burro.Parsers
{
    public interface ICruiseControlParser
    {
        IEnumerable<PipelineReport> Parse(XDocument sampleDocument);
    }
}
