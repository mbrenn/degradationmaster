namespace DegradationMeister
{
    public interface ICapability
    {
        /// <summary>
        /// Gets or sets the information about the degrader being connected to the capability
        /// </summary>
        IDegrader Degrader { get; }

        /// <summary>
        /// Gets or sets current Degradation of the capability
        /// </summary>
        int Current { get; set; }
    }
}