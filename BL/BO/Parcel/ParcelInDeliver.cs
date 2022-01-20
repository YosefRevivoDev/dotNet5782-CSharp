using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelInDeliver
    {
        public int ID;
        public StatusParcrlInDeliver StatusParcrlInDeliver { get; set; }
        public Priorities Priorities { get; set; }
        public WeightCategories WeightCategories { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public Location CollectionLocation { get; set; }
        public Location DeliveryDestination { get; set; }
        public double TransportDistance { get; set; }

        public override string ToString()
        {
            return string.Format("ID: " + ID + "\r\n" + "StatusParcrlInDeliver: " + StatusParcrlInDeliver + "\r\n"+
              "Priorities: " + Priorities + "\r\n" + "WeightCategories: " + WeightCategories + "\r\n" +
             "CustomerInParcel: " + Sender + "\r\n" + "CustomerInParcel: "+ Target + "\r\n" +
             "LocationCollection: " +CollectionLocation+ "\r\n" +"LocationDestination: "+ DeliveryDestination
             + "\r\n" + "TransportDistance: " + TransportDistance);
        }
    }
}
