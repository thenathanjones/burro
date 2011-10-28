using System;
using YamlDotNet.RepresentationModel;
using System.Collections.Generic;
using Burro.BuildServers;
namespace Burro
{
    interface IBurroCore
    {
        IEnumerable<IBuildServer> BuildServers { get; }
        void Initialise(string pathToConfig);
        YamlSequenceNode RawConfig { get; }
        void StartMonitoring();
        void StopMonitoring();
    }
}
