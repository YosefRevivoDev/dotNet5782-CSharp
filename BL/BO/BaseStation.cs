using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BaseStation
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int AvailableChargingStations { get; set; }
        public Location Location { get; set; }
        public List<DroneCharge> droneCharges = new List<DroneCharge>();
        

        public override string ToString()
        {

            return base.ToString() + $"Location: {Location} , " +
                string.Join(Environment.NewLine + " :", droneCharges
            + $" Basestation ID: {ID} ," + $"Basestation name: {Name}" +
                $"AvailableChargingStations: {AvailableChargingStations}");
        }
    }
}
