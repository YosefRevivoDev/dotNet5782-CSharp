using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{

    public class ParcelAssociationExeptions : Exception
    {
        public ParcelAssociationExeptions(string messge) : base(messge) { }
    } 
    public class CheckIdException : Exception
    {
        public CheckIdException(string messge, Exception innerExeption) : base(messge) { }
    }
    public class CheckIfIdNotException : Exception
    {
        public CheckIfIdNotException(string messge, Exception innerExeption) : base(messge) { }
    } public class CheckIfIdNotExceptions : Exception
    {
        public CheckIfIdNotExceptions(string messge) : base(messge) { }
    }
    public class ChargExeptions : Exception
    {
        public ChargExeptions(string messge) : base(messge) { }
    }
    public class invalidValueForChargeSlots : Exception
    {
        public invalidValueForChargeSlots(string messge) : base(messge) { }
    }
}
