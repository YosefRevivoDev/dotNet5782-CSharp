

namespace IDAL.DO
{
    public struct Drone
    {
        public int DroneID { get; set; }
        public string DroneModel { get; set; }
        public WeightCategories DroneWeight { get; set; }
        public DroneStatus status { get; set; }
        public override string ToString()
        {
            return $"Drone:, ID:{DroneID}, Model:{DroneModel}, Weight:{DroneWeight},DroneStatus:{status}";
        }
    }

}