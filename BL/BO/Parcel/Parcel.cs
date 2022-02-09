using System;

namespace BO
{
    public class Parcel : ParcelPoly
    {
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public DroneInParcel DroneInParcel { get; set; }
        public DateTime Requested { get; set; }
        public DateTime assigned { get; set; }
        public DateTime PickedUp { get; set; }
        public DateTime Delivered { get; set; }

        public override string ToString()
        {
            return base.ToString() + $"CustomerInParcel: {Sender}, CustomerInParcel: {Target}, " +
                $"DroneInParcel: {DroneInParcel}, DateTime: {assigned}, DateTime: {PickedUp}, DateTime: {Delivered}, DateTime: {Requested} ";
        }

    }
}