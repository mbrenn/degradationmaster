using DegradationMeister;
using DegradationMeister.Impl;

namespace DegradationMeisterTest
{
    public class PowerSupplyMonitor : Monitor
    {
        public IFailure TotalFailure { get; } = new Failure();

        public IFailure LimitedCurrent { get; } = new Failure();
    }
}