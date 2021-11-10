using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    class BaseStation : BasestationPoly
    {
        public Location Location { get; set; }
        List<DroneCharge> droneCharge = new List<DroneCharge>();

        public override string ToString()
        {
            return base.ToString() + $" Location:{Location}," + 
                string.Join("/t", droneCharge);
        }
    }
}
