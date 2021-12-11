using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelToList : ParcelPoly
    {

        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public WeightCategories Weight { get; set; }
        public ParcelStatus ParcelStatus { get; set; }
        public int ParcelAreNotAssighmentToDrone { get; set; }
        public override string ToString()
        {
            return base.ToString() + $"Sender ID: {SenderId}, Target ID: {TargetId}, "
                +$"Status: {ParcelStatus} ";
        }
    }
}
