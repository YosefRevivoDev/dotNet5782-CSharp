using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CustomerInParcel
    {
        public int CustomerId { get; set; }
        public string NameCustomer { get; set; }


        public override string ToString()
        {
            return string.Format("Id {0}\t mane {1}\t", CustomerId, NameCustomer);
        }
    }
}
