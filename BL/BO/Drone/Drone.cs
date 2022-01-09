using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace BL
{
    public class Drone : DroneCharge, INotifyPropertyChanged
    {
        public string DroneModel { get; set; }
        public WeightCategories DroneWeight { get; set; }
        public DroneStatus Status { get; set; }
        public Location CurrentLocation { get; set; }
        public ParcelInDeliver ParcelInDeliverd { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void onPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            return base.ToString() + string.Format("Drone Model: " + DroneModel + "\r\n" + "WeightCategories: " + DroneWeight + "\r\n" +
                "DroneStatus: " + Status + "\r\n" + "Location: " + CurrentLocation + "\r\n" + "Parcel In Deliver: " + ParcelInDeliverd);
        }
    }
}
