using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;

namespace DalObject
{
    public class DalObject : IDal
    {
        public DalObject() { DataSource.Initialize(); }
        //----------------------------------------ADD FUNCTIONS---------------------------------------//
        /// <summary>
        /// Checks if the object the user is looking for is in the list, then throws an error if it does not add to the list 
        /// </summary>
        /// <param name="newBaseStation"></param>
        public void AddBaseStation(BaseStation newBaseStation)
        {
            DataSource.Stations.Add(DataSource.Stations.FindIndex(i => i.StationID == newBaseStation.StationID) == -1 ?
                newBaseStation : throw new BaseStationException($"This id{newBaseStation.StationID}already exist"));
        }

        public void AddDrone(Drone newDrone)
        {
            DataSource.Drones.Add((Drone)(DataSource.Drones.FindIndex(i => i.DroneID == newDrone.DroneID) == -1 ?
                newDrone : throw new DroneException($"This id {newDrone.DroneID} already exist")));
        }

        public void AddCustomer(Customer newCustomer)
        {
            DataSource.Clients.Add(DataSource.Clients.FindIndex(i => i.CustomerId == newCustomer.CustomerId) == -1 ?
            newCustomer : throw new CustumerException($"This id {newCustomer.CustomerId} already exist"));
        }

        public void AddParcel(Parcel newParcel)
        {
            DataSource.Packages.Add(DataSource.Packages.FindIndex(i => i.ParcelId == newParcel.ParcelId) == -1 ?
            newParcel : throw new ParcelException($"This id {newParcel.ParcelId} already exist"));
        }
        public void AddDroneCharge(DroneCharge droneCharge)
        {
            DataSource.DroneCharges.Add((DataSource.DroneCharges.FindIndex(i => i.StationID == droneCharge.StationID) == -1 ?
            droneCharge : throw new DroneChargeException($"This id {droneCharge.StationID} already exist")));
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

        public User GetUser(string userName)
        {
            for (int i = 0; i < DataSource.users.Count; i++)
            {
                if (userName == DataSource.users[i].UserName)
                {
                    return DataSource.users[i];
                }
            }
            throw new Exception("The User not exist");
        }

        //--------------------------------------------------UPDATE FUNCTION---------------------------------------//
        /// <summary>
        /// Set the following update functions in order:
        /// Assigning a package to a glider
        /// Picking up the package by skimmer
        /// Sending the package to the customer
        /// Charging skimmer
        /// Release skimmer from charging
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void SetDroneForParcel(int parcelId, int droneId)
        {
            int index = DataSource.Packages.ToList().FindIndex(i => i.ParcelId == parcelId);
            Parcel parcel = DataSource.Packages[index];
            parcel.DroneId = droneId;
            parcel.PickedUp = DateTime.Now;
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

            int index = DataSource.DroneCharges.ToList().FindIndex(i => i.DroneID == droneId);
            if (index != -1)
            {
                throw new Exception("The charge Drone have not exists");
            }
            AddDroneCharge(new DroneCharge { DroneID = droneId, StationID = baseStationId });
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
            int index = DataSource.DroneCharges.FindIndex(i => i.DroneID == droneID);
            DataSource.DroneCharges.RemoveAt(DataSource.DroneCharges.FindIndex(i => i.DroneID == droneID));
            if (index == -1)
            {
                throw new Exception($"This drone have not exist, Please try again.");
            }
            DataSource.Drones.RemoveAt(index);
            //DataSource.Drones.RemoveAt(DataSource.Drones.FindIndex(i => i.DroneID == droneID));
        }

        public void RemoveCustomer(int customerId)
        {
            int index = DataSource.Clients.FindIndex(i => i.CustomerId == customerId);
            DataSource.Clients.RemoveAt(DataSource.Clients.FindIndex(i => i.CustomerId == customerId));
            if (index == -1)
            {
                throw new Exception($"This customer have not exist, Please try again.");
            }
            DataSource.Clients.RemoveAt(index);
        }

        public void RemoveParcel(int parcelId)
        {
            int index = DataSource.Packages.FindIndex(i => i.ParcelId == parcelId);
            DataSource.Packages.RemoveAt(DataSource.Packages.FindIndex(i => i.ParcelId == parcelId));
            if (index == -1)
            {
                throw new Exception($"This parcel have not exist, Please try again.");
            }
            DataSource.Packages.RemoveAt(index);
        }

        public void RemoveBaseStation(int stationID)
        {
            int index = DataSource.Stations.FindIndex(i => i.StationID == stationID);
            if (index == -1)
            {
                throw new Exception($"This station have not exist, Please try again.");
            }
            DataSource.Stations.RemoveAt(index);
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

        public void UpdateParcel(Parcel parcel)
        {
            int index = DataSource.Packages.FindIndex(i => i.ParcelId == parcel.ParcelId);
            if (index == -1)
            {
                throw new Exception($"This Parcel have not exist, Please try again.");
            }
            DataSource.Packages[index] = parcel;
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
        public void MinusDroneCharge(int stationId)
        {
            BaseStation station = DataSource.Stations.Find(x => x.StationID == stationId);
            DataSource.Stations.Remove(station);
            station.AvailableChargeSlots--;
            DataSource.Stations.Add(station);
        }

        public void PlusDroneCharge(int stationId)
        {
            BaseStation station = DataSource.Stations.Find(x => x.StationID == stationId);
            DataSource.Stations.Remove(station);
            station.AvailableChargeSlots++;
            DataSource.Stations.Add(station);
        }


        //--------------------------------------------------DISPLAY FUNCTION---------------------------------------//

        // Gets a list and sends a copy to  generic func in Main prog.

        public IEnumerable<Drone> GetDronesByPredicate(Predicate<Drone> predicate = null)
        { 
            return DataSource.Drones.FindAll(i => predicate == null ? true : predicate(i));
        }
        public IEnumerable<Customer> GetCustomersByPredicate(Predicate<Customer> predicate = null)
        {
            return DataSource.Clients.FindAll(i => predicate == null ? true : predicate(i));
        }
        public IEnumerable<Parcel> GetPackagesByPredicate(Predicate<Parcel> predicate = null)
        {
            return DataSource.Packages.FindAll(i => predicate == null ? true : predicate(i));
        }
        public IEnumerable<BaseStation> GetBaseStationByPredicate(Predicate<BaseStation> predicate = null)
        {
            return DataSource.Stations.FindAll(i => predicate == null ? true : predicate(i));
        }
        public IEnumerable<DroneCharge> GetDroneChargesByPredicate(Predicate<DroneCharge>predicate = null)
        {
            return DataSource.DroneCharges.FindAll(i => predicate == null ? true : predicate(i));
        }
       

        // Returns an array of power consumption per mile
        public double[] RequetPowerConsumption()
        {
            double[] arr =
           {
                DataSource.Config.PowerConsumptionAvailable ,
                DataSource.Config.PowerConsumptionHeavyWeight ,
                DataSource.Config.PowerConsumptionMediumWeight ,
                DataSource.Config.PowerConsumptionHeavyWeight,
                DataSource.Config.LoadingDrone
            };
            return arr;
        }
    }

}


