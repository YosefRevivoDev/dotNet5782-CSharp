using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class DroneCharge
    {
        public int DroneID { get; set; }
        public DroneStatus status { get;  set; }

        public override string ToString()
        {
            return $"DroneCharge: ID:{DroneID}, DroneStatus:{status}";
        }

    }
}
