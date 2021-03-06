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
        public Location location { get; set; }
        public List<DroneInCharging> droneCharges = new List<DroneInCharging>();
        
        public override string ToString()
        {

            return base.ToString() + $"Location: {location} , " +
                string.Join(Environment.NewLine + " :", droneCharges
            + $" Basestation ID: {ID} ," + $"Basestation name: {Name}" +
                $"AvailableChargingStations: {AvailableChargingStations}");
        }
    }
}
