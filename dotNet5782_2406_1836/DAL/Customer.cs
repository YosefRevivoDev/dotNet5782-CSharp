using DAL;

namespace IDAL
{
    namespace DO
    {
        public class Customer
        {

            public int Id { get;  set; }
            public string Name { get;  set; }
            public string Phone { get;  set; }
            public double Longtitude { get;  set; }
            public double Latitude { get;  set; }
            public override string ToString()
            {
                return $"ID:{Id}, Name:{Name},Phone:{Phone},Longtitude:{Longtitude},"+
                 $"Latitude:{Latitude}";
            }
        }
    }
}