using System;
using System.Collections.Generic;


namespace BO
{
    public partial interface IBL
    {
        List<DroneToList> DroneToList { get; set; }
        IEnumerable<ParcelToList> GetParcelToLists();
        IEnumerable<CustomerToList> GetCustomerToList();
        IEnumerable<BasetationToList> GetBasetationToLists();
        BaseStation GetBaseStation(int stationID);
        Customer GetCustomer(int id);
        Drone GetDrone(int id);
        Parcel GetParcel(int id);
        void AddBaseStation(BaseStation newBaseStation);
        void AddNewCustomer(Customer newCustomer);
        void AddNewDrone(Drone newDrone, int NumberOfStation);
        void AddNewParcel(Parcel newParcel, int SenderId, int TargetId);
        void InitDroneToLists();
        void ReleaseDroneFromCharge(int droneId, int stationId, DateTime dateTime);
        //void ReleaseDroneFromCharged();
        void SendDroneToCharge(int droneId, int stationId);
        void UpdateDrone(int id, string newNameModel);
        void UpdateBaseStation(int stationId, string newNameStation, int sumOfChargestation);
        void UpdateCustomr(int customerId, string newNameCustomer, string newPhoneCustomer);
        //  void ChargeDrone(int droneIdAssociate, int baseStationId);
        User GetUser(string userName);
    }
}