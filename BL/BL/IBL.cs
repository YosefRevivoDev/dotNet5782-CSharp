using System;
using System.Collections.Generic; 


namespace BL
{
    public partial interface IBL
    {
        List<DroneToList> DroneToList { get; set; }
        IEnumerable<ParcelToList> GetParcelToLists();
        IEnumerable<CustomerToList> GetCustomerToList();
        IEnumerable<BaseStationToList> GetBasetationToLists();
      //  IEnumerable<DroneToList> GetDroneToListsBLByPredicate();

        void InitDroneToLists();
        void ReleaseDroneFromCharge(int droneId, int stationId, DateTime dateTime);
        void SendDroneToCharge(int droneId, int stationId);
        void CollectParcelByDrone(int droneID);
        void AssignmentOfPackageToDrone(int droneID, ParcelInDeliver parcelInDeliverd);



        #region GetObject
        BaseStation GetBaseStation(int stationID);
        Customer GetCustomer(int id);
        Drone GetDrone(int id);
        Parcel GetParcel(int id);
        User GetUser(string userName);
        #endregion

        #region AddObject
        void AddBaseStation(BaseStation newBaseStation);
        void AddNewCustomer(Customer newCustomer);
        void AddNewDrone(Drone newDrone, int NumberOfStation);
        void AddNewParcel(Parcel newParcel, int SenderId, int TargetId);
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