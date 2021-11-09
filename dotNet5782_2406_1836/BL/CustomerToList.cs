using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    class CustomerToList: CustomerInParcel
    {
        public int Phone { get; set; }
        public int SendParcelAndSupplied { get; set; }
        public int SendParcelAndNotSupplied { get; set; }
        public int ParcelsReciever { get; set; }
        public int ParcelOweyToCustomer { get; set; }

        public override string ToString()
        {
            return base.ToString()+$"Phone {Phone},SendParcelAndSupplied{SendParcelAndSupplied}," +
                $"SendParcelAndNotSupplied{SendParcelAndNotSupplied},ParcelsReciever{ParcelsReciever}," +
                $"ParcelOweyToCustomer{ParcelOweyToCustomer}";
        }


    }
}
