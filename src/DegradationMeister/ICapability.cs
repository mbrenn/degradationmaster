namespace DegradationMeister
{
    public interface ICapability
    {
        /// <summary>
        /// Gets or sets the information about the degrader being connected to the capability
        /// </summary>
        IDegrader Degrader { get; set; }

        int CurrentValue { get; set; }
    }
}