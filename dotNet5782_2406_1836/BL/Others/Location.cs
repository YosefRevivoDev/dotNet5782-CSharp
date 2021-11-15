using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Location
    {
        public double Longtitude { get; init; }
        public double Latitude { get; init; }

        public override string ToString()
        {
            return $"Location:, Longtitude:{Longtitude}, Latitude:{Latitude}";
        }
    }
}
