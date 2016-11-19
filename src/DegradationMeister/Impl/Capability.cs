namespace DegradationMeister.Impl
{
    public class Capability : ICapability
    {
        private readonly string _name;

        public Capability(IDegrader degrader, string name = "")
        {
            _name = name;
            Degrader = degrader;
        }

        public IDegrader Degrader { get; }

        public int Current { get; set; } = 0;

        public override string ToString()
        {
            return _name;
        }
    }
}