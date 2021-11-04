using System;
using System.Collections.Generic;
using IDAL.DO;

namespace IDAL
{
    public interface IDal
    {
        void Add_BaseStation(BaseStation new_baseStation);
        void Add_Customer(Customer new_customer);
        void Add_Drone(Drone new_drone);
        void Add_DroneCharge(DroneCharge droneCharge);
        void Add_Parcel(Parcel new_parcel);
        void ChargeDrone(int droneId, int baseStationId);
        void DeliveredPackageToCustumer(int parcelId, int droneId);
        BaseStation GetBaseStation(int Id);
        IEnumerable<BaseStation> GetBaseStationByPredicate(Predicate<BaseStation> predicate = null);
        Customer GetCustomer(int Id);
        IEnumerable<Customer> GetCustomersByPredicate(Predicate<Customer> predicate = null);
        Drone GetDrone(int Id);
        IEnumerable<Drone> GetDronesByPredicate(Predicate<Drone> predicate = null);
        IEnumerable<Parcel> GetPackagesByPredicate(Predicate<Parcel> predicate = null);
        Parcel GetParcel(int Id);
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
    }
}