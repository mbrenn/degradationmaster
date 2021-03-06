﻿namespace DegradationMeister
{
    public static class Extensions
    {
        /// <summary>
        /// Adds a default rule to set the target capability to Failed or Unknown depending on the source capability. 
        /// If additional relationships need to be established, further explicit calls are necessary
        /// </summary>
        /// <param name="degrader">Degrader to be used for the rule</param>
        /// <param name="sourceCapability">The capability whose change will have an effect on the <c>targetCapability</c></param>
        /// <param name="targetCapability">The targetcapability being used</param>
        public static void AddDefaultRules(
            this IDegrader degrader,
            ICapability sourceCapability,
            ICapability targetCapability)
        {
            AddRuleForFailure(degrader, sourceCapability, targetCapability);
            AddRuleForUnknown(degrader, sourceCapability, targetCapability);
        }

        /// <summary>
        /// Adds a default rule to set the target capability to Failed or Unknown depending on the failure. 
        /// NOK will be mapped to Failed, Unknown will be mapped to Unknown
        /// If additional relationships need to be established, further explicit calls are necessary
        /// </summary>
        /// <param name="degrader">Degrader to be used for the rule</param>
        /// <param name="failure">The falilue whose change will have an effect on the <c>targetCapability</c></param>
        /// <param name="targetCapability">The targetcapability being used</param>
        public static void AddDefaultRules(
            this IDegrader degrader,
            IFailure failure,
            ICapability targetCapability)
        {
            AddRuleForFailure(degrader, failure, targetCapability);
            AddRuleForUnknown(degrader, failure, targetCapability);
        }

        /// <summary>
        /// Adds a rule, that the target Capability shall be Failed in case of a degraded source Capability
        /// </summary>
        /// <param name="degrader">Degrader being used to add the rule</param>
        /// <param name="sourceCapability"></param>
        /// <param name="targetCapability"></param>
        public static void AddRuleForFailure(
            this IDegrader degrader, 
            ICapability sourceCapability,
            ICapability targetCapability)
        {
            degrader.AddRule(sourceCapability, Capabilities.Failed, targetCapability, Capabilities.Failed);
        }

        /// <summary>
        /// Add a rule that target Capability shall be failed in case of a NOK monitoring result
        /// </summary>
        /// <param name="degrader">Degrader being used to add the rule</param>
        /// <param name="failure"></param>
        /// <param name="targetCapability"></param>
        public static void AddRuleForFailure(
            this IDegrader degrader,
            IFailure failure,
            ICapability targetCapability)
        {
            degrader.AddRule(failure, MonitoringResult.NOK, targetCapability, Capabilities.Failed);
        }

        /// <summary>
        /// Adds a rule that the target capability shall be set to unknown in case the source capability is unknown
        /// </summary>
        /// <param name="degrader">Degrader being used to add the rule</param>
        /// <param name="sourceCapability">Source capability that is triggering the change of the targetCapability</param>
        /// <param name="targetCapability">The target capability that will be updated</param>
        public static void AddRuleForUnknown(
            this IDegrader degrader, 
            ICapability sourceCapability,
            ICapability targetCapability)
        {
            degrader.AddRule(sourceCapability, Capabilities.Unknown, targetCapability, Capabilities.Unknown);
        }

        /// <summary>
        /// Add a rule that target Capability shall be unknown in case of a unknown monitoring result
        /// </summary>
        /// <param name="degrader">Degrader being used to add the rule</param>
        /// <param name="failure"></param>
        /// <param name="targetCapability"></param>
        public static void AddRuleForUnknown(
            this IDegrader degrader, 
            IFailure failure,
            ICapability targetCapability)
        {
            degrader.AddRule(failure, MonitoringResult.Unknown, targetCapability, Capabilities.Unknown);
        }
    }
}
