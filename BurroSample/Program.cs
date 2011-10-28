using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Burro;
using Burro.Parsers;
using Burro.Util;
using Ninject;

namespace BurroSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var burroSample = new BurroSampleApp();
        }
    }

    internal class BurroSampleApp
    {
        public BurroSampleApp()
        {
            const string configFile = "vagrant.yml";
            var kernel = new StandardKernel();

            var timer = new TimersTimer(new TimeSpan(0, 0, 5));
            kernel.Bind<ITimer>().ToConstant(timer);

            kernel.Bind<IParser>().To<CruiseControlParser>();

            var core = kernel.Get<BurroCore>();
            core.Initialise(configFile);

            core.BuildServers.First().PipelinesUpdated += (p) =>
            {
                Console.WriteLine("" + DateTime.Now + "-" + p.Count());
            };

            core.StartMonitoring();


            while (true)
            {
            }
        }
    }
}
