using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using DalApi;
using BO;
using BlApi;

namespace BL
{
    public partial class BL : IBL
    {
        private IDal dal = DalFactory.GetDal();

        Random random;

        internal static BL instance { get; } = new BL();
        public List<DroneToList> DroneToList { get; set; } //Dynamic drone's list at BL layer
        List<DO.BaseStation> baseStations;
        List<DO.Customer> customers;
        List<DO.Drone> drones;
        List<DO.Parcel> ParcelOfDelivery;
        HelpFunction helpFunction; 

        internal static double PowerConsumptionAvailable;
        internal static double PowerConsumptionLightWeight;
        internal static double PowerConsumptionMediumWeight;
        internal static double PowerConsumptionHeavyWeight;
        internal static double LoadingDrone;

        
        private BL()
        {
            try
            {
                dal = DalFactory.GetDal(); 
                DroneToList = new List<DroneToList>();
                helpFunction = new();
                random = new Random(DateTime.Now.Millisecond);

                baseStations = dal.GetBaseStationByPredicate().ToList();
                customers = dal.GetCustomersByPredicate().ToList();
                drones = dal.GetDronesByPredicate().ToList();
                ParcelOfDelivery = dal.GetPackagesByPredicate().ToList();

                foreach (var item in drones)
                {
                    DroneToList.Add(new DroneToList
                    {
                        DroneID = item.DroneID,
                        DroneModel = item.DroneModel,
                        DroneWeight = (WeightCategories)item.DroneWeight,
                    });
                }

                InitDroneToLists(); 
            }
            catch (DO.CheckIdException Ex)
            {
                throw new CheckIdException("ERORR", Ex);
            }
            catch (DO.CheckIfIdNotException Ex)
            {
                throw new CheckIfIdNotException("ERORR", Ex);
            }

        }

        public User GetUser(string userName)
        {
            try
            {
                DO.User DalUser = dal.GetUser(userName);
                User BlUser = new User();
                BlUser.FirstName = DalUser.FirstName;
                BlUser.LastName = DalUser.LastName;
                BlUser.Password = DalUser.Password;
                BlUser.UserId = DalUser.UserId;
                BlUser.UserName = DalUser.UserName;

                return BlUser;
            }
            catch (Exception)
            {

                throw new Exception("The User not exist");
            }
        }
      
    }
}