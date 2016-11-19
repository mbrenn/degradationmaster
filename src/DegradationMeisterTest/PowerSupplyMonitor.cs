using DegradationMeister;
using DegradationMeister.Impl;

namespace DegradationMeisterTest
{
    public class PowerSupplyMonitor : Monitor
    {
        public IFailure TotalFailure { get; } = new Failure("TotalFailure");

        public IFailure LimitedCurrent { get; } = new Failure("LimitedCurrent");
        
        public PowerSupplyMonitor(IDegrader degrader) : base(degrader)
        {
        }

        public void InjectTotalFailure()
        {
            ReportMonitoringResult(TotalFailure, MonitoringResult.NOK);
        }

        public void InjectLimitedCurrentFailure()
        {
            ReportMonitoringResult(LimitedCurrent, MonitoringResult.NOK);

        }

        public void MakeOk()
        {
            ReportMonitoringResult(TotalFailure, MonitoringResult.OK);
            ReportMonitoringResult(LimitedCurrent, MonitoringResult.OK);
        }
    }
}