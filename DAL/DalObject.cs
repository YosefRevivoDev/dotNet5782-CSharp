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

        /// <summary>
        /// AddDrone
        /// </summary>
        /// <param name="newDrone"></param>
        public void AddDrone(Drone newDrone)
        {
            DataSource.Drones.Add((DataSource.Drones.FindIndex(i => i.DroneID == newDrone.DroneID) == -1 ?
                newDrone : throw new DroneException($"This id {newDrone.DroneID} already exist")));
        }

        /// <summary>
        /// AddCustomer
        /// </summary>
        /// <param name="newCustomer"></param>
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

        /// <summary>
        /// AddDroneCharge
        /// </summary>
        /// <param name="droneCharge"></param>
        public void AddDroneCharge(DroneCharge droneCharge)
        {
            DataSource.DroneCharges.Add(droneCharge);
        }


        /// <summary>
        /// AddUser
        /// </summary>
        /// <param name="newUser"></param>
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
        public DroneCharge GetDroneChargeByStation(int baseStationId)
        {
            DroneCharge drone = DataSource.DroneCharges.Find(d => d.StationID == baseStationId);
            return drone.StationID != default ? drone : throw new CheckIfIdNotException("he Station have not exists");
            //int index = DataSource.DroneCharges.ToList().FindIndex(i => i.StationID == baseStationId);
            //if (index != -1)
            //{
            //    throw new Exception("The Station have not exists");
            //}
            // AddDroneCharge(new DroneCharge {StationID = baseStationId });
        }
        public DroneCharge GetDroneChargeByDrone(int droneId)
        {
            int index = DataSource.DroneCharges.ToList().FindIndex(i => i.DroneID == droneId);

            DroneCharge droneCharge = DataSource.DroneCharges.Find(i => i.DroneID == droneId);
            return droneCharge.StationID != default ? droneCharge : throw new CheckIfIdNotException("sorry, this Drone is not found.");

        }
        #endregion
        //Set Drone For Parcel by ID
        #region Operations of drone
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool SetDroneForParcel(int parcelId, int droneId)
        {
            int index = DataSource.Parcels.ToList().FindIndex(i => i.ParcelId == parcelId);
            Parcel parcel = DataSource.Parcels[index];
            int droneIndex = DataSource.Drones.FindIndex(i => i.DroneID == droneId);
            if (index != -1 && droneIndex != -1)
            {
                parcel.DroneId = droneId;
                parcel.Assignment = DateTime.Now;
                DataSource.Parcels[index] = parcel;
                return true;
            }
            else
            {
                throw new CheckIdException("There is no match between the package and the drone");
            }
        }
        /// <summary>
        /// check the time of parcel's pick up to collection parcel
        /// </summary>
        /// <param name="parcelId"></param>
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


        /// <summary>
        /// check the time of parcel's deliver up to deliver parcel
        /// </summary>
        /// <param name="parcelId"></param>
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


        /// <summary>
        /// ReleasingChargeDrone by remove from chargeSlot's list 
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="baseStationId"></param>
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


        /// <summary>
        /// remove one AvailableChargeSlots from all AvailableChargeSlots 
        /// </summary>
        /// <param name="stationId"></param>
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

        /// <summary>
        /// add one AvailableChargeSlots from all AvailableChargeSlots 
        /// </summary>
        /// <param name="stationId"></param>
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

     /// <summary>
     /// initial power consuption for drone 
     /// </summary>
     /// <returns></returns>
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
        /// <summary>
        /// RemoveDrone by droneID
        /// </summary>
        /// <param name="droneID"></param>
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


        /// <summary>
        /// RemoveCustomer by customerId
        /// </summary>
        /// <param name="customerId"></param>
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


        /// <summary>
        /// RemoveParcel by parcelId
        /// </summary>
        /// <param name="parcelId"></param>
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


        /// <summary>
        /// Remove BaseStation by stationID
        /// </summary>
        /// <param name="stationID"></param>
        public void RemoveBaseStation(int stationID)
        {
            int index = DataSource.Stations.FindIndex(i => i.StationID == stationID);
            if (index == -1)
            {
                throw new Exception($"This station have not exist, Please try again.");
            }
            DataSource.Stations.RemoveAt(index);
        }


        /// <summary>
        /// Remove User by UserID
        /// </summary>
        /// <param name="UserID"></param>
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
        /// <summary>
        /// Update BaseStation by baseStation
        /// </summary>
        /// <param name="baseStation"></param>
        public void UpdateBaseStation(BaseStation baseStation)
        {
            int index = DataSource.Stations.FindIndex(i => i.StationID == baseStation.StationID);
            if (index == -1)
            {
                throw new Exception($"This BaseStation have not exist, Please Try again.");
            }
            DataSource.Stations[index] = baseStation;
        }


        /// <summary>
        /// Update Drone by drone obj
        /// </summary>
        /// <param name="drone"></param>
        public void UpdateDrone(Drone drone)
        {
            int index = DataSource.Drones.FindIndex(i => i.DroneID == drone.DroneID);
            if (index == -1)
            {
                throw new Exception($"This Drone have not exist, Please try again.");
            }
            DataSource.Drones[index] = drone;
        }


        /// <summary>
        /// Update Customer by customer obj 
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(Customer customer)
        {
            int index = DataSource.Customer.FindIndex(i => i.CustomerId == customer.CustomerId);
            if (index == -1)
            {
                throw new Exception($"This Customer have not exist, Please try again.");
            }
            DataSource.Customer[index] = customer;
        }


        /// <summary>
        /// Update Parcel by parcel obj 
        /// </summary>
        /// <param name="parcel"></param>
        public void UpdateParcel(Parcel parcel)
        {
            int index = DataSource.Parcels.FindIndex(i => i.ParcelId == parcel.ParcelId);
            if (index == -1)
            {
                throw new Exception($"This Parcel have not exist, Please try again.");
            }
            DataSource.Parcels[index] = parcel;
        }


        /// <summary>
        /// Update DroneCharge by droneCharge obj 
        /// </summary>
        /// <param name="droneCharge"></param>
        public void UpdateDroneCharge(DroneCharge droneCharge)
        {
            int index = DataSource.DroneCharges.FindIndex(i => i.DroneID == droneCharge.DroneID);
            if (index == -1)
            {
                throw new Exception($"This DroneCharge have not exist, Please try again.");
            }
            DataSource.DroneCharges[index] = droneCharge;
        }

        /// <summary>
        /// Update User by user obj 
        /// </summary>
        /// <param name="user"></param>
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

        /// <summary>
        /// return list by prdicate to display
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// 
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
    }
}


