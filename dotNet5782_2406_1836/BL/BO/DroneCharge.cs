using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneCharge
    {
        public int DroneID { get; set; }
        public double BattaryStatus  { get;  set; }


        public override string ToString()
        {
            return $"DroneCharge: ID: {DroneID} , BattaryStatus: {BattaryStatus} ";
        }

    }
}
