using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
   public class BasetationToList : BasestationPoly
    {
     public int NotAvailableChargingStations { get; set; }
        public override string ToString()
        {
            return base.ToString() + $" Not available charging stations:{NotAvailableChargingStations}";
        }
    }
}
