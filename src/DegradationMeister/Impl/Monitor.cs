using System;

namespace DegradationMeister.Impl
{
    public class Monitor
    {
        /// <summary>
        /// Gets or sets the degrader being invoked in case of an update
        /// </summary>
        public IDegrader Degrader { get; set; }

        public Monitor(IDegrader degrader)
        {
            Degrader = degrader;
        }

        protected void ReportFailureStatus(IFailure failure, MonitoringResult result)
        {
            if (Degrader == null)
            {
                throw new InvalidOperationException("Degrader is not set");
            }

            Degrader.UpdateMonitoringResult(failure, result);
        }
    }
}