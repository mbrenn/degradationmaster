using System;
using System.Collections.Generic;
using System.Linq;

namespace DegradationMeister.Impl
{
    public class Degrader : IDegrader
    {
        public List<CapabilityRuleSet> Rules = new List<CapabilityRuleSet>();

        public CapabilityRuleSet GetRuleSetFor(ICapability targetCapability)
        {
            var result = Rules.FirstOrDefault(x => x.TargetCapability == targetCapability);
            if (result == null)
            {
                result = new CapabilityRuleSet()
                {
                    TargetCapability = targetCapability
                };

                Rules.Add(result);
            }

            return result;
        }

        public void UpdateMonitoringResult(IFailure failure, MonitoringResult monitor)
        {
            throw new NotImplementedException();
        }

        public int GetCapability(ICapability capability)
        {
            return capability.CurrentValue;
        }

        public void AddRule(ICapability sourceCapability, int sourceValue, ICapability targetCapability, int targetValue)
        {
            var ruleSet = GetRuleSetFor(targetCapability);
            throw new NotImplementedException();
        }

        public void AddRule(IFailure sourceMonitor, MonitoringResult sourceValue, ICapability targetCapability, int targetValue)
        {
            throw new NotImplementedException();
        }

        public void AddTrigger(ICapability capability, Action<ICapability> function)
        {
            throw new NotImplementedException();
        }
    }   
}