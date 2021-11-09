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
            double [] temp = dal.RequetPowerConsumption();
            PowerConsumption_Available = temp[0];

            List<IDAL.DO.Drone> DroneList = dal.GetDronesByPredicate().ToList();

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

                    }
                    else
                    {
                        item.Location.Latitude = customer.Latitude;
                        item.Location.Longtitude = customer.Longtitude;
                    }
                }
                
            }



        }
    }
}
