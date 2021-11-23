using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
   public class BasestationPoly
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int AvailableChargingStations { get; set; }
        public override string ToString()
        {
            return string.Join(Environment.NewLine + " :" + $" Basestation ID: {ID} ," + $"Basestation name: {Name}",
                $"AvailableChargingStations: {AvailableChargingStations} ");
        }

    }
}
