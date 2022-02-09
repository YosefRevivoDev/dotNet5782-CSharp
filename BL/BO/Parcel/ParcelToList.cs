using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelToList : ParcelPoly
    {

        public string SenderName { get; set; }
        public string TargetName { get; set; }
        public ParcelStatus parcelStatus { get; set; }
        public int ParcelAreNotAssighmentToDrone { get; set; }

        public override string ToString()
        {
            return base.ToString() + $"Sender Name: {SenderName}, Target Name: {TargetName}," +
                $" ParcelAreNotAssighmentToDrone: {ParcelAreNotAssighmentToDrone} ";
        }
    }
}
