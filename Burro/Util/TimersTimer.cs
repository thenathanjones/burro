using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Burro.Util
{
    public class TimersTimer : ITimer
    {
        private readonly Timer _timer;
        public event Action Tick;

        public TimersTimer(TimeSpan interval)
        {
            _timer = new Timer(interval.TotalMilliseconds);
            _timer.Elapsed += (s, e) => { if (Tick != null) Tick(); };
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
