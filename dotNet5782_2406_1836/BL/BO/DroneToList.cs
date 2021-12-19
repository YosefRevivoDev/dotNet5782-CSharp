using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneToList : DroneCharge
    {
        public string DroneModel { get; set; }
        public WeightCategories DroneWeight { get; set; }
        public Location CurrentLocation { get; set; }
        public int NumOfPackageDelivered { get; set; } // num of packege
        public DroneStatus Status { get; set; }

        public override string ToString()
        {
            return base.ToString() + string.Format("Drone Model: " + DroneModel + "\r\n" + "DroneWeightCategories: " + DroneWeight + "\r\n" +
                "Location: " + CurrentLocation + "\r\n" + "PackageDelivered: " + NumOfPackageDelivered + "\r\n" + "DroneStatus: " + Status);
        }

    }
}
