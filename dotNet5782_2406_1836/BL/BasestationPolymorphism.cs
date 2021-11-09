using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    class BasestationPolymorphism
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int AvailableChargingStations { get; set; }
        public override string ToString()
        {
            return  $" Basestation ID:{ID} ,Basestation name: {Name} , " +
                $"AvailableChargingStations: {AvailableChargingStations}";
        }

    }
}
