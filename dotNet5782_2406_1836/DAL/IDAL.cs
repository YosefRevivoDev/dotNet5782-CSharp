using System;

namespace IDAL
{
    namespace DO
    {
      public struct customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longtitude { get; set; }
            public double Latitude { get; set; }
        }

        public struct Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            
        }
    }
}
