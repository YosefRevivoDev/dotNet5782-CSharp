using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;   

namespace BL
{
    public class Parcel_polymorphism
    {
        public int Id { get; set; }
        public  WeightCategories weight { get; set; }
        public override string ToString()
        {
            return $"ID:{Id}, weight:{weight}";
        }
    }
}
