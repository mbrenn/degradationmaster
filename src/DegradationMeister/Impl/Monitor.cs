using System;
using System.IO;

namespace DegradationMeister.Impl
{
    public class Monitor : IMonitor
    {
        public IDegrader Degrader { get; set; }

        public void InvokeFailure(IFailure failure, MonitoringResult result)
        {
            if (Degrader == null)
            {
                throw new InvalidOperationException("Degrader is not set");
            }

            Degrader.UpdateMonitoringResult(failure, result);
        }
    }
}