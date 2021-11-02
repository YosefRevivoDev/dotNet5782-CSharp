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
                new_drone : throw new Exception($"This id {new_drone.DroneID} already exist"));
        }

        public void Add_Customer(Customer new_customer)
        {
            DataSource.Clients.Add(DataSource.Clients.FindIndex(i => i.CustomerId == new_customer.CustomerId) == -1 ?
            new_customer : throw new Exception($"This id {new_customer.CustomerId} already exist"));
        }

        public void Add_Parcel(Parcel new_parcel)
        {
            DataSource.Packages.Add(DataSource.Packages.FindIndex(i => i.ParcelId == new_parcel.ParcelId) == -1 ?
            new_parcel : throw new Exception($"This id {new_parcel.ParcelId} already exist"));
        }
        public void Add_DroneCharge(DroneCharge droneCharge)
        {
            DataSource.DroneCharges.Add(DataSource.DroneCharges.FindIndex(i => i.StationID == droneCharge.StationID) == -1 ?
            droneCharge : throw new Exception($"This id {droneCharge.StationID} already exist"));
        }

        //-------------------------------------------RETURN OBJ BY ID (GET FUNCTION)----------------------------------------//

        public Drone GetDrone(int Id)
        {
            for (int i = 0; i < DataSource.Drones.Count; i++)
            {
                if (Id == DataSource.Drones[i].DroneID)
                {
                    return DataSource.Drones[i];
                }
            }
            return default;
        }
        public BaseStation GetBaseStation(int Id)
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
        public Parcel GetParcel(int Id)
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
            int index = DataSource.Packages.ToList().FindIndex(i => i.ParcelId == parcelId);
            Parcel parcel = DataSource.Packages[index];
            parcel.DroneId = droneId;
            DataSource.Packages[index] = parcel;
        }

        public void PackageCollectionByDrone(int parcelId, int droneId)
        {
            int index = DataSource.Packages.ToList().FindIndex(i => i.ParcelId == parcelId);
            Parcel parcel = DataSource.Packages[index];
            parcel.DroneId = droneId;
            parcel.PickedUp = DateTime.Now;
            DataSource.Packages[index] = parcel;
        }

        public void DeliveredPackageToCustumer(int parcelId, int droneId)
        {

            int index = DataSource.Packages.ToList().FindIndex(i => i.ParcelId == parcelId);
            Parcel parcel = DataSource.Packages[index];
            parcel.DroneId = droneId;
            parcel.Delivered = DateTime.Now;
            DataSource.Packages[index] = parcel;
        }


        public void ChargeDrone(int droneId, int baseStationId)
        {
            //DataSource.Drones[DataSource.Drones.ToList().
            //FindIndex(i => i.DroneID == droneId)].status = DroneStatus.maintenance;

            int index = DataSource.DroneCharges.ToList().FindIndex(i => i.StationID == baseStationId);
            DroneCharge droneCharge = DataSource.DroneCharges[index];
            droneCharge.DroneID = droneId;
            DataSource.DroneCharges[index] = droneCharge;

            //Add_DroneCharge(new DroneCharge { DroneID = droneId, StationID = baseStationId });
        }

        public void ReleasingChargeDrone(int droneId, int baseStationId)
        {

            DataSource.DroneCharges.RemoveAt(DataSource.DroneCharges.FindIndex(i => i.DroneID == droneId
                        && i.StationID == baseStationId));

            //FindIndex(i => i.DroneID == droneId)].status = DroneStatus.available;
            //DataSource.Stations[DataSource.Stations.ToList().
            //FindIndex(i => i.StationID == baseStationId)].ChargeSlots++;
        }


        ///////////////////// Remove Object From Function //////////////////


        public void RemoveDrone(int droneID)
        {
            DataSource.Drones.RemoveAt(DataSource.Drones.FindIndex(i => i.DroneID == droneID));
        }

        public void RemoveCustomer(int customerId)
        {
            DataSource.Clients.RemoveAt(DataSource.Clients.FindIndex(i => i.CustomerId == customerId));
        }

        public void RemoveParcel(int parcelId)
        {
            DataSource.Packages.RemoveAt(DataSource.Packages.FindIndex(i => i.ParcelId == parcelId));
        }

        public void RemoveBaseStation(int stationID)
        {
            DataSource.Stations.RemoveAt(DataSource.Stations.FindIndex(i => i.StationID == stationID));
        }

        // ------------------------ Update Object in Function -------------------------------//
        public void UpdateBaseStation(BaseStation baseStation)
        {
            int index = DataSource.Stations.FindIndex(i => i.StationID == baseStation.StationID);
            if (index == -1)
            {
                throw new Exception($"This BaseStation have not exist, Please Try again.");
            }
            DataSource.Stations[index] = baseStation;

        }

        public void UpdateDrone(Drone drone)
        {
            int index = DataSource.Drones.FindIndex(i => i.DroneID == drone.DroneID);
            if (index == -1)
            {
                throw new Exception($"This Drone have not exist, Please try again.");
            }
            DataSource.Drones[index] = drone;
        }

        public void UpdateCustomer(Customer customer)
        {
            int index = DataSource.Clients.FindIndex(i => i.CustomerId == customer.CustomerId);
            if (index == -1)
            {
                throw new Exception($"This Customer have not exist, Please try again.");
            }
            DataSource.Clients[index] = customer;
        }

        public void UpdateParcel(Parcel parcrl)
        {
            int index = DataSource.Packages.FindIndex(i => i.ParcelId == parcrl.ParcelId);
            if (index == -1)
            {
                throw new Exception($"This Parcel have not exist, Please try again.");
            }
            DataSource.Packages[index] = parcrl;
        }

        public void UpdateDroneCharge(DroneCharge droneCharge)
        {
            int index = DataSource.DroneCharges.FindIndex(i => i.DroneID == droneCharge.DroneID);
            if (index == -1)
            {
                throw new Exception($"This DroneCharge have not exist, Please try again.");
            }
            DataSource.DroneCharges[index] = droneCharge;
        }



        //--------------------------------------------------DISPLAY FUNCTION---------------------------------------//

        // Gets a list and sends a copy to  generic func in Main prog.

        public IEnumerable<Drone> GetDronesByPredicate(Predicate<Drone> predicate = null)
        {
            return DataSource.Drones.TakeWhile(i => predicate == null ? true : predicate(i));
        }
        public IEnumerable<Customer> GetCustomersByPredicate(Predicate<Customer> predicate = null)
        {
            return DataSource.Clients.TakeWhile(i => predicate == null ? true : predicate(i));
        }
        public IEnumerable<Parcel> GetPackagesByPredicate(Predicate<Parcel> predicate = null)
        {
            return DataSource.Packages.TakeWhile(i => predicate == null ? true : predicate(i));
        }
        public IEnumerable<BaseStation> GetBaseStationByPredicate(Predicate<BaseStation> predicate = null)
        {
            return DataSource.Stations.TakeWhile(i => predicate == null ? true : predicate(i));
        }
    }
}


