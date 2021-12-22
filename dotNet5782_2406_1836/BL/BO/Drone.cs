using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Drone : DroneCharge
    {
        public string DroneModel { get; set; }
        public WeightCategories DroneWeight { get; set; }
        public DroneStatus Status { get; set; }
        public Location CurrentLocation { get; set; }
        public ParcelInDeliver ParcelInDeliverd { get; set; }


        public override string ToString()
        {
            return base.ToString() + string.Format("Drone Model: " + DroneModel + "\r\n" + "WeightCategories: " + DroneWeight + "\r\n" +
                "DroneStatus: " + Status + "\r\n" + "Location: " + CurrentLocation + "\r\n");
        }
    }
}
