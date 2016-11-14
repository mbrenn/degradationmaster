using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace DegradationMeister.Impl
{
    /// <summary>
    /// Implements the degrader which is responsible to find out the status of the degradations
    /// </summary>
    public class Degrader : IDegrader
    {
        private readonly string _name;

        /// <summary>
        /// caches an enumeration of the failures of each monitor
        /// </summary>
        private readonly Dictionary<IFailure, MonitoringResult> _failureStatus = 
            new Dictionary<IFailure, MonitoringResult>();

        private readonly List<CapabilityRuleSet> _rules = new List<CapabilityRuleSet>();

        public Degrader(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Gets the ruleset for a certain capability. If the ruleset does not exist before, 
        /// it will be created and a new instance will be returned. 
        /// </summary>
        /// <param name="targetCapability">Capability whose ruleset is queried</param>
        /// <param name="createIfNotExisting">true, if the ruleset shall be created</param>
        /// <returns>The found capability set</returns>
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

        /// <summary>
        /// Updates the monitoring result of a failure, including all the degradation which will be updated
        /// </summary>
        /// <param name="failure">Failure which is changed</param>
        /// <param name="monitoringResult">Requests an update of the failure</param>
        public void UpdateMonitoringResult(IFailure failure, MonitoringResult monitoringResult)
        {
            // Checks, if the failure has changed
            MonitoringResult oldResult;
            if (_failureStatus.TryGetValue(failure, out oldResult))
            {
                if (oldResult == monitoringResult)
                {
                    // No change of failure... so just return
                    return;
                }
            }

            _failureStatus[failure] = monitoringResult;

            // Go through all rulesets where the failure is allocated
            var alreadyUpdated = new HashSet<ICapability>();
            foreach (var ruleSet in _rules)
            {
                foreach (var rule in ruleSet.Rules
                    .OfType<FailureRule>()
                    .Where(x=> x.Failure.Equals(failure)))
                {
                    if (rule.Failure.Equals(failure))
                    {
                        UpdateDegradation(ruleSet.Capability, alreadyUpdated);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the degradation if requested by the failure
        /// </summary>
        /// <param name="capability">Capability which is updated</param>
        public void UpdateDegradation(ICapability capability, HashSet<ICapability> alreadyUpdated)
        {
            if (alreadyUpdated.Contains(capability))
            {
                Console.WriteLine(" -- Already updated: " + capability);
                return;
            }

            alreadyUpdated.Add(capability);
            Console.WriteLine(" -- Updating for: " + capability);

            var ruleSet = GetRuleSetFor(capability, false);
            foreach (var rule in ruleSet.Rules)
            {
                var ruleAsFailureRule = rule as FailureRule;

                if (ruleAsFailureRule != null)
                {
                    MonitoringResult result;
                    if (!_failureStatus.TryGetValue(ruleAsFailureRule.Failure, out result))
                    {
                        result = MonitoringResult.Unknown;
                    }

                    // Checks, if the failure status matches to the capability
                    if (ruleAsFailureRule.Value == result)
                    {
                        ChangeCapabilityTo(ruleSet, rule.TargetCapability, alreadyUpdated);
                        return; // First match is success
                    }
                }
            }
            foreach (var rule in ruleSet.Rules)
            {
                var ruleAsCapability = rule as CapabilityRule;
                if (ruleAsCapability != null)
                {
                    // Checks if the capability matches to the current value
                    if (ruleAsCapability.Value == ruleAsCapability.Capability.CurrentValue)
                    {
                        ChangeCapabilityTo(ruleSet, rule.TargetCapability, alreadyUpdated);
                        return;
                    }
                }
            }

            // If no rule matches, capability is assumed as OK
            ChangeCapabilityTo(ruleSet, Capabilities.Passed, alreadyUpdated);
        }

        /// <summary>
        /// Changes the value of the capabiltiy to the given value
        /// </summary>
        /// <param name="ruleSet">Ruleset of the capability containing also the triggers</param>
        /// <param name="targetCapability">The target capability value</param>
        /// <param name="alreadyUpdated">The capabilities that already have been updated</param>
        private static void ChangeCapabilityTo(CapabilityRuleSet ruleSet, int targetCapability, HashSet<ICapability> alreadyUpdated )
        {
            var current = ruleSet.Capability.CurrentValue;
            if (current != targetCapability)
            {
                ruleSet.Capability.CurrentValue = targetCapability;

                foreach (var dependent in ruleSet.DependentCapabilities)
                {
                    dependent.Degrader.UpdateDegradation(dependent, alreadyUpdated);
                }

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
                new CapabilityRule()
                {
                    Capability = sourceCapability,
                    Value =  sourceValue,
                    TargetCapability = targetValue
                });

            var sourceRuleSet = GetRuleSetFor(sourceCapability, true);
            sourceRuleSet.AddDependent(targetCapability);
        }

        public void AddRule(IFailure failure, MonitoringResult sourceValue, ICapability targetCapability, int targetValue)
        {
            targetCapability.Degrader = this;

            var ruleSet = GetRuleSetFor(targetCapability, true);
            if (ruleSet == null) throw new ArgumentNullException(nameof(ruleSet));
            ruleSet.Rules.Add(
                new FailureRule()
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