using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using DO;
using DS;
using DalApi;

namespace DAL
{
    sealed class DalObject : IDal
    {
        #region Singleton
        static readonly IDal instance = new DalObject();
        public static IDal Instance { get => instance; }
        DalObject() { }
        #endregion

        [MethodImpl(MethodImplOptions.Synchronized)]
        
        #region Add object
        public void AddBaseStation(BaseStation newBaseStation)
        {
            DataSource.Stations.Add(DataSource.Stations.FindIndex(i => i.StationID == newBaseStation.StationID) == -1 ?
                newBaseStation : throw new BaseStationException($"This id{newBaseStation.StationID}already exist"));
        }

        public void AddDrone(Drone newDrone)
        {
            DataSource.Drones.Add((DataSource.Drones.FindIndex(i => i.DroneID == newDrone.DroneID) == -1 ?
                newDrone : throw new DroneException($"This id {newDrone.DroneID} already exist")));
        }

        public void AddCustomer(Customer newCustomer)
        {
            DataSource.Customer.Add(DataSource.Customer.FindIndex(i => i.CustomerId == newCustomer.CustomerId) == -1 ?
            newCustomer : throw new CustumerException($"This id {newCustomer.CustomerId} already exist"));
        }

        public int AddParcel(Parcel newParcel)
        {
            DataSource.Parcels.Add(DataSource.Parcels.FindIndex(i => i.ParcelId == newParcel.ParcelId) == -1 ?
            newParcel : throw new ParcelException($"This id {newParcel.ParcelId} already exist"));
            return 0;
        }

        public void AddDroneCharge(DroneCharge droneCharge)
        {
            DataSource.DroneCharges.Add(droneCharge);
            //    DataSource.DroneCharges.Add(DataSource.DroneCharges.FindIndex(i => i.StationID == droneCharge.StationID) == -1 ?
            //    droneCharge : throw new DroneChargeException($"This id {droneCharge.StationID} already exist"));
            //
        }

            public void AddUser(User newUser)
        {
            DataSource.users.Add((DataSource.users.FindIndex(i => i.UserId == newUser.UserId) == -1 ?
                newUser : throw new UserException($"This id {newUser.UserId} already exist")));
        }
        #endregion

        #region Get Object
        public Drone GetDrone(int Id)
        {
            Drone drone = DataSource.Drones.Find(i => i.DroneID == Id);
            return drone.DroneID != default ? drone : throw new DroneException("Drone not found");
        }
        public BaseStation GetBaseStation(int Id)
        {
            BaseStation station = DataSource.Stations.Find(i => i.StationID == Id);
            return station.StationID != default ? station : throw new BaseStationException("Station not found");
        }
        public Customer GetCustomer(int Id)
        {
            Customer customer = DataSource.Customer.Find(i => i.CustomerId == Id);
            return customer.CustomerId != default ? customer : throw new CustumerException("Customer not found");
        }
        public Parcel GetParcel(int Id)
        {
            Parcel parcel = DataSource.Parcels.Find(i => i.ParcelId == Id);
            return parcel.ParcelId != default ? parcel : throw new ParcelException("Parcel not found");
        }
        public User GetUser(string userId)
        {
            User user = DataSource.users.Find(i => i.UserId == userId);
            return user.UserId != default ? user : throw new UserException("User not found");
        }
        public void GetDroneChargeByStation(int baseStationId)
        {
            int index = DataSource.DroneCharges.ToList().FindIndex(i => i.StationID == baseStationId);
            if (index != -1)
            {
                throw new Exception("The Station have not exists");
            }
            AddDroneCharge(new DroneCharge {StationID = baseStationId });
        }
        public DroneCharge GetDroneChargeByDrone(int droneId)
        {
            int index = DataSource.DroneCharges.ToList().FindIndex(i => i.DroneID == droneId);

            DroneCharge droneCharge = DataSource.DroneCharges.Find(i => i.DroneID == droneId);
            return droneCharge.StationID != default ? droneCharge : throw new CheckIfIdNotException("sorry, this Drone is not found.");

        }
        #endregion

        #region Operations of drone
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetDroneForParcel(int parcelId, int droneId)
        {
            int index = DataSource.Parcels.ToList().FindIndex(i => i.ParcelId == parcelId);
            Parcel parcel = DataSource.Parcels[index];
            int droneIndex = DataSource.Drones.FindIndex(i => i.DroneID == droneId);
            if (index != -1 && droneIndex != -1)
            {
                parcel.DroneId = droneId;
                parcel.PickedUp = DateTime.Now;
                DataSource.Parcels[index] = parcel;
            }
            else
            {
                throw new CheckIdException("There is no match between the package and the drone");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PackageCollectionByDrone(int parcelId)
        {
            int index = DataSource.Parcels.ToList().FindIndex(i => i.ParcelId == parcelId);
            Parcel parcel = DataSource.Parcels[index];
            if(index != -1)
            {
                parcel.PickedUp = DateTime.Now;
                DataSource.Parcels[index] = parcel;
            }
            else
            {
                throw new CheckIdException("Parcel not found");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool DeliveredPackageToCustumer(int parcelId)
        {
            int index = DataSource.Parcels.ToList().FindIndex(i => i.ParcelId == parcelId);
            Parcel parcel = DataSource.Parcels[index];
            if (index != -1)
            {
                parcel.Delivered = DateTime.Now;
                DataSource.Parcels[index] = parcel;
                return true;
            }
            else
            {
                throw new CheckIdException("Parcel not found");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleasingChargeDrone(int droneId, int baseStationId)
        {
            int index = DataSource.DroneCharges.FindIndex(i => i.DroneID == droneId && i.StationID == baseStationId);
            if (index != -1)
            {
                DataSource.DroneCharges.RemoveAt(index);
            }
            else
            {
                throw new CheckIdException("Drone not found");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void MinusDroneCharge(int stationId)
        {
            BaseStation station = DataSource.Stations.Find(x => x.StationID == stationId);
            int index = DataSource.Stations.FindIndex(i => i.StationID == stationId);
            if (index != -1)
            {
                station.AvailableChargeSlots--;
                DataSource.Stations[index] = station;
            }
            else
            {
                throw new CheckIdException("Station not found");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PlusDroneCharge(int stationId)
        {
            BaseStation station = DataSource.Stations.Find(x => x.StationID == stationId);
            int index = DataSource.Stations.FindIndex(i => i.StationID == stationId);
            if (index != -1)
            {
                station.AvailableChargeSlots++;
                DataSource.Stations[index] = station;
            }
            else
            {
                throw new CheckIdException("Station not found");
            }
        }

        public double[] RequetPowerConsumption()
        {
            double[] arr = new double[]
           {
                DataSource.Config.PowerConsumptionAvailable ,
                DataSource.Config.PowerConsumptionHeavyWeight ,
                DataSource.Config.PowerConsumptionMediumWeight ,
                DataSource.Config.PowerConsumptionHeavyWeight,
                DataSource.Config.LoadingDrone
            };
            return arr;
        }
        #endregion

        #region Remove Object

        public void RemoveDrone(int droneID)
        {
            int index = DataSource.DroneCharges.FindIndex(i => i.DroneID == droneID);
            DataSource.DroneCharges.RemoveAt(DataSource.DroneCharges.FindIndex(i => i.DroneID == droneID));
            if (index == -1)
            {
                throw new Exception($"This drone have not exist, Please try again.");
            }
            DataSource.Drones.RemoveAt(index);
        }

        public void RemoveCustomer(int customerId)
        {
            int index = DataSource.Customer.FindIndex(i => i.CustomerId == customerId);
            DataSource.Customer.RemoveAt(DataSource.Customer.FindIndex(i => i.CustomerId == customerId));
            if (index == -1)
            {
                throw new Exception($"This customer have not exist, Please try again.");
            }
            DataSource.Customer.RemoveAt(index);
        }

        public void RemoveParcel(int parcelId)
        {
            int index = DataSource.Parcels.FindIndex(i => i.ParcelId == parcelId);
            DataSource.Parcels.RemoveAt(DataSource.Parcels.FindIndex(i => i.ParcelId == parcelId));
            if (index == -1)
            {
                throw new Exception($"This parcel have not exist, Please try again.");
            }
            DataSource.Parcels.RemoveAt(index);
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

        public void RemoveUser (string UserID)
        {
            int index = DataSource.users.FindIndex(i => i.UserId == UserID);
            DataSource.users.RemoveAt(DataSource.users.FindIndex(i => i.UserId == UserID));
            if (index == -1)
            {
                throw new Exception($"This User have not exist, Please try again.");
            }
            DataSource.users.RemoveAt(index);
        }

        #endregion

        #region Update object
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
            int index = DataSource.Customer.FindIndex(i => i.CustomerId == customer.CustomerId);
            if (index == -1)
            {
                throw new Exception($"This Customer have not exist, Please try again.");
            }
            DataSource.Customer[index] = customer;
        }

        public void UpdateParcel(Parcel parcel)
        {
            int index = DataSource.Parcels.FindIndex(i => i.ParcelId == parcel.ParcelId);
            if (index == -1)
            {
                throw new Exception($"This Parcel have not exist, Please try again.");
            }
            DataSource.Parcels[index] = parcel;
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

        public void UpdateUser(User user)
        {
            int index = DataSource.users.FindIndex(i => i.UserId == user.UserId);
            if (index == -1)
            {
                throw new Exception($"This User have not exist, Please try again.");
            }
            DataSource.users[index] = user;
        }
        #endregion

        #region Display lists
        public IEnumerable<Drone> GetDronesByPredicate(Predicate<Drone> predicate = null)
        {
            return DataSource.Drones.FindAll(i => predicate == null ? true : predicate(i));
        }
        public IEnumerable<Customer> GetCustomersByPredicate(Predicate<Customer> predicate = null)
        {
            return DataSource.Customer.FindAll(i => predicate == null ? true : predicate(i));
        }
        public IEnumerable<Parcel> GetPackagesByPredicate(Predicate<Parcel> predicate = null)
        {
            return DataSource.Parcels.FindAll(i => predicate == null ? true : predicate(i));
        }
        public IEnumerable<BaseStation> GetBaseStationByPredicate(Predicate<BaseStation> predicate = null)
        {
            return DataSource.Stations.FindAll(i => predicate == null ? true : predicate(i));
        }
        public IEnumerable<DroneCharge> GetDroneChargesByPredicate(Predicate<DroneCharge> predicate = null)
        {
            return DataSource.DroneCharges.FindAll(i => predicate == null ? true : predicate(i));
        }
        #endregion

        // Returns an array of power consumption per mile

    }
}


