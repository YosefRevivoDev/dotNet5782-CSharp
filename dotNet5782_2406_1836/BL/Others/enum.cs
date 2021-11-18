using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public enum DroneStatus { available, maintenance, busy }
    public enum WeightCategories {light = 1,medium = 2,heavy = 3 }
    public enum Priorities { regular, fast, emergency }
    public enum ParcelStatus { Defined, associated, collected, provided }
}
