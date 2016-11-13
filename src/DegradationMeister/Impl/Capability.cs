namespace DegradationMeister.Impl
{
    public class Capability : ICapability
    {
        private readonly string _name;

        public Capability(string name)
        {
            _name = name;
        }

        public IDegrader Degrader { get; set; }

        public int CurrentValue { get; set; } = 0;

        public override string ToString()
        {
            return _name;
        }
    }
}