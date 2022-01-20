using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneInParcel : DroneInCharging
    {
        public Location CorrentLocation { get; set; }
        
        public override string ToString()
        {
            return base.ToString() + string.Format("Location: " + "\r\n" +CorrentLocation);
        }
    }
}
