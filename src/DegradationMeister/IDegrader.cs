using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegradationMeister
{
    public interface IDegrader
    {
        void UpdateMonitoringResult(IFailure failure, MonitoringResult monitor);

        int GetCapability(ICapability capability);

        void AddRule(ICapability sourceCapability, int sourceValue, ICapability targetCapability, int targetValue);

        void AddRule(IFailure sourceMonitor, MonitoringResult sourceValue, ICapability targetCapability, int targetValue);

        void AddTrigger(ICapability capability, Action<ICapability> function);
    }
}
