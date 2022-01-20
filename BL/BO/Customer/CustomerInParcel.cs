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
            return $"CustomerId: {CustomerId} , NameCustomer: {NameCustomer} ";
        }
    }
}
