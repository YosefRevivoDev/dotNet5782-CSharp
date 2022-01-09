using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   public class BaseStationToList: BaseStation
    {
     public int NotAvailableChargingStations { get; set; }
        public override string ToString()
        {
            return base.ToString() + $" Not available charging stations: {NotAvailableChargingStations} ";
        }
    }
}
