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
            ReportMonitoringResult(TotalFailure, MonitoringResult.NOK);
        }

        public void MakeOk()
        {
            ReportMonitoringResult(TotalFailure, MonitoringResult.OK);
        }
    }
}
