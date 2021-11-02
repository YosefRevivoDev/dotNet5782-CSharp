using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.DO
{
    public class DroneException : Exception
    {
        public DroneException(string messge) : base(messge){}
    }
}
