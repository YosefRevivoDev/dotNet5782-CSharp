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
        public DroneWeightCategories DroneWeight { get; set; }
        public DroneStatus Status { get; set; }
        public Location CurrentLocation { get; set; }
        public int PackageInDeliver { get; set; }

        public override string ToString()
        {
            return base.ToString() + $"Drone: {DroneModel} ,WeightCategories: {DroneWeight} ," +
                $"DroneStatus: {Status} ,Location: {CurrentLocation} ,PackageInDeliver: {PackageInDeliver} ";
        }
    }
}
