using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.DO
{
    public struct Parcel
    {
        public int ParcelId { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public WeightCategories Parcel_weight { get; set; }
        public Priorities Parcel_priority { get; set; }
        public DateTime intvation_date { get; set; }
        public int DroneId { get; set; }
        public DateTime Scheduled { get; set; }
        public DateTime PickedUp { get; set; }
        public DateTime Delivered { get; set; }
        public DateTime Requested { get; set; }
        public override string ToString()
        {
            return $"Parcel: , Id:{ParcelId}, Senderld: {SenderId}" +
                    $", Targetld: {TargetId}" +
                    $", Weight: {Parcel_weight}" +
                    $", Priority: {Parcel_priority}" +
                    $", Requested: {Requested}" +
                    $", Droneld: {DroneId}" +
                    $", Scheduled: {Scheduled}" +
                    $", PickedUp: {PickedUp}" +
                    $", Delivered: {Delivered}";
        }
    }

}
