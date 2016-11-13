namespace DegradationMeister
{
    public static class Extensions
    {
        public static void AddRuleForFailure(this IDegrader degrader, ICapability sourceCapability,
            ICapability targetCapability)
        {
            degrader.AddRule(sourceCapability, Capabilities.Failed, targetCapability, Capabilities.Failed);
        }

        public static void AddRuleForFailure(this IDegrader degrader, IFailure failure,
            ICapability targetCapability)
        {
            degrader.AddRule(failure, MonitoringResult.NOK, targetCapability, Capabilities.Failed);
        }

        public static void AddRuleForUnknown(this IDegrader degrader, ICapability sourceCapability,
            ICapability targetCapability)
        {
            degrader.AddRule(sourceCapability, Capabilities.Unknown, targetCapability, Capabilities.Unknown);
        }

        public static void AddRuleForUnknown(this IDegrader degrader, IFailure failure,
            ICapability targetCapability)
        {
            degrader.AddRule(failure, MonitoringResult.Unknown, targetCapability, Capabilities.Unknown);
        }
    }
}
