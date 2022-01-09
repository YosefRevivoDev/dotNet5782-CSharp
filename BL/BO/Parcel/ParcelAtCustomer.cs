using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ParcelAtCustomer : ParcelPoly
    {
        public ParcelStatus ParcelStatus { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }

        public override string ToString()
        {
            return base.ToString() + string.Format("Parcel Status: " + ParcelStatus + "\r\n" +
                "CustomerInParcel: " + Sender + "\r\n" + "Customer In Parcel: " + Target);
        }
    }
}
