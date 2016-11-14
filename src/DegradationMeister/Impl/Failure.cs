namespace DegradationMeister.Impl
{
    public class Failure : IFailure
    {
        private readonly string _name;

        public Failure(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
