using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
     public class Customer : CustomerInParcel
    {
        
        public string PhoneCustomer { get; set; }
        public Location LocationCustomer { get; set; }

        public List<ParcelAtCustomer> PackagesToCustomer { get; set; }
        public List<ParcelAtCustomer> PackagesFromCustomer { get; set; }

        public override string ToString()
        {
            return base.ToString()+$"Customer: PhoneCustomer{PhoneCustomer} ," +
                $"LocationCustomer: {LocationCustomer} " + string.Join("/t", PackagesToCustomer)
                +string.Join("/t", PackagesFromCustomer);
        }
    }
}
