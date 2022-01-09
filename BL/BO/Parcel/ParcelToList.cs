using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ParcelToList : ParcelPoly
    {

        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public SenderOrTarget SenderOrTarget { get; set; }
        public ParcelStatus parcelStatus { get; set; }
        public int ParcelAreNotAssighmentToDrone { get; set; }
        public override string ToString()
        {
            return base.ToString() + $"Sender ID: {SenderId}, Target ID: {TargetId}," +
                $" ParcelAreNotAssighmentToDrone: {ParcelAreNotAssighmentToDrone} ";
        }
    }
}
