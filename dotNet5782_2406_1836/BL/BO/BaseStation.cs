using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    class BaseStation : BasestationPoly
    {
        public Location location { get; set; }
        List<DroneCharge> DroneCharge = new List<DroneCharge>();

        public override string ToString()
        {
            return base.ToString() + $" Location:{location}, DroneCharge:{DroneCharge}" + 
                string.Join("/t", DroneCharge);
        }
    }
}
