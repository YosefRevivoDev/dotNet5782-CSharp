using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public struct Parcel
    {
        public int ParcelId { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public WeightCategories ParcelWeight { get; set; }
        public Priorities ParcelPriority { get; set; }
        public int DroneId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Assignment { get; set; }
        public DateTime Delivered { get; set; }
        public DateTime PickedUp { get; set; }
        public override string ToString()
        {
            return $"Parcel: , Id:{ParcelId}, Senderld: {SenderId}" +
                    $", Targetld: {TargetId}" +
                    $", Weight: {ParcelWeight}" +
                    $", Priority: {ParcelPriority}" +
                    $", DroneId: {DroneId}" +
                    $", Scheduled: {Created}" +
                    $", Assignment: {Assignment}" +
                    $", Delivered: {Delivered}" +
                    $", PickedUp: {PickedUp}";
        }
    }

}
