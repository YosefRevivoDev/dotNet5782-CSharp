using System;

namespace BO
{
    public class Parcel : ParcelPoly
    {
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public DroneInParcel Drone { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime PickedUp { get; set; }
        public DateTime Delivered { get; set; }
        public DateTime Requested { get; set; }

        public override string ToString()
        {
            return base.ToString() + $"CustomerInParcel: {Sender}, CustomerInParcel: {Target}, " +
                $"DroneInParcel: {Drone}, DateTime: {CreateTime}, DateTime: {PickedUp}, DateTime: {Delivered}, DateTime: {Requested} ";
        }

    }
}