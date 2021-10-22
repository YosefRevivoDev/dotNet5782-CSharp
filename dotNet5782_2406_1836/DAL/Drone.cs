using DAL;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int DroneID { get; internal set; }
            public string Drone_Model { get; internal set; }
            public WeightCategories Drone_weight { get; internal set; }
            public DroneStatus status { get; internal set; }
            public double Battary  { get; internal set; }
            public override string ToString()
            {
                return $"ID:{DroneID}, Model:{Drone_Model}, Weight:{Drone_weight} "+
                    $",DroneStatus:{status}, Battary: { Battary}";
            }
        }  
    }
}