using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
//Throw Exception Classes 
namespace DO
{
    // [Serializable]
    public class DroneException : Exception 
    {
        public DroneException(string message) : base(message) { } 
    }
    public class ParcelException : Exception { public ParcelException(string message) : base(message) { } }
    public class CustumerException : Exception { public CustumerException(string message) : base(message) { } }
    public class BaseStationException : Exception { public BaseStationException(string message) : base(message) { } }
    public class DroneChargeException : Exception { public DroneChargeException(string message) : base(message) { } }
    public class UserException : Exception { public UserException(string message) : base(message) { } }
    public class CheckIdException : Exception { public CheckIdException(string message) : base(message) { } }
    public class CheckIfIdNotException : Exception { public CheckIfIdNotException(string message) : base(message) { } }
}
