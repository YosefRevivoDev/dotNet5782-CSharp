using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneInCharging
    {
        public int DroneID { get; set; }
        public double BattaryStatus { get; set; }


        public override string ToString()
        {
            return string.Format("ID: " + DroneID + "\r\n" + "BattaryStatus: " + BattaryStatus + "\r\n");
        }

    }
}
