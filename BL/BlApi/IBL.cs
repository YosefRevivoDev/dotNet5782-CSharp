using System;
using System.Collections.Generic;
using BO;

namespace BlApi
{
    public partial interface IBL
    {
        public IEnumerable<ParcelToList> GetParcelToListsByPredicate(Predicate<ParcelToList> p = null);
        public IEnumerable<CustomerToList> GetCustomerToList(Predicate<CustomerToList> p = null);
        public IEnumerable<BaseStationToList> GetBasetationToListsByPredicate(Predicate<BaseStationToList> p = null);
        public IEnumerable<DroneToList> GetDroneToListsBLByPredicate(Predicate<DroneToList> predicate = null);

        

        #region Operations of Drone
        void InitDroneToLists();
        bool ReleaseDroneFromCharge(int droneId);
        bool SendDroneToCharge(int droneId);
        bool CollectParcelByDrone(int droneID);
        bool DeliveryParcelToCustomer(int droneID);
        bool AssignmentOfPackageToDrone(int droneID);
        public ParcelAtCustomer GetParcelAtCustomer(int parcelID, int customrID);
        public CustomerInParcel GetCustomerInParcel(int customerID);
        public Location LocationOfTheNearestStation(Location customerLocation, List<DO.BaseStation> stations);
        public ParcelInDeliver GetParcelInDeliverd(Location firstLocation, int NumOfPackageId);
        #endregion

        #region GetObject
        public BaseStation GetBaseStation(int stationID);
        public Customer GetCustomer(int id);
        public Drone GetDrone(int id);
        public Parcel GetParcel(int id);
        public User GetUser(string userName);
        public ParcelToList GetParcelToList(int parcelID);
        public DroneInParcel GetDroneAtParcel(int droneId);
        public CustomerToList GetCustomerToList(int customerID);
        public int GetTheIdOfCloseStation(int idDrone);
        #endregion

        #region AddObject
        void AddBaseStation(BaseStation newBaseStation);
        void AddNewCustomer(Customer newCustomer);
        void AddNewDrone(Drone newDrone, int NumberOfStation);
        int AddNewParcel(Parcel newParcel, int SenderId, int TargetId);
        #endregion
        
        #region UpdateObject
        void UpdateDrone(int id, string newNameModel);
        void UpdateBaseStation(int stationId, string newNameStation, int sumOfChargestation);
        void UpdateCustomr(int customerId, string newNameCustomer, string newPhoneCustomer);
        #endregion

        #region RemoveObject
        void RemoveBaseStationBL(int id);
        void RemoveParcelBL(int id);
        void RemoveCustomerBL(int id);
        void RemoveDroneBL(int id);
        #endregion
    }
}