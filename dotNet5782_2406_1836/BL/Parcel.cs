using System;

namespace BL
{
    public class Parcel : Parcel_polymorphism
    {
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public Priorities Priorities { get; set; }
        public IDAL.DO.Drone Drone { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime PickedUp { get; set; }
        public DateTime Delivered { get; set; }
        public DateTime Requested { get; set; }


    }
}