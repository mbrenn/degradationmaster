using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegradationMeister.Impl
{
    public class CapabilityRuleSet
    {
        public ICapability Capability { get; set; }

        public List<Rule> Rules { get; } = new List<Rule>();

        public List<Action<ICapability>> Triggers { get; } = new List<Action<ICapability>>();

        public class Rule
        {
            public int TargetCapability { get; set; }
        }
        public class FailureRule : Rule
        {
            public IFailure Failure { get; set; }
            public MonitoringResult Value { get; set; }
        }

        public class CapabilityRule : Rule
        {
            public ICapability Capability { get; set; }
            public int Value { get; set; }
        }
    }
}
