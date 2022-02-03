using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;


namespace BL
{
    internal class HelpFunction
    {
        //internal static double ShortDistance(double latitude1, double latitude2, double longtitude1, double longtitude2)
        //{
        //    double Dtemp;
        //    Dtemp = Distance(latitude1, latitude2, longtitude1, longtitude2);
        //    if (shortDistance > Dtemp)
        //    {
        //        shortDistance = Dtemp;
        //    }
        //    return shortDistance;
        //}

        const double PIx = 3.141592653589793;
        const double Radius = 6378.16;

        /// <summary>
        /// Convert degrees to Radians
        /// </summary>
        /// <param name="x">Degrees</param>
        /// <returns>The equivalent in radians</returns>
        internal double ToRadians(double x)
        {
            return x * PIx / 180;
        }
        /// <summary>
        /// Calculate the distance between two places.
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        internal double DistanceBetweenLocations(Location location1, Location location2)
        {
            double dlongtitude = ToRadians(location2.Longtitude - location1.Longtitude);
            double dlatitude = ToRadians(location2.Longtitude - location1.Longtitude);

            double a = (Math.Sin(dlatitude / 2) * Math.Sin(dlatitude / 2)) + Math.Cos(ToRadians(location1.Longtitude)) * 
                Math.Cos(ToRadians(location2.Longtitude)) * (Math.Sin(dlongtitude / 2) * Math.Sin(dlongtitude / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return angle * Radius;
        }

        
        public Location IndexOfMinDistancesBetweenLocations(Location customerLocation, List<DO.BaseStation> baseStations)
        {
            DO.BaseStation closeStation = baseStations[0];
            double distance = double.MaxValue;

            for (int i = 0; i < baseStations.Count; i++)
            {
                Location stationLocation = new() { Latitude = baseStations[i].Latitude, Longtitude = baseStations[i].Longtitude };
                double distance1 = DistanceBetweenLocations(stationLocation, customerLocation);
                if (distance1 < distance)
                {
                    distance = distance1;
                    closeStation = baseStations[i];
                }
            }
            Location location = new() { Longtitude = closeStation.Longtitude, Latitude = closeStation.Latitude };
            return location;
        }
        //public static double BatteryStatusBetweenLocations(Location location, DroneToList drone, WeightCategories weight = default)
        //{

        //    double BatteryStatusBetweenLocations =
        //        Distance(location.Latitude, drone.CurrentLocation.Latitude, location.Longtitude, drone.CurrentLocation.Longtitude);
        //    switch (weight)
        //    {
        //        case WeightCategories.light:
        //            BatteryStatusBetweenLocations *=  BL.PowerConsumptionLightWeight;
        //            break;
        //        case WeightCategories.medium:
        //            BatteryStatusBetweenLocations *= BL.PowerConsumptionMediumWeight;
        //            break;
        //        case WeightCategories.heavy:
        //            BatteryStatusBetweenLocations *= BL.PowerConsumptionHeavyWeight;
        //            break;
        //        default:
        //            BatteryStatusBetweenLocations *= BL.PowerConsumptionAvailable;
        //            break;
        //    }
        //    //Check if drone's way enough to pass the mession 
        //    return drone.BattaryStatus - BatteryStatusBetweenLocations > 0 ? BatteryStatusBetweenLocations : default;
        //}

    }
}


