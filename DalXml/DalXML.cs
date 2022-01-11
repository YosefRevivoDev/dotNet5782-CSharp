using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

using DalApi;
using DAL;
using DO;
using System.Globalization;


namespace DAL
{
    public class DalXML : IDal
    {
        static readonly DalXML instance = new DalXML();

        public static DalXML Instance { get => instance; }

        DalXML() { }

        static DalXML() { }
        //#region Singleton
        //static readonly Lazy<DalXML> lazy = new Lazy<DalXML>(() => new DalXML());
        //public static DalXML Instance => lazy.Value;

        //private DalXML() { }
        //#endregion

        private static string FileName<T>() => typeof(T).Name + ".xml";

        string baseStationPath = @"BaseStationXml.xml";
        string dronePath = @"DroneXml.xml";
        string parcelPath = @"ParcelXml.xml";
        string customerPath = @"CustomerXml.xml";
        string droneToListPath = @"DroneToListXml.xml";


        public void AddBaseStation(BaseStation new_baseStation)
        {
            throw new NotImplementedException();
        }

        public void AddCustomer(Customer new_customer)
        {
            throw new NotImplementedException();
        }

        public void AddDrone(Drone new_drone)
        {
            throw new NotImplementedException();
        }

        public void AddDroneCharge(DroneCharge droneCharge)
        {
            throw new NotImplementedException();
        }

        public void AddParcel(Parcel new_parcel)
        {
            throw new NotImplementedException();
        }

        public void ChargeDrone(int droneId, int baseStationId)
        {
            throw new NotImplementedException();
        }

        public void DeliveredPackageToCustumer(int parcelId, int droneId)
        {
            throw new NotImplementedException();
        }

        public BaseStation GetBaseStation(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BaseStation> GetBaseStationByPredicate(Predicate<BaseStation> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetCustomersByPredicate(Predicate<Customer> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Drone GetDrone(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Drone> GetDronesByPredicate(Predicate<Drone> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> GetPackagesByPredicate(Predicate<Parcel> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DroneCharge> GetDroneChargesByPredicate(Predicate<DroneCharge> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Parcel GetParcel(int Id)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string userName)
        {
            throw new NotImplementedException();
        }

        public void PackageCollectionByDrone(int parcelId, int droneId)
        {
            throw new NotImplementedException();
        }

        public void ReleasingChargeDrone(int droneId, int baseStationId)
        {
            throw new NotImplementedException();
        }

        public void RemoveBaseStation(int stationID)
        {
            throw new NotImplementedException();
        }

        public void RemoveCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public void RemoveDrone(int droneID)
        {
            throw new NotImplementedException();
        }

        public void RemoveParcel(int parcelId)
        {
            throw new NotImplementedException();
        }

        public void SetDroneForParcel(int parcelId, int droneId)
        {
            throw new NotImplementedException();
        }

        public void UpdateBaseStation(BaseStation baseStation)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public void UpdateDrone(Drone drone)
        {
            throw new NotImplementedException();
        }

        public void UpdateDroneCharge(DroneCharge droneCharge)
        {
            throw new NotImplementedException();
        }

        public void UpdateParcel(Parcel parcrl)
        {
            throw new NotImplementedException();
        }

        public double[] RequetPowerConsumption()
        {
            throw new NotImplementedException();
        }

        public void MinusDroneCharge(int stationId)
        {
            throw new NotImplementedException();
        }

        public void PlusDroneCharge(int stationId)
        {
            throw new NotImplementedException();
        }
         

    }
}
