namespace DegradationMeister.Impl
{
    public class Capability : ICapability
    {
        private readonly string _name;

        public Capability(Degrader degrader, string name = "")
        {
            _name = name;
            Degrader = degrader;
        }

        public IDegrader Degrader { get; }

        public int CurrentDegradation { get; set; } = 0;

        public override string ToString()
        {
            return _name;
        }
    }
}