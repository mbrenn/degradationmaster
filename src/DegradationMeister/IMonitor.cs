using System;

namespace DegradationMeister
{
    /// <summary>
    /// Defines the interface for the monitor
    /// </summary>
    public interface IMonitor
    {
        /// <summary>
        /// Gets or sets the degrader being invoked in case of an update
        /// </summary>
        IDegrader Degrader { get; set; }
    }
}