using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;   

namespace BO
{
    public class ParcelPoly
    {
        public int Id { get; set; }
        public  WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }

        public override string ToString()
        {
            return $"ID: {Id} , weight: {Weight},Priorities: {Priority} ";
        }
    }
}
