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
        public ActuatorMonitor(IDegrader degrader) : base(degrader)
        {
        }

        public IFailure TotalFailure { get; set; } = new Failure("TotalFailure");

        public void InjectTotalFailure()
        {
            ReportFailureStatus(TotalFailure, MonitoringResult.NOK);
        }

        public void MakeOk()
        {
            ReportFailureStatus(TotalFailure, MonitoringResult.OK);
        }
    }
}
