using System;
using System.Collections.Generic;
using DO;

namespace DalApi
{
    public interface IDal
    {
        #region Add object
        void AddBaseStation(BaseStation new_baseStation);
        void AddCustomer(Customer new_customer);
        void AddDrone(Drone new_drone);
        void AddDroneCharge(DroneCharge droneCharge);
        int AddParcel(Parcel new_parcel);
        #endregion

        #region Get object
        BaseStation GetBaseStation(int Id);
        Parcel GetParcel(int Id);
        User GetUser(string userName);
        Drone GetDrone(int Id);
        Customer GetCustomer(int Id);
        void GetDroneChargeByStation(int baseStationId);
        DroneCharge GetDroneChargeByDrone(int droneId);
        #endregion

        #region Update object
        void UpdateBaseStation(BaseStation baseStation);
        void UpdateCustomer(Customer customer);
        void UpdateDrone(Drone drone);
        //void UpdateDroneCharge(DroneCharge droneCharge);
        void UpdateParcel(Parcel parcrl);
       // void UpdateUser(User user);
        #endregion

        #region Remove object
        void RemoveBaseStation(int stationID);
        void RemoveCustomer(int customerId);
        void RemoveDrone(int droneID);
        void RemoveParcel(int parcelId);
        void RemoveUser(string UserID);
        #endregion

        #region Display lists
        IEnumerable<BaseStation> GetBaseStationByPredicate(Predicate<BaseStation> predicate = null);
        IEnumerable<Customer> GetCustomersByPredicate(Predicate<Customer> predicate = null);
        IEnumerable<Drone> GetDronesByPredicate(Predicate<Drone> predicate = null);
        IEnumerable<Parcel> GetPackagesByPredicate(Predicate<Parcel> predicate = null);
        IEnumerable<DroneCharge> GetDroneChargesByPredicate(Predicate<DroneCharge> predicate = null);
        #endregion

        #region Operations 
        void PackageCollectionByDrone(int parcelId);
        void ReleasingChargeDrone(int droneId, int baseStationId);
        bool DeliveredPackageToCustumer(int parcelId);
        void SetDroneForParcel(int parcelId, int droneId);
        void MinusDroneCharge(int stationId);
        void PlusDroneCharge(int stationId);
        #endregion

        double[] RequetPowerConsumption();
    }
}