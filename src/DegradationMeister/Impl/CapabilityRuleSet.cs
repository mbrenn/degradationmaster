using System;
using System.Collections.Generic;

namespace DegradationMeister.Impl
{
    public class CapabilityRuleSet
    {
        public ICapability Capability { get; set; }

        public List<Rule> Rules { get; } = new List<Rule>();

        public List<Action<ICapability>> Triggers { get; } = new List<Action<ICapability>>();

        public List<ICapability> DependentCapabilities { get; } = new List<ICapability>();

        /// <summary>
        /// Adds the information that the given capability is dependent on the this capability. 
        /// This means that if the this capability is updated, the dependent capabilities also have to be updated
        /// </summary>
        /// <param name="capability">Capability which needs to be updated in case of an update of this capability</param>
        public void AddDependent(ICapability capability)
        {
            if (!DependentCapabilities.Contains(capability))
            {
                DependentCapabilities.Add(capability);
            }
        }

        public override string ToString()
        {
            return $"Ruleset for {Capability}";
        }
    }


    public class Rule
    {
        public int TargetCapability { get; set; }
    }

    public class CapabilityRule : Rule
    {
        public ICapability Capability { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return $"Depending on {Capability}";
        }
    }

    public class FailureRule : Rule
    {
        public IFailure Failure { get; set; }
        public MonitoringResult Value { get; set; }

        public override string ToString()
        {
            return $"Depending on {Failure}";
        }
    }
}
