using System;
using System.Collections.Generic;
using System.Linq;

namespace DegradationMeister.Impl
{
    public class Degrader : IDegrader
    {
        private readonly string _name;

        private readonly Dictionary<IFailure, MonitoringResult> _failureStatus = 
            new Dictionary<IFailure, MonitoringResult>();

        private readonly List<CapabilityRuleSet> _rules = new List<CapabilityRuleSet>();

        public Degrader(string name)
        {
            _name = name;
        }

        private CapabilityRuleSet GetRuleSetFor(ICapability targetCapability, bool createIfNotExisting)
        {
            var result = _rules.FirstOrDefault(x => x.Capability == targetCapability);
            if (result == null)
            {
                if (!createIfNotExisting)
                {
                    throw new InvalidOperationException("Given capability does not exist");
                }

                result = new CapabilityRuleSet
                {
                    Capability = targetCapability
                };

                _rules.Add(result);
            }

            return result;
        }

        public void UpdateMonitoringResult(IFailure failure, MonitoringResult monitoringResult)
        {
            _failureStatus[failure] = monitoringResult;

            foreach (var ruleSet in _rules)
            {
                foreach (var rule in ruleSet.Rules
                    .OfType<CapabilityRuleSet.FailureRule>()
                    .Where(x=> x.Failure.Equals(failure)))
                {
                    if (rule.Failure.Equals(failure))
                    {
                        UpdateDegradation(ruleSet.Capability);
                    }
                }
            }
        }

        public void UpdateDegradation(ICapability ruleSetTargetCapability)
        {
            Console.WriteLine(" -- Updating for: " + ruleSetTargetCapability);
            var ruleSet = GetRuleSetFor(ruleSetTargetCapability, false);
            foreach (var rule in ruleSet.Rules)
            {
                var ruleAsFailureRule = rule as CapabilityRuleSet.FailureRule;
                var ruleAsCapability = rule as CapabilityRuleSet.CapabilityRule;

                if (ruleAsFailureRule != null)
                {
                    MonitoringResult result;
                    if (!_failureStatus.TryGetValue(ruleAsFailureRule.Failure, out result))
                    {
                        result = MonitoringResult.Unknown;
                    }

                    if (ruleAsFailureRule.Value == result)
                    {
                        ChangeCapabilityTo(ruleSet, rule.TargetCapability);
                        return; // First match is success
                    }
                }

                if (ruleAsCapability != null)
                {
                    if (ruleAsCapability.Value == ruleAsCapability.Capability.CurrentValue)
                    {
                        ChangeCapabilityTo(ruleSet, rule.TargetCapability);
                        return;
                    }
                }
            }

            // If no rule matches, capability is assumed as OK
            ruleSet.Capability.CurrentValue = Capabilities.Passed;
        }

        private void ChangeCapabilityTo(CapabilityRuleSet ruleSet, int ruleTargetCapability)
        {
            var current = ruleSet.Capability.CurrentValue;
            if (current != ruleTargetCapability)
            {
                ruleSet.Capability.CurrentValue = ruleTargetCapability;
                foreach (var trigger in ruleSet.Triggers)
                {
                    trigger(ruleSet.Capability);
                }
            }
        }

        public void AddRule(ICapability sourceCapability, int sourceValue, ICapability targetCapability, int targetValue)
        {
            targetCapability.Degrader = this;

            var ruleSet = GetRuleSetFor(targetCapability, true);
            if (ruleSet == null) throw new ArgumentNullException(nameof(ruleSet));
            ruleSet.Rules.Add(
                new CapabilityRuleSet.CapabilityRule()
                {
                    Capability = sourceCapability,
                    Value =  sourceValue,
                    TargetCapability = targetValue
                });

            sourceCapability.Degrader.AddTrigger(sourceCapability, x => UpdateDegradation(targetCapability));
        }

        public void AddRule(IFailure failure, MonitoringResult sourceValue, ICapability targetCapability, int targetValue)
        {
            targetCapability.Degrader = this;

            var ruleSet = GetRuleSetFor(targetCapability, true);
            if (ruleSet == null) throw new ArgumentNullException(nameof(ruleSet));
            ruleSet.Rules.Add(
                new CapabilityRuleSet.FailureRule()
                {
                    Failure = failure,
                    Value = sourceValue,
                    TargetCapability = targetValue
                });
        }

        public void AddTrigger(ICapability capability, Action<ICapability> function)
        {
            var ruleSet = GetRuleSetFor(capability, true);
            if (ruleSet == null) throw new ArgumentNullException(nameof(ruleSet));

            ruleSet.Triggers.Add(function);
        }

        public override string ToString()
        {
            return _name;
        }
    }   
}