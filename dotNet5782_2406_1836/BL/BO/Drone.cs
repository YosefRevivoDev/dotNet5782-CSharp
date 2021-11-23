using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Drone : DroneCharge
    {
        public string Drone_Model { get; set; }
        public WeightCategories Drone_weight { get; set; }
        public DroneStatus Status { get; set; }
        public Location CurrentLocation { get; set; }
        public int PackageInDeliver { get; set; }

        public override string ToString()
        {
            return base.ToString()+$"Drone: {Drone_Model} ,WeightCategories: {Drone_weight} ," +
                $"DroneStatus: {Status} ,Location: {CurrentLocation} ,PackageInDeliver: {PackageInDeliver} ";
        }
    }
}
