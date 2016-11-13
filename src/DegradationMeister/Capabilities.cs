using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegradationMeister
{
    public class Capabilities
    {
        public const int Unknown = 0;
        public const int Passed = 1;
        public const int Failed = 2;
        public const int Limited = 3;

        public static string Convert(int degradation)
        {
            switch (degradation)
            {
                case 0:
                    return nameof(Unknown);
                case 1:
                    return nameof(Passed);
                case 2:
                    return nameof(Failed);
                case 3:
                    return nameof(Limited);
            }

            return degradation.ToString();
        }
    }
}
