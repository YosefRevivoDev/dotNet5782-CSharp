using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DalApi;
using DO;
using System.Globalization;


namespace DAL
{

    sealed class DalXml : IDal
    {
        static readonly DalXml instance = new DalXml();

        public static IDal Instance { get => instance; }

        private DalXml() { }

        string baseStationPath = @"BaseStationXml.xml";
        string dronePath = @"DroneXml.xml";
        string parcelPath = @"ParcelXml.xml";
        string customerPath = @"CustomerXml.xml";
        string droneChargesPath = @"DroneChargesXml.xml";
        string userPath = @"UserXml.xml";
        string runNumbers = @"RunNumbers.xml";

        #region Drone
        public void UpdateDrone(Drone drone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);

            int index = drones.FindIndex(i => i.DroneID == drone.DroneID);
            if (index == -1)
            {
                throw new DroneException($"This Drone have not exist, Please try again.");
            }
            drones[index] = drone;
            XMLTools.SaveListToXMLSerializer(drones, dronePath);
        }
        public void AddDrone(Drone newDrone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);

            int find = drones.FindIndex(Drone => Drone.DroneID == newDrone.DroneID);
            if (find == -1)
            {
                drones.Add(newDrone);
                XMLTools.SaveListToXMLSerializer(drones, dronePath);
            }
            else
            {
                throw new CheckIdException("Sorry, i have already a drone with this id:" + newDrone.DroneID);
            }
        }
        public void RemoveDrone(int droneID)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);

            int index = drones.FindIndex(i => i.DroneID == droneID);

            if (index != -1)
            {
                drones.RemoveAt(index);
            }
            throw new DroneException($"This drone have not exist, Please try again.");

        }
        public Drone GetDrone(int Id)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);

            Drone drone = drones.Find(i => i.DroneID == Id);
            return drone.DroneID != default ? drone : throw new DroneException("Drone not found");
        }
        public IEnumerable<Drone> GetDronesByPredicate(Predicate<Drone> predicate = null)
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);

            return ListDrone.FindAll(i => predicate == null ? true : predicate(i));
        }
        #endregion

        #region DroneCharge
        /// <summary>
        ///  This function add element to the list of drinecarge.
        /// </summary>
        /// <param name="droneCharge"></param>
        public void AddDroneCharge(DroneCharge droneCharge)
        {
            List<DroneCharge> ListdroneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargesPath);

            ListdroneCharge.Add(droneCharge);
            XMLTools.SaveListToXMLSerializer(ListdroneCharge, droneChargesPath);
        }


        /// <summary>
        /// This function returns a charging entity by ID.
        /// </summary>
        /// <param name="baseStationId"></param>
        public void GetDroneChargeByStation(int baseStationId)
        {

            List<DroneCharge> ListDroneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargesPath);

            int index = ListDroneCharge.ToList().FindIndex(i => i.StationID == baseStationId);
            if (index != -1)
            {
                throw new DroneChargeException("The Station have not exists");
            }
            AddDroneCharge(new DroneCharge { StationID = baseStationId });
        }

        /// <summary>
        /// This function returns a charging entity by ID.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public DroneCharge GetDroneChargeByDrone(int droneId)
        {
            List<DroneCharge> ListDroneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargesPath);

            DroneCharge droneCharge = ListDroneCharge.Find(i => i.DroneID == droneId);
            return droneCharge.StationID != default ? droneCharge : throw new CheckIfIdNotException("sorry, this Drone is not found.");

        }

        /// <summary>
        /// This function returns a ListDroneCharge by prdicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<DroneCharge> GetDroneChargesByPredicate(Predicate<DroneCharge> predicate = null)
        {

            List<DroneCharge> ListDroneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargesPath);

            return ListDroneCharge.FindAll(i => predicate == null ? true : predicate(i));
        }
        #endregion

        #region BaseStation

        /// <summary>
        /// This function allows the user to add a base station to the list.
        /// </summary>
        /// <param name="new_baseStation"></param>
        public void AddBaseStation(BaseStation new_baseStation)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListFromXMLSerializer<BaseStation>(baseStationPath);

            ListBaseStation.Add(ListBaseStation.FindIndex(i => i.StationID == new_baseStation.StationID) == -1 ?
                           new_baseStation : throw new BaseStationException($"This id{new_baseStation.StationID}already exist"));

            XMLTools.SaveListToXMLSerializer(ListBaseStation, baseStationPath);
        }


        /// <summary>
        /// This function deletes an object by ID
        /// </summary>
        /// <param name="stationID"></param>
        public void RemoveBaseStation(int stationID)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListFromXMLSerializer<BaseStation>(baseStationPath);
            int index = ListBaseStation.FindIndex(i => i.StationID == stationID);
            if (index == -1)
            {
                throw new BaseStationException($"This station have not exist, Please try again.");
            }
            ListBaseStation.RemoveAt(index);
        }


        /// <summary>
        /// This function update a baseStation
        /// </summary>
        /// <param name="baseStation"></param>
        public void UpdateBaseStation(BaseStation baseStation)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListFromXMLSerializer<BaseStation>(baseStationPath);
            int index = ListBaseStation.FindIndex(i => i.StationID == baseStation.StationID);
            if (index == -1)
            {
                throw new BaseStationException($"This BaseStation have not exist, Please Try again.");
            }
            ListBaseStation[index] = baseStation;
            XMLTools.SaveListToXMLSerializer(ListBaseStation, baseStationPath);
        }


        /// <summary>
        /// GetBaseStation by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public BaseStation GetBaseStation(int Id)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListFromXMLSerializer<BaseStation>(baseStationPath);

            BaseStation station = ListBaseStation.Find(i => i.StationID == Id);
            return station.StationID != default ? station : throw new BaseStationException("Station not found");
        }

        /// <summary>
        /// Return ListBaseStation by predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<BaseStation> GetBaseStationByPredicate(Predicate<BaseStation> predicate = null)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListFromXMLSerializer<BaseStation>(baseStationPath);

            return ListBaseStation.FindAll(i => predicate == null ? true : predicate(i));
        }
        #endregion

        #region Parcel
        /// <summary>
        /// This func add parcel by id.
        /// </summary>
        /// <param name="newParcel"></param>
        /// <returns></returns>
        public int AddParcel(Parcel newParcel)
        {
            XElement xElement = XMLTools.LoadListFromXMLElement(runNumbers);
            newParcel.ParcelId = int.Parse(xElement.Element("RunNumberParcel").Value) + 1;
            xElement.Element("RunNumberParcel").Value = newParcel.ParcelId.ToString();
            XMLTools.SaveListToXMLElement(xElement, runNumbers);

            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            ListParcel.Add(ListParcel.FindIndex(i => i.ParcelId == newParcel.ParcelId) == -1 ?
                           newParcel : throw new ParcelException($"This id {newParcel.ParcelId} already exist"));

            XMLTools.SaveListToXMLSerializer(ListParcel, parcelPath);

            return newParcel.ParcelId;
        }

        /// <summary>
        /// This func remove parcel by id.
        /// </summary>
        /// <param name="parcelId"></param>
        public void RemoveParcel(int parcelId)
        {
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);

            int index = ListParcel.FindIndex(i => i.ParcelId == parcelId);
            ListParcel.RemoveAt(ListParcel.FindIndex(i => i.ParcelId == parcelId));
            if (index == -1)
            {
                throw new ParcelException($"This parcel have not exist, Please try again.");
            }
            ListParcel.RemoveAt(index);
            XMLTools.SaveListToXMLSerializer(ListParcel, parcelPath);
        }

        /// <summary>
        /// GetParcel by id. 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Parcel GetParcel(int Id)
        {
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            object obj = ListParcel.Find(i => i.ParcelId == Id);

            if (obj != null)
            {
                return (Parcel)obj;
            }
            throw new ParcelException("Parcel not found");
        }


        /// <summary>
        /// UpdateParcel by obj.
        /// </summary>
        /// <param name="parcel"></param>
        public void UpdateParcel(Parcel parcel)
        {
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);

            int index = ListParcel.FindIndex(i => i.ParcelId == parcel.ParcelId);
            if (index == -1)
            {
                throw new ParcelException($"This Parcel have not exist, Please try again.");
            }
            ListParcel[index] = parcel;
            XMLTools.SaveListToXMLSerializer(ListParcel, parcelPath);
        }

        /// <summary>
        /// return parcel list by prdicate 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<Parcel> GetPackagesByPredicate(Predicate<Parcel> predicate = null)
        {
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);

            return ListParcel.Where(i => predicate == null ? true : predicate(i)).ToList();
        }
        #endregion

        /// <summary>
        /// RemoveCustomer by id.
        /// </summary>
        /// <param name="customerId"></param>
        #region Customer
        public void RemoveCustomer(int customerId)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            int index = ListCustomer.FindIndex(i => i.CustomerId == customerId);
            ListCustomer.RemoveAt(ListCustomer.FindIndex(i => i.CustomerId == customerId));
            if (index == -1)
            {
                throw new CustumerException($"This customer have not exist, Please try again.");
            }
            ListCustomer.RemoveAt(index);
            XMLTools.SaveListToXMLSerializer(ListCustomer, customerPath);
        }

        /// <summary>
        /// Update Customer by obj.
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(Customer customer)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);

            int index = ListCustomer.FindIndex(i => i.CustomerId == customer.CustomerId);
            if (index == -1)
            {
                throw new CustumerException($"This Customer have not exist, Please try again.");
            }
            ListCustomer[index] = customer;
            XMLTools.SaveListToXMLSerializer(ListCustomer, customerPath);
        }

        /// <summary>
        /// AddCustomer by obj.
        /// </summary>
        /// <param name="newCustomer"></param>
        public void AddCustomer(Customer newCustomer)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            ListCustomer.Add(ListCustomer.FindIndex(i => i.CustomerId == newCustomer.CustomerId) == -1 ?
                           newCustomer : throw new CustumerException($"This id {newCustomer.CustomerId} already exist"));

            XMLTools.SaveListToXMLSerializer(ListCustomer, customerPath);
        }

        /// <summary>
        /// GetCustomer by ID.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Customer GetCustomer(int Id)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);

            Customer customer = ListCustomer.Find(i => i.CustomerId == Id);
            return customer.CustomerId != default ? customer : throw new CustumerException("Customer not found");
        }

        /// <summary>
        /// Return customer list by prdicate. 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomersByPredicate(Predicate<Customer> predicate = null)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            return ListCustomer.FindAll(i => predicate == null ? true : predicate(i));

        }
        #endregion

        #region User 
        /// <summary>
        /// Add a user by object. (XMLElement use)
        /// </summary>
        /// <param name="newUser"></param>
        public void AddUser(User newUser)
        {
            XElement xElement = XMLTools.LoadListFromXMLElement(userPath);

            //select correct user by Linq 
            XElement us = (from u in xElement.Elements()
                           where u.Element("UserId").Value == newUser.UserId
                           select u).FirstOrDefault();

            if (us != null)
            {
                new XElement("User",
                new XElement("UserId", newUser.UserId),
                new XElement("Password", newUser.Password));

                xElement.Add(us);
                XMLTools.SaveListToXMLElement(xElement, userPath);
            }
            else
            {
                throw new UserException("User not found");
            }
        }
        /// <summary>
        /// Get user by object. (XMLElement use)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public User GetUser(string userID)
        {
            XElement userRootElement = XMLTools.LoadListFromXMLElement(userPath);

            User user = (from users in userRootElement.Elements()
                         where users.Element("UserId").Value == userID
                         select new User()
                         {
                             UserId = users.Element("UserId").Value,
                             Password = users.Element("Password").Value
                         }
                        ).FirstOrDefault();

            return user.UserId == userID ? user : throw new CheckIdException("User not found");
        }

        /// <summary>
        /// Get user by object (XMLElement use).
        /// </summary>
        /// <param name="user"></param>
        public void C(User user)
        {
            XElement userRootElement = XMLTools.LoadListFromXMLElement(userPath);

            XElement us = (from u in userRootElement.Elements()
                           where u.Element("UserId").Value == user.UserId
                           select u).FirstOrDefault();

            if (us != null)
            {
                us.Element("UserId").Value = user.UserId.ToString();
                us.Element("Password").Value = user.Password.ToString();

                XMLTools.SaveListToXMLElement(userRootElement, userPath);
            }
            else
            {
                throw new UserException("User not found");
            }
        }

        /// <summary>
        /// Remove user (XMLElement use).
        /// </summary>
        /// <param name="UserID"></param>
        public void RemoveUser(string UserID)
        {
            XElement userRootElement = XMLTools.LoadListFromXMLElement(userPath);

            XElement us = (from u in userRootElement.Elements()
                           where u.Element("UserId").Value == UserID
                           select u).FirstOrDefault();
            if (us != null)
            {
                us.Remove(); // Remove us from userRootElement

                XMLTools.SaveListToXMLElement(userRootElement, userPath);
            }
            else
            {
                throw new UserException("User not found");
            }
        }
        #endregion

        /// <summary>
        /// Set drone for parcel by parcel Id & drone Id
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        #region Operations
        public void SetDroneForParcel(int parcelId, int droneId)
        {
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);

            Parcel parcel = ListParcel.Find(Parcel => Parcel.ParcelId == parcelId);
            int index = ListParcel.ToList().FindIndex(i => i.ParcelId == parcelId);
            int droneIndex = ListDrone.FindIndex(i => i.DroneID == droneId);

            if (index != -1 && droneIndex != -1)
            {
                parcel.DroneId = droneId;
                parcel.PickedUp = DateTime.Now;
                ListParcel[index] = parcel;
                XMLTools.SaveListToXMLSerializer(ListParcel, parcelPath);
            }
            else
            {
                throw new CheckIdException("There is no match between the package and the drone");
            }
        }

        /// <summary>
        /// Collect parcel by parcel ID.
        /// </summary>
        /// <param name="parcelId"></param>
        public void PackageCollectionByDrone(int parcelId)
        {
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);

            Parcel parcel = ListParcel.Find(p => p.ParcelId == parcelId);
            int index = ListParcel.FindIndex(i => i.ParcelId == parcelId);

            if (index != -1)
            {
                parcel.PickedUp = DateTime.Now;
                ListParcel[index] = parcel;
                XMLTools.SaveListToXMLSerializer(ListParcel, parcelPath);
            }
            else
            {
                throw new CheckIdException("Parcel not found");
            }
        }

        /// <summary>
        /// Delivered package to custumer by parcelId find.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public bool DeliveredPackageToCustumer(int parcelId)
        {
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            int index = ListParcel.ToList().FindIndex(i => i.ParcelId == parcelId);
            Parcel parcel = ListParcel[index];
            if (index != -1)
            {
                parcel.Delivered = DateTime.Now;
                ListParcel[index] = parcel;
                XMLTools.SaveListToXMLSerializer(ListParcel, parcelPath);
                return true;
            }
            else
            {
                throw new CheckIdException("Parcel not found");
            }
        }


        /// <summary>
        /// Releasing ChargeDrone by drone Id & baseStation Id find.
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="baseStationId"></param>
        public void ReleasingChargeDrone(int droneId, int baseStationId)
        {
            List<DroneCharge> ListDroneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargesPath);

            int index = ListDroneCharge.FindIndex(i => i.DroneID == droneId && i.StationID == baseStationId);
            if (index != -1)
            {
                ListDroneCharge.RemoveAt(index);
                XMLTools.SaveListToXMLSerializer(ListDroneCharge, droneChargesPath);
            }
            else
            {
                throw new CheckIdException("Drone not found");
            }
        }


        /// <summary>
        /// Reduce one drone charge.
        /// </summary>
        /// <param name="stationId"></param>
        public void MinusDroneCharge(int stationId)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListFromXMLSerializer<BaseStation>(baseStationPath);

            BaseStation station = ListBaseStation.Find(x => x.StationID == stationId);
            int index = ListBaseStation.FindIndex(i => i.StationID == stationId);
            if (index != -1)
            {
                station.AvailableChargeSlots--;
                ListBaseStation[index] = station;
                XMLTools.SaveListToXMLSerializer(ListBaseStation, baseStationPath);
            }
            else
            {
                throw new CheckIdException("Station not found");
            }
        }

        /// <summary>
        /// Plus one drone charge
        /// </summary>
        /// <param name="stationId"></param>
        public void PlusDroneCharge(int stationId)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListFromXMLSerializer<BaseStation>(baseStationPath);

            BaseStation station = ListBaseStation.Find(x => x.StationID == stationId);
            int index = ListBaseStation.FindIndex(i => i.StationID == stationId);
            if (index != -1)
            {
                station.AvailableChargeSlots++;
                ListBaseStation[index] = station;
                XMLTools.SaveListToXMLSerializer(ListBaseStation, baseStationPath);
            }
            else
            {
                throw new CheckIdException("Station not found");
            }
        }

        public double[] RequetPowerConsumption()
        {
            double[] arr = new double[] {
                Config.PowerConsumptionAvailable ,
                Config.PowerConsumptionHeavyWeight ,
                Config.PowerConsumptionMediumWeight ,
                Config.PowerConsumptionHeavyWeight,
                Config.LoadingDrone
            };
            return arr;
        }
        #endregion
    }

    public static class Config
    {
        public static double PowerConsumptionAvailable = 0.01;
        public static double PowerConsumptionLightWeight = 0.04;
        public static double PowerConsumptionMediumWeight = 0.07;
        public static double PowerConsumptionHeavyWeight = 0.1;
        public static double LoadingDrone = 2;
    }
}
