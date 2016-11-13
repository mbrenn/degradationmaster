using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DegradationMeister;
using DegradationMeister.Impl;

namespace DegradationMeisterTest
{   class Program
    {
        private static ICapability _capabilityPowerSupply;
        private static ICapability _capabilityActuator;
        private static ICapability _capabilitySystem;

        static void Main(string[] args)
        {
            IDegrader degraderPowerSupply = new Degrader("Power Supply");
            IDegrader degraderActuator = new Degrader("Actuator");

            var monitorPowerSupply = new PowerSupplyMonitor(degraderPowerSupply);
            _capabilityPowerSupply = new Capability("Power Supply");

            var monitorActuator = new ActuatorMonitor(degraderActuator);
            _capabilityActuator = new Capability("Actuator");
            _capabilitySystem = new Capability("System");

            IDegrader degraderSystem = new Degrader("System");

            degraderPowerSupply.AddRuleForUnknown(monitorPowerSupply.TotalFailure, _capabilityPowerSupply);
            degraderPowerSupply.AddRuleForUnknown(monitorPowerSupply.LimitedCurrent, _capabilityPowerSupply);
            degraderPowerSupply.AddRule(monitorPowerSupply.TotalFailure, MonitoringResult.NOK, _capabilityPowerSupply, Capabilities.Failed);
            degraderPowerSupply.AddRule(monitorPowerSupply.LimitedCurrent, MonitoringResult.NOK, _capabilityPowerSupply, Capabilities.Limited);

            degraderActuator.AddRuleForUnknown(monitorActuator.TotalFailure, _capabilityActuator) ;
            degraderActuator.AddRule(monitorActuator.TotalFailure, MonitoringResult.NOK, _capabilityActuator, Capabilities.Failed);
            degraderActuator.AddRule(_capabilityPowerSupply, Capabilities.Failed, _capabilityActuator, Capabilities.Failed);
            degraderActuator.AddRule(_capabilityPowerSupply, Capabilities.Limited, _capabilityActuator, Capabilities.Limited);

            degraderSystem.AddRule(_capabilityActuator, Capabilities.Failed, _capabilitySystem, Capabilities.Failed);
            degraderSystem.AddRule(_capabilityActuator, Capabilities.Limited, _capabilitySystem, Capabilities.Limited);

            degraderSystem.AddTrigger(_capabilitySystem, x => Console.WriteLine($"- System: {Capabilities.Convert(x.CurrentValue)}"));

            GiveStatus();
            Console.WriteLine("Pass Power Supply...");
            monitorPowerSupply.MakeOk();
            GiveStatus();
            Console.WriteLine("Pass Power Actuator...");
            monitorActuator.MakeOk();
            GiveStatus();

            Console.WriteLine("Injecting Power Supply Failure...");
            monitorPowerSupply.InjectTotalFailure();
            GiveStatus();

            Console.WriteLine("Rehealing Power Supply Failure...");
            monitorPowerSupply.MakeOk();
            GiveStatus();

            Console.WriteLine("Injecting Limited Power Supply Failure...");
            monitorPowerSupply.InjectLimitedCurrentFailure();
            GiveStatus();

            Console.WriteLine("Rehealing Power Supply Failure...");
            monitorPowerSupply.MakeOk();
            GiveStatus();

            Console.WriteLine("Injecting Actuator Failure...");
            monitorActuator.InjectTotalFailure();
            GiveStatus();

            Console.WriteLine("Rehealing Actuator Failure...");
            monitorActuator.MakeOk();
            GiveStatus();

            Console.WriteLine("Injecting Power Supply + Actuator Failure...");
            monitorActuator.InjectTotalFailure();
            monitorPowerSupply.InjectTotalFailure();
            GiveStatus();

            Console.WriteLine("Rehealing Actuator Failure...");
            monitorActuator.MakeOk();
            GiveStatus();

            Console.WriteLine("Rehealing Power Supply Failure...");
            monitorPowerSupply.MakeOk();
            GiveStatus();

            Console.ReadKey();
        }

        private static void GiveStatus()
        {
            Console.WriteLine(
                $" - Power Supply: {Capabilities.Convert(_capabilityPowerSupply.CurrentValue)} - " +
                $"Actuator: {Capabilities.Convert(_capabilityActuator.CurrentValue)} - " + 
                $"System: {Capabilities.Convert(_capabilitySystem.CurrentValue)}");
        }
    }
}
