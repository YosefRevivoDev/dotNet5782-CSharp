using System;

namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {
           
            public int CustomerId { get;  set; }
            public string Name { get;  set; }
            public string Phone { get;  set; }
            public double Longtitude { get;  set; }
            public double Latitude { get;  set; }
            public override string ToString()
            {
                return $"Custumer: ID:{CustomerId}, Name:{Name},Phone:{Phone},Longtitude:{Longtitude},"+
                        $"Latitude:{Latitude}";
            }
        }
    }
}