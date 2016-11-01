using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DegradationMeister;
using DegradationMeister.Impl;

namespace DegradationMeisterTest
{
    public class PowerSupply
    {
    }

    class Program
    {
        static void Main(string[] args)
        {
            var monitorPowerSupply = new PowerSupplyMonitor();
            ICapability capabilityPowerSupply = new Capability();

            var monitorActuator = new ActuatorMonitor();
            ICapability capabilityActuator = new Capability();

            ICapability capabilitySystem = new Capability();

            IDegrader degraderPowerSupply = new Degrader();
            IDegrader degraderActuator = new Degrader();

            IDegrader degraderSystem = new Degrader();

            degraderPowerSupply.AddRule(monitorPowerSupply.TotalFailure, MonitoringResult.NOK, capabilityPowerSupply, Capabilities.NOK);
            degraderPowerSupply.AddRule(monitorPowerSupply.LimitedCurrent, MonitoringResult.NOK, capabilityPowerSupply, Capabilities.Limited);

            degraderActuator.AddRule(monitorActuator.TotalFailure, MonitoringResult.NOK, capabilityActuator, Capabilities.NOK);
            degraderActuator.AddRule(capabilityPowerSupply, Capabilities.NOK, capabilityActuator, Capabilities.NOK);
            degraderActuator.AddRule(capabilityPowerSupply, Capabilities.Limited, capabilityActuator, Capabilities.Limited);

            degraderSystem.AddRule(capabilityActuator, Capabilities.NOK, capabilitySystem, Capabilities.NOK);
            degraderSystem.AddRule(capabilityActuator, Capabilities.Limited, capabilitySystem, Capabilities.Limited);

            degraderSystem.AddTrigger(capabilitySystem, x => Console.WriteLine(x.ToString()));

            monitorPowerSupply.InvokeFailure(monitorPowerSupply.TotalFailure, MonitoringResult.OK);
        }
    }
}
