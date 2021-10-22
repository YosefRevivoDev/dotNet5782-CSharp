using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct DroneCharge
        {
            public int DroneID { get; internal set; }
            public int StationID { get; internal set; }

            public override string ToString()
            {
                return $"ID:{DroneID}, StationID:{StationID}";
            }
        }
    }
}
