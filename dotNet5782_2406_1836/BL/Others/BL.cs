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
             
            dal = new DalObject.DalObject(); // Access to summon methods from Datasource
            DroneToLists = new List<DroneToList>();
            double[] temp = dal.RequetPowerConsumption();
            PowerConsumption_Available = temp[0];


            List<IDAL.DO.Drone> DroneList = dal.GetDronesByPredicate().ToList();
            List<BaseStation> baseStations = new();

            
            List<IDAL.DO.Parcel> parcels = dal.GetPackagesByPredicate(x => x.DroneId != 0).ToList();// Sorts packages that belong to the drone but are not provided
            foreach (var item in DroneToLists)
            {
                int index = parcels.FindIndex(x => x.DroneId == item.DroneID && x.Delivered == DateTime.MinValue);
                if (index != -1)
                {
                    item.Status = DroneStatus.busy;
                    IDAL.DO.Customer customer = dal.GetCustomer(parcels[index].SenderId);
                    if (parcels[index].PickedUp == DateTime.MinValue)
                    {
                        double shortDistance = 0;
                        foreach (var b in baseStations)
                        {
                            shortDistance = BO.HelpFunction.ShortDistance(b.Location.Latitude, customer.Latitude,
                                b.Location.Longtitude, customer.Longtitude);
                            if (shortDistance == BO.HelpFunction.Distance(b.Location.Latitude, customer.Latitude,
                                   b.Location.Longtitude, customer.Longtitude))
                            {
                                customer.Latitude = b.Location.Latitude;
                                customer.Longtitude = b.Location.Longtitude;
                            }
                        }
                        if (shortDistance == -1) // לבדוק אם זה false
                        {
                            throw new Exception("There are no stations in the area");
                        }
                    }
                    else
                    {
                        item.CurrentLocation.Latitude = customer.Latitude;
                        item.CurrentLocation.Longtitude = customer.Longtitude;
                    }
                    
                    // מתחילים מצב סוללה 


                }
               
            }
        }

    }
}
