

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int DroneID { get;  set; }
            public string Drone_Model { get;  set; }
            public WeightCategories Drone_weight { get;  set; }
            //public DroneStatus status { get;  set; }
            //public double Battary  { get;  set; }
            public override string ToString()
            {
                return $"Drone:, ID:{DroneID}, Model:{Drone_Model}, Weight:{Drone_weight}";
                    /*",DroneStatus:{status}, Battary: { Battary}"*/
            }
        }  
    }
}