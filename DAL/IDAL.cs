using System;
using System.Collections.Generic;
using IDAL.DO;

namespace IDAL
{
    public interface IDal
    {
        void AddBaseStation(BaseStation new_baseStation);
        void AddCustomer(Customer new_customer);
        void AddDrone(Drone new_drone);
        void AddDroneCharge(DroneCharge droneCharge);
        void AddParcel(Parcel new_parcel);
        void ChargeDrone(int droneId, int baseStationId);
        void DeliveredPackageToCustumer(int parcelId, int droneId);
        BaseStation GetBaseStation(int Id);
        IEnumerable<BaseStation> GetBaseStationByPredicate(Predicate<BaseStation> predicate = null);
        Customer GetCustomer(int Id);
        IEnumerable<Customer> GetCustomersByPredicate(Predicate<Customer> predicate = null);
        Drone GetDrone(int Id);
        IEnumerable<Drone> GetDronesByPredicate(Predicate<Drone> predicate = null);
        IEnumerable<Parcel> GetPackagesByPredicate(Predicate<Parcel> predicate = null);
        IEnumerable<DroneCharge> GetDroneChargesByPredicate(Predicate<DroneCharge> predicate = null);
        Parcel GetParcel(int Id);
        User GetUser(string userName);
        void PackageCollectionByDrone(int parcelId, int droneId);
        void ReleasingChargeDrone(int droneId, int baseStationId);
        void RemoveBaseStation(int stationID);
        void RemoveCustomer(int customerId);
        void RemoveDrone(int droneID);
        void RemoveParcel(int parcelId);
        void SetDroneForParcel(int parcelId, int droneId);
        void UpdateBaseStation(BaseStation baseStation);
        void UpdateCustomer(Customer customer);
        void UpdateDrone(Drone drone);
        void UpdateDroneCharge(DroneCharge droneCharge);
        void UpdateParcel(Parcel parcrl);
        double[] RequetPowerConsumption();
        void MinusDroneCharge(int stationId);
        void PlusDroneCharge(int stationId);


    }
}