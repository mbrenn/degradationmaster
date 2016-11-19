using System;

namespace DegradationMeister.Impl
{
    public class Monitor
    {
        /// <summary>
        /// Gets or sets the degrader being invoked in case of an update
        /// </summary>
        private IDegrader Degrader { get; set; }

        public Monitor(IDegrader degrader)
        {
            Degrader = degrader;
        }

        /// <summary>
        /// This method can be called by the inherited methods which define a specific failure
        /// </summary>
        /// <param name="failure">Failure, whose status shall be updated</param>
        /// <param name="result">The monitoring result of the failure</param>
        protected void ReportMonitoringResult(IFailure failure, MonitoringResult result)
        {
            if (Degrader == null)
            {
                throw new InvalidOperationException("Degrader is not set");
            }

            Degrader.UpdateMonitoringResult(failure, result);
        }
    }
}