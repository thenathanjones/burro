using System.Collections.Generic;
using System.Xml.Linq;

namespace Burro.Parsers
{
    public interface ICCParser
    {
        IEnumerable<PipelineReport> Parse(XDocument sampleDocument);
    }
}
