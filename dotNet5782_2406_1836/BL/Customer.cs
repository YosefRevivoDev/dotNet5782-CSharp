using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Customer
    {
        public int  CustomeId { get; set; }
        public string NameCustomer { get; set; }
        public int PhoneCustomer { get; set; }
        public int LocationCustomer { get; set; }

        internal static List<Parcel> PackagesToCustomer = new List<Parcel>();
        internal static List<Parcel> PackagesFromCustomer = new List<Parcel>();
    }
}
