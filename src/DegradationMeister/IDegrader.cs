using System;
using System.Collections.Generic;

namespace DegradationMeister
{
    public interface IDegrader
    {
        void UpdateMonitoringResult(IFailure failure, MonitoringResult monitoringResult);

        /// <summary>
        /// Updates the degradation for the given capability
        /// </summary>
        /// <param name="capability">Capability which shall be upgraded</param>
        /// <param name="alreadyDegraded">Enumeration of the capabilities which were upgraded</param>
        void UpdateDegradation(ICapability capability, HashSet<ICapability> alreadyDegraded);

        /// <summary>
        /// Adds a rule that if the source capability has the value sourceValue, the targetCapability shall
        /// retrieve the value targetValue
        /// </summary>
        /// <param name="sourceCapability">Capability whose change will be degrade another capability. This capability can be owned by another degrader</param>
        /// <param name="sourceValue">The degradation state that will trigger a change in the targetCapability</param>
        /// <param name="targetCapability">The capability that will be changed</param>
        /// <param name="targetValue">The degradation that will be put to the targetValue</param>
        void AddRule(ICapability sourceCapability, int sourceValue, ICapability targetCapability, int targetValue);

        /// <summary>
        /// Adds a rule that the the targetCapability shall get the targetValue if the given monitor of
        /// the given failure retruns the degradation state
        /// </summary>
        /// <param name="failure">Failure that triggers a change of the capability</param>
        /// <param name="monitoringResult">The monitoring result that will trigger a change of the <c>targetCapability</c>.</param>
        /// <param name="targetCapability">The capability that will be changed</param>
        /// <param name="targetValue">The degradation that will be put to the targetValue</param>
        void AddRule(IFailure failure, MonitoringResult monitoringResult, ICapability targetCapability, int targetValue);

        /// <summary>
        /// Adds a trigger which is called in case of a change of the capability degradation. 
        /// </summary>
        /// <param name="capability">Capability whose change will be triggered</param>
        /// <param name="function">Action that will be called in case of a change</param>
        void AddTrigger(ICapability capability, Action<ICapability> function);

        /// <summary>
        /// Adds a dependency of two capabilities. 
        /// Whenever the source capability has a new degradation, the target capability needs to be updated
        /// </summary>
        /// <param name="sourceCapability">Capability which will be changed</param>
        /// <param name="targetCapability">Capability that is updated in case of an update of the source capability</param>
        void AddDependency(ICapability sourceCapability, ICapability targetCapability);
    }
}
