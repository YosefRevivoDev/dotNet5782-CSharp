using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class DroneToList
    {
        public int DroneID { get; set; }
        public string Drone_Model { get; set; }
        public WeightCategories Drone_weight { get; set; }
        public DroneStatus Status { get; set; }
        public Location Location { get; set; }
    }
}
