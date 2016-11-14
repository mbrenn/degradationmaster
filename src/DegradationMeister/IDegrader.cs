using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegradationMeister
{
    public interface IDegrader
    {
        void UpdateMonitoringResult(IFailure failure, MonitoringResult monitoringResult);

        /// <summary>
        /// Updates the degradation for the given capability
        /// </summary>
        /// <param name="capability">Capability which shall be upgraded</param>
        /// <param name="alreadyDegraded">Enumeration of the capabilities which were upgraded</param>
        void UpdateDegradation(ICapability capability, HashSet<ICapability> alreadyDegraded);

        void AddRule(ICapability sourceCapability, int sourceValue, ICapability targetCapability, int targetValue);

        void AddRule(IFailure sourceMonitor, MonitoringResult sourceValue, ICapability targetCapability, int targetValue);

        void AddTrigger(ICapability capability, Action<ICapability> function);
    }
}
