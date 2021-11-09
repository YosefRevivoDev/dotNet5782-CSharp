using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    class ParcelToList : Parcel_polymorphism
    {
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public WeightCategories Weight { get; set; }
        public  Priorities Priority { get; set; }
        public  ParcelStatus ParcelStatus { get; set; }
        public override string ToString()
        {
            return base.ToString() + $"Sender ID:{SenderId}, Target ID:{TargetId} , Priority: {Priority}"
                +$"Status {ParcelStatus}";
        }
    }
}
