using System;
using System.Collections.Generic;

namespace BO
{
    public interface IBL
    {
        List<DroneToList> DroneToList { get; set; }

        void AddBaseStation(BaseStation newBaseStation);
        void AddNewCustomer(Customer newCustomer);
        void AddNewDrone(DroneToList newDrone, int NumberOfStation);
        void AddNewParcel(Parcel newParcel, int SenderId, int TargetId);
        BaseStation GetBaseStation(int stationID);
        IEnumerable<BasetationToList> GetBasetationToLists();
        Customer GetCustomer(int id);
        IEnumerable<CustomerToList> GetCustomerToList();
        Drone GetDrone(int id);
        void InitDroneToLists();
        Parcel GetParcel(int id);
        IEnumerable<ParcelToList> GetParcelToLists();
        void ReleaseDroneFromCharge(int droneId, DateTime dateTime);
        void SendDroneToCharge(int droneId);
        void UpadateDrone(int id, string newNameModel);
        void UpdateBaseStation(int stationId, string newNameStation, int sumOfChargestation);
        void UpdateCustomr(int customerId, string newNameCustomer, string newPhoneCustomer);
        void PackageCollectionByDrone(int pacelIdAssociate, int droneIdAssociate);
        void DeliveredPackageToCustumer(int parcelIdAssociate, int droneIdAssociate);
       // void ReleasingChargeDrone(int droneIdAssociate, int baseStationId);
    }
}