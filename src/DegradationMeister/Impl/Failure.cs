namespace DegradationMeister.Impl
{
    /// <summary>
    /// Implements the IFailure interface
    /// </summary>
    public class Failure : IFailure
    {
        /// <summary>
        /// Stores the name of the failure
        /// </summary>
        private readonly string _name;

        public Failure(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Gets the name of the failure
        /// </summary>
        /// <returns>The name of the failure</returns>
        public override string ToString()
        {
            return _name;
        }
    }
}
