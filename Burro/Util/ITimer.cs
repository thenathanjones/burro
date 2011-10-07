using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burro.Util
{
    public interface ITimer
    {
        /// <summary>
        /// Action that happens when the timer elapses
        /// </summary>
        event Action Tick;

        /// <summary>
        /// Start the timer
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the timer
        /// </summary>
        void Stop();
    }
}
