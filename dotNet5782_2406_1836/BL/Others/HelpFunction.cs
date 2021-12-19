using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public partial class HelpFunction
    {
        static double shortDistance = 400000000;
        internal static double ShortDistance(double latitude1, double latitude2, double longtitude1, double longtitude2)
        {
            double Dtemp;
            Dtemp = Distance(latitude1, latitude2, longtitude1, longtitude2);
            if (shortDistance > Dtemp)
            {
                shortDistance = Dtemp;
            }
            return shortDistance;
        }


        //---------------Calculate distance betweene 2 cordinates------------//
        static double ToRadians(double angleIn10thofaDegree)
        {
            // Angle in 10th
            // of a degree
            return (angleIn10thofaDegree * Math.PI) / 180;
        }
        internal static double Distance(double latitude1, double latitude2, double longtitude1, double longtitude2)
        {
            // The math module contains
            // a function named toRadians
            // which converts from degrees
            // to radians.
            longtitude1 = ToRadians(longtitude1);
            longtitude2 = ToRadians(longtitude2);
            latitude1 = ToRadians(latitude1);
            latitude2 = ToRadians(latitude2);

            // Haversine formula
            double dlon = longtitude2 - longtitude1;
            double dlat = latitude2 - latitude1;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                       Math.Cos(latitude1) * Math.Cos(latitude2) *
                       Math.Pow(Math.Sin(dlon / 2), 2);

            double c = 2 * Math.Asin(Math.Sqrt(a));

            // Radius of earth in
            // kilometers. Use 3956
            // for miles
            double r = 6371;

            // calculate the result
            return (c * r);
        }

        public static (int, double) IndexOfMinDistancesBetweenLocations(List<IDAL.DO.BaseStation> baseStations, Location location)
        {
            List<double> distances = new List<double>();
            foreach (var b in baseStations)
            {
                distances.Add(Distance(b.Latitude, location.Latitude,
                    b.Longtitude, location.Longtitude));
            }
            return (distances.FindIndex(i => i == distances.Min()), distances.Min());
        }

        public static double BatteryStatusBetweenLocations(Location location, DroneToList drone, WeightCategories weight)
        {

            double BatteryStatusBetweenLocations =
                Distance(location.Longtitude, drone.CurrentLocation.Latitude, location.Latitude, drone.CurrentLocation.Longtitude);
            switch (weight)
            {
                case WeightCategories.light:
                    BatteryStatusBetweenLocations *= BL.PowerConsumptionLightWeight;
                    break;
                case WeightCategories.medium:
                    BatteryStatusBetweenLocations *= BL.PowerConsumptionMediumWeight;
                    break;
                case WeightCategories.heavy:
                    BatteryStatusBetweenLocations *= BL.PowerConsumptionHeavyWeight;
                    break;
                default:
                    break;
            }
            //Check if drone's way enough to pass the mession 
            return drone.BattaryStatus - BatteryStatusBetweenLocations > 0 ? BatteryStatusBetweenLocations : default;
        }

       
    }
}


