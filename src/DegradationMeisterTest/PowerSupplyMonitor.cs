using DegradationMeister;
using DegradationMeister.Impl;

namespace DegradationMeisterTest
{
    public class PowerSupplyMonitor : Monitor
    {
        public IFailure TotalFailure { get; } = new Failure();

        public IFailure LimitedCurrent { get; } = new Failure();
        
        public PowerSupplyMonitor(IDegrader degrader) : base(degrader)
        {
        }

        public void InjectTotalFailure()
        {
            ReportFailureStatus(TotalFailure, MonitoringResult.NOK);
        }

        public void InjectLimitedCurrentFailure()
        {
            ReportFailureStatus(LimitedCurrent, MonitoringResult.NOK);

        }

        public void MakeOk()
        {
            ReportFailureStatus(TotalFailure, MonitoringResult.OK);
            ReportFailureStatus(LimitedCurrent, MonitoringResult.OK);
        }
    }
}