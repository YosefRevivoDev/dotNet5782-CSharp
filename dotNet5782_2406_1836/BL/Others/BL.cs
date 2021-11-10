using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BL : IBL.IBL
    {
        IDAL.IDal dal;
        public List<DroneToList> DroneToLists;

        internal static double PowerConsumption_Available = 0;
        internal static double PowerConsumption_LightWeight = 0;
        internal static double PowerConsumption_MediumWeight = 0;
        internal static double PowerConsumption_HeavyWeight = 0;
        internal static double LoadingDrone = 0;

        public BL()
        {

            dal = new DalObject.DalObject();
            DroneToLists = new List<DroneToList>();
            double[] temp = dal.RequetPowerConsumption();
            PowerConsumption_Available = temp[0];

            List<IDAL.DO.Drone> DroneList = dal.GetDronesByPredicate().ToList();
            List<BaseStation> baseStations = new();

            //for (int i = 0; i < DroneList.Count; i++)
            //{
            //    DroneToLists[i].id = DroneList[i].DroneID;
            //}
            List<IDAL.DO.Parcel> parcels =
                dal.GetPackagesByPredicate(x => x.DroneId != 0 && x.Delivered == DateTime.MinValue).ToList();
            foreach (var item in DroneToLists)
            {
                int index = parcels.FindIndex(x => x.DroneId == item.DroneID);
                if (index != -1)
                {
                    item.Status = DroneStatus.busy;
                    IDAL.DO.Customer customer = dal.GetCustomer(parcels[index].SenderId);
                    if (parcels[index].PickedUp == DateTime.MinValue)
                    {
                        double tmp, short_distance = 0;
                        foreach (var b in baseStations)
                        {
                            tmp = Distance(b.Location.Latitude, customer.Latitude, b.Location.Longtitude, customer.Longtitude);
                            if (short_distance < tmp)
                            {
                                
                            }
                            
                        }
                    }
                    else
                    {
                        item.CurrentLocation.Latitude = customer.Latitude;
                        item.CurrentLocation.Longtitude = customer.Longtitude;
                    }
                }

            }

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
    }
}
