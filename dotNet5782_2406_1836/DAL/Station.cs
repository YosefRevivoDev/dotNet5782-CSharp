namespace IDAL
{
    namespace DO
    {
        public class Station
        { 
            public Station(int id, string name, double longitude, double latitude, int chargeSlots)
            {
                this.StationID = id;
                this.Name = name;
                this.Longtitude = longitude;
                this.Latitude = latitude;
                this.ChargeSlots = chargeSlots;
            }

            public int StationID { get; internal set; }
            public string Name { get; internal set; }
            public double Longtitude { get; internal set; }
            public double Latitude { get; internal set; }
            public int ChargeSlots { get; internal set; }
            public override string ToString()
            {
                return $"ID:{StationID}, Name:{Name}, Longtitude:{Longtitude}," +
                 $"Latitude:{Latitude}, ChargeSlots:{ChargeSlots}";
            }

        }
    }
}