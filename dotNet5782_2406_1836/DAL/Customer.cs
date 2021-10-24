using DAL;

namespace IDAL
{
    namespace DO
    {
        internal struct Customer
        {
            public int Id { get; internal set; }
            public string Name { get; internal set; }
            public string Phone { get; internal set; }
            public double Longtitude { get; internal set; }
            public double Latitude { get; internal set; }
        }
    }
}