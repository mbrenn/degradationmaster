namespace DegradationMeister.Impl
{
    public class Capability : ICapability
    {
        public IDegrader Degrader { get; set; }

        public int CurrentValue { get; set; } = 0;
    }
}