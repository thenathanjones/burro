using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace Burro.Util
{
    public static class BurroUtils
    {
        public static string ExtractValue(YamlMappingNode config, string key)
        {
            return config.Children[new YamlScalarNode(key)].ToString();
        }

        public static Type GetTypeFromName(string typeName)
        {
            return AppDomain.CurrentDomain.GetAssemblies().AsEnumerable().SelectMany(a => a.GetTypes()).Single(t => t.FullName == typeName);
        }
    }
}
