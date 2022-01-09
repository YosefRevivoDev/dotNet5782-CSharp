

namespace DO
{
    public struct Drone
    {
        public int DroneID { get; set; }
        public string DroneModel { get; set; }
        public WeightCategories DroneWeight { get; set; }
        public override string ToString()
        {
            return string.Format("DroneID: " + DroneID
            +"\n" + "Model: " + "\n" + DroneModel
            +"Weight: " + "\n" + DroneWeight);
        }
    }

}