using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.DO
{
    public struct DroneCharge
    {
        public int DroneID { get; set; }
        public int StationID { get; set; }

        public override string ToString()
        {
            return $"ID:{DroneID}, StationID:{StationID}";
        }
    }

}
