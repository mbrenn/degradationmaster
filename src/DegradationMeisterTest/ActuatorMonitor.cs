using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DegradationMeister;
using DegradationMeister.Impl;

namespace DegradationMeisterTest
{
    public class ActuatorMonitor : Monitor
    {
        public IFailure TotalFailure { get; set; } = new Failure();
    }
}
