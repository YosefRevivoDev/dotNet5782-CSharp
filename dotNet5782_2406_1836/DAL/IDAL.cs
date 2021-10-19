using DAL;
using System;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategories Parcel_weight { get; internal set; }
            public priority Parcel_priority { get; internal set; }
            public DateTime intvation_date { get; internal set; }
            public int Dronled  { get; set; }
            public DateTime Schueduled { get; internal set; }
            public DateTime PickedUp { get; internal set; }
            public DateTime Delivered { get; internal set; }


        }
    }
}
