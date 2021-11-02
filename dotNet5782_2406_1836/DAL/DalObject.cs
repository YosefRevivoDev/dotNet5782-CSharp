//using DAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using DalObject;
using static DalObject.DataSource;

namespace DalObject
{
    public class DalObject
    {
        public DalObject() { DataSource.Initialize(); }
        //----------------------------------------ADD FUNCTIONS---------------------------------------//
        /// <summary>
        /// Checks if the object the user is looking for is in the list, then throws an error if it does not add to the list 
        /// </summary>
        /// <param name="new_baseStation"></param>
        public void Add_BaseStation(BaseStation new_baseStation)
        {

            DataSource.Stations.Add(new_baseStation);
        }

        public void Add_Drone(Drone new_drone)
        {
            DataSource.Drones.Add(DataSource.Drones.FindIndex(i => i.DroneID == new_drone.DroneID) == -1 ?
                new_drone : throw new DroneException($"This id {new_drone.DroneID} already exist"));
        }

        public  void Add_Customer(Customer new_customer)
        {
            DataSource.Clients.Add(DataSource.Clients.FindIndex (i => i.CustomerId == new_customer.CustomerId) == -1 ?
            new_customer : throw new DroneException($"This id {new_customer.CustomerId} already exist"));
        }

        public void Add_Parcel(Parcel new_parcel)
        {
            DataSource.Packages.Add( DataSource.Packages.FindIndex(i => i.ParcelId == new_parcel.ParcelId) == -1 ?
            new_parcel : throw new DroneException($"This id {new_parcel.ParcelId} already exist"));
        }
        public void Add_DroneCharge(DroneCharge droneCharge)
        {
            DataSource.DroneCharges.Add(DataSource.DroneCharges.FindIndex(i => i.StationID == droneCharge.StationID) == -1 ?
            droneCharge : throw new DroneException($"This id {droneCharge.StationID} already exist"));
        }

        //-------------------------------------------RETURN OBJ BY ID (GET FUNCTION)----------------------------------------//

        public Drone GetDrone (int Id)
        {
            for (int i = 0; i < DataSource.Drones.Count ; i++)
            {
                if (Id == DataSource.Drones[i].DroneID)
                {
                    return DataSource.Drones[i];
                }
            }
            return default;
        }
        public BaseStation GetBaseStation (int Id)
        {
            for (int i = 0; i < DataSource.Stations.Count; i++)
            {
                if (Id == DataSource.Stations[i].StationID)
                {
                    return DataSource.Stations[i];
                }
            }
            return default;
        }
        public Customer GetCustomer(int Id)
        {
            for (int i = 0; i < DataSource.Clients.Count; i++)
            {
                if (Id == DataSource.Clients[i].CustomerId)
                {
                    return DataSource.Clients[i];
                }
            }
            return default;
        }
        public Parcel GetParcel (int Id)
        {
            for (int i = 0; i < DataSource.Packages.Count; i++)
            {
                if (Id == DataSource.Packages[i].ParcelId)
                {
                    return DataSource.Packages[i];
                }
            }
            return default;
        }

        //--------------------------------------------------UPDATE FUNCTION---------------------------------------//
        /// <summary>
        ///  הגדרת הפונקציות העדכון הבאות לפי הסדר:
        ///  שיוך חבילה לרחפן
        /// איסוף החבילה על ידי רחפן    
        /// שליחת החבילה ללקוח
        /// טעינת רחפן 
        /// שחרור רחפן מטעינה
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void SetDroneForParcel(int parcelId, int droneId)
        {
            for (int i = 0; i < Packages.Count; i++)
            {
                if(Packages[i].ParcelId == parcelId)
                {
                    Packages[i].DroneId = droneId;
                    return;
                }
            }
        }
               
        public void PackageCollectionByDrone(int parcelId, int droneId)
        { 
            int index = DataSource.Packages.ToList().
                FindIndex(i => i.ParcelId == parcelId);
            DataSource.Packages[index].DroneId = droneId;
            DataSource.Packages[index].PickedUp = DateTime.Now;

            DataSource.Drones[DataSource.Drones.
             FindIndex(i => i.DroneID == droneId)].status = DroneStatus.busy;
        }

        public void DeliveredPackageToCustumer(int parcelId, int droneId)
        {
            int index = DataSource.Packages.ToList().
              FindIndex(i => i.ParcelId == parcelId);
            DataSource.Packages[index].DroneId = 0;
            DataSource.Packages[index].Delivered = DateTime.Now;

            DataSource.Drones[DataSource.Drones.ToList().
             FindIndex(i => i.DroneID == droneId)].status = DroneStatus.available;
        }

        public void ChargeDrone(int droneId, int baseStationId)
        {
            DataSource.Drones[DataSource.Drones.ToList().
              FindIndex(i => i.DroneID == droneId)].status = DroneStatus.maintenance;

            DataSource.Stations[DataSource.Stations.ToList().
           FindIndex(i => i.StationID == baseStationId)].ChargeSlots--; 
            Add_DroneCharge(new DroneCharge { DroneID = droneId, StationID = baseStationId });
        }

        public void ReleasingChargeDrone(int droneId, int baseStationId)
        {
            DataSource.DroneCharges.RemoveAt(DataSource.DroneCharges.
                FindIndex(i => i.DroneID == droneId
                && i.StationID == baseStationId));

            DataSource.Drones[DataSource.Drones.ToList().
              FindIndex(i => i.DroneID == droneId)].status = DroneStatus.available;

            DataSource.Stations[DataSource.Stations.ToList().
           FindIndex(i => i.StationID == baseStationId)].ChargeSlots++;
        }
         

        //--------------------------------------------------DISPLAY FUNCTION---------------------------------------//
     
        // Gets a list and sends a copy to  generic func in Main prog.

        public IEnumerable<Drone> GetDronesByPredicate(Predicate<Drone> predicate)
        {
            return DataSource.Drones.TakeWhile(i => predicate(i));
        }
        public IEnumerable<Customer> GetCustomersByPredicate(Predicate<Customer>predicate)
        {
            return DataSource.Clients.TakeWhile(i => predicate(i));
        }
        public IEnumerable<Parcel> GetPackagesByPredicate(Predicate<Parcel> predicate)
        {
            return DataSource.Packages.TakeWhile(i => predicate(i));
        }
        public IEnumerable<BaseStation> GetBaseStationByPredicate(Predicate<BaseStation> predicate)
        {
            return DataSource.Stations.TakeWhile(i => predicate(i));
            // dal.GetBaseStationByPredicate(i => i.state == free)
        }
    }
}


