using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class DroneToList : DroneCharge
    {
        public string Drone_Model { get; set; }
        public WeightCategories Drone_weight { get; set; }
        public Location CurrentLocation { get; set; }
        public int PackageDelivered { get; set; }
        public  DroneStatus Status { get; set; }

        public override string ToString()
        {
            return base.ToString() + $"DroneToList: {Drone_Model},WeightCategories:{Drone_weight}," +
                $"Location: {CurrentLocation},PackageDelivered: {PackageDelivered}, DroneStatus:{Status}";
        }

    }
}
