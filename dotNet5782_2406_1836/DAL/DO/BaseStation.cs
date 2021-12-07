using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.DO
{
    public struct BaseStation
    {
        public BaseStation(int id, string name, double longitude, double latitude, int chargeSlots)
        {
            this.StationID = id;
            this.Name = name;
            this.Longtitude = longitude;
            this.Latitude = latitude;
            this.AvailableChargeSlots = chargeSlots;
        }

        public int StationID { get; set; }
        public string Name { get; set; }
        public double Longtitude { get; set; }
        public double Latitude { get; set; }
        public int AvailableChargeSlots { get; set; }
        public override string ToString()
        {
            return $"Station: ID: {StationID}, Name: {Name}, Longtitude: {Longtitude}," +
             $"Latitude: {Latitude}, ChargeSlots: {AvailableChargeSlots}";
        }
    }
}