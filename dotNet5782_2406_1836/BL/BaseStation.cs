using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    class BaseStation : BasestationPolymorphism
    {
        public Location location { get; set; }
        
        List<DroneCharge> DroneCharge = new List<DroneCharge>();
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
