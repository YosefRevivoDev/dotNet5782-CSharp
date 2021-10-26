using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int ParcelId { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategories Parcel_weight { get; internal set; }
            public Priorities Parcel_priority { get; internal set; }
            public DateTime intvation_date { get; internal set; }
            public int DroneId { get; set; }
            public DateTime Scheduled { get; internal set; }
            public DateTime PickedUp { get; internal set; }
            public DateTime Delivered { get; internal set; }
            public DateTime Requested { get; internal set; }



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
}
