using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DegradationMeister;
using DegradationMeister.Impl;
using NUnit.Framework;

namespace DegradationMeisterTest
{
    [TestFixture]
    class Tests
    {
        private static ICapability _capabilityPowerSupply;
        private static ICapability _capabilityActuator;
        private static ICapability _capabilitySystem;

        [Test]
        public void TestDegrader()
        {
            var degraderPowerSupply = new Degrader("Power Supply");
            var monitorPowerSupply = new PowerSupplyMonitor(degraderPowerSupply);
            _capabilityPowerSupply = new Capability(degraderPowerSupply, "Power Supply");

            var degraderActuator = new Degrader("Actuator");
            var monitorActuator = new ActuatorMonitor(degraderActuator);
            _capabilityActuator = new Capability(degraderActuator, "Actuator");

            var degraderSystem = new Degrader("System");
            _capabilitySystem = new Capability(degraderSystem, "System");

            degraderPowerSupply.AddDefaultRules(monitorPowerSupply.TotalFailure, _capabilityPowerSupply);
            degraderPowerSupply.AddRuleForUnknown(monitorPowerSupply.LimitedCurrent, _capabilityPowerSupply);
            degraderPowerSupply.AddRule(monitorPowerSupply.LimitedCurrent, 
                MonitoringResult.NOK, 
                _capabilityPowerSupply,
                Capabilities.Limited);


            degraderActuator.AddDefaultRules(monitorActuator.TotalFailure, _capabilityActuator);
            degraderActuator.AddDefaultRules(_capabilityPowerSupply, _capabilityActuator);
            degraderActuator.AddRule(_capabilityPowerSupply, Capabilities.Limited, _capabilityActuator,
                Capabilities.Limited);

            degraderSystem.AddRule(_capabilityActuator, Capabilities.Failed, _capabilitySystem, Capabilities.Failed);
            degraderSystem.AddRule(_capabilityActuator, Capabilities.Limited, _capabilitySystem, Capabilities.Limited);

            degraderSystem.AddTrigger(_capabilitySystem,
                x => Console.WriteLine($"- System: {Capabilities.Convert(x.CurrentDegradation)}"));

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
        }

        [Test]
        public void TestMassDegradation()
        { 
            // Creates complex Degrading and capability hierarchy
            var degrader = new Degrader("Test");
            var capability = new Capability(degrader, "Test");

            var failure = CreateSup(degrader, capability, 0);

            var watch = new Stopwatch();
            watch.Start();
            for (var m = 0; m < 2000; m++)
            {
                failure.InjectTotalFailure();
                Assert.That(capability.CurrentDegradation, Is.EqualTo(Capabilities.Failed));

                failure.MakeOk();
                Assert.That(capability.CurrentDegradation, Is.EqualTo(Capabilities.Full));
            }

            watch.Stop();

            Console.WriteLine(watch.Elapsed.ToString());
        }

        private static ActuatorMonitor CreateSup(IDegrader degrader, ICapability capability, int i)
        {
            ActuatorMonitor last = null;
            for (var n = 0; n < 5; n++)
            {
                var subDegrader = new Degrader("Sub");
                var subCapability = new Capability(subDegrader, "Sub");

                var failure = new ActuatorMonitor(subDegrader);
                subDegrader.AddDefaultRules(failure.TotalFailure, subCapability);
                degrader.AddDefaultRules(subCapability, capability);

                if (i < 5)
                {
                    last = CreateSup(subDegrader, subCapability, i + 1);
                }
                else
                {
                    last = failure;
                }

                failure.MakeOk();
            }

            return last;
        }

        private static void GiveStatus()
        {
            Console.WriteLine(
                $" - Power Supply: {Capabilities.Convert(_capabilityPowerSupply.CurrentDegradation)} - " +
                $"Actuator: {Capabilities.Convert(_capabilityActuator.CurrentDegradation)} - " + 
                $"System: {Capabilities.Convert(_capabilitySystem.CurrentDegradation)}");
        }
    }
}
