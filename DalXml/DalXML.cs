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

        ~DalXml()
        {
            XMLTools.SaveListToXMLSerializer(Drones, dronePath);
        }

        private List<Drone> Drones;
        private List<BaseStation> Stations;
        private List<Customer> Customer;
        private List<Parcel> Parcels;
        private List<DroneCharge> DroneCharges;
        private XElement userXelement;

        private void initXelement()
        {
            if (userXelement == null)
                userXelement = XMLTools.LoadListFromXMLElement(userPath);
        }

        private void InitListsSerializer<T>(ref List<T> ListOfData, string fileName)
        {
             ListOfData ??= XMLTools.LoadListToXMLSerializer<T>(fileName);
        }

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
            InitListsSerializer(ref Drones, dronePath);

            int index = Drones.FindIndex(i => i.DroneID == drone.DroneID);
            if (index == -1)
            {
                throw new DroneException($"This Drone have not exist, Please try again.");
            }
            Drones[index] = drone;
        }
        public void AddDrone(Drone newDrone)
        {
            InitListsSerializer(ref Drones, dronePath);

            XElement xElement = XMLTools.LoadListFromXMLElement(runNumbers);
            newDrone.DroneID = int.Parse(xElement.Element("RunNumberDrone").Value) + 1;
            xElement.Element("RunNumberDrone").Value = newDrone.DroneID.ToString();
            XMLTools.SaveListToXMLElement(xElement, runNumbers);


            Drones.Add(Drones.FindIndex(i => i.DroneID == newDrone.DroneID) == -1 ?
                           newDrone : throw new DroneException($"This id{newDrone.DroneID}already exist"));

          // XMLTools.SaveListToXMLSerializer(Drones, dronePath);
        }
        public void RemoveDrone(int droneID)
        {
            InitListsSerializer(ref Drones, dronePath);
            InitListsSerializer(ref DroneCharges, droneChargesPath);

            int index = DroneCharges.FindIndex(i => i.DroneID == droneID);
            DroneCharges.RemoveAt(DroneCharges.FindIndex(i => i.DroneID == droneID));
            if (index == -1)
            {
                throw new DroneException($"This drone have not exist, Please try again.");
            }
            Drones.RemoveAt(index);

        }
        public Drone GetDrone(int Id)
        {
            InitListsSerializer(ref Drones, dronePath);

            Drone drone = Drones.Find(i => i.DroneID == Id);
            return drone.DroneID != default ? drone : throw new DroneException("Drone not found");
        }
        public IEnumerable<Drone> GetDronesByPredicate(Predicate<Drone> predicate = null)
        {
            //InitListsSerializer(Drones, dronePath);
            List<Drone> ListDrone = XMLTools.LoadListToXMLSerializer<Drone>(dronePath);

            return ListDrone.FindAll(i => predicate == null ? true : predicate(i));
        }
        #endregion

        #region DroneCharge
        public void AddDroneCharge(DroneCharge droneCharge)
        {
            List<DroneCharge> ListdroneCharge = XMLTools.LoadListToXMLSerializer<DroneCharge>(droneChargesPath);

            ListdroneCharge.Add(droneCharge);
            XMLTools.SaveListToXMLSerializer(ListdroneCharge, droneChargesPath);
            //InitListsSerializer(ref DroneCharges, droneChargesPath);

            //DroneCharges.Add(DroneCharges.FindIndex(i => i.StationID == droneCharge.StationID) == -1 ?
            //            droneCharge : throw new DroneChargeException($"This id {droneCharge.StationID} already exist"));
            //XMLTools.SaveListToXMLSerializer(DroneCharges, droneChargesPath);

        }
        public void UpdateDroneCharge(DroneCharge droneCharge)
        {
            InitListsSerializer(ref DroneCharges, droneChargesPath);

           // List<DroneCharge> ListDroneCharge = XMLTools.LoadListToXMLSerializer<DroneCharge>(droneChargesPath);

            int index = DroneCharges.FindIndex(i => i.DroneID == droneCharge.DroneID);
            if (index == -1)
            {
                throw new DroneChargeException($"This DroneCharge have not exist, Please try again.");
            }
            DroneCharges[index] = droneCharge;
        }
        public void GetDroneChargeByStation(int baseStationId)
        {
            InitListsSerializer(ref DroneCharges, droneChargesPath);

            //List<DroneCharge> ListDroneCharge = XMLTools.LoadListToXMLSerializer<DroneCharge>(droneChargesPath);

            int index = DroneCharges.ToList().FindIndex(i => i.StationID == baseStationId);
            if (index != -1)
            {
                throw new DroneChargeException("The Station have not exists");
            }
            AddDroneCharge(new DroneCharge { StationID = baseStationId });
        }
        public DroneCharge GetDroneChargeByDrone(int droneId)
        {
            InitListsSerializer(ref DroneCharges, droneChargesPath);

            //List<DroneCharge> ListDroneCharge = XMLTools.LoadListToXMLSerializer<DroneCharge>(droneChargesPath);

            DroneCharge droneCharge = DroneCharges.Find(i => i.DroneID == droneId);
            return droneCharge.StationID != default ? droneCharge : throw new CheckIfIdNotException("sorry, this Drone is not found.");

        }
        public IEnumerable<DroneCharge> GetDroneChargesByPredicate(Predicate<DroneCharge> predicate = null)
        {
            InitListsSerializer(ref DroneCharges, droneChargesPath);

            //List<DroneCharge> ListDroneCharge = XMLTools.LoadListToXMLSerializer<DroneCharge>(droneChargesPath);

            return DroneCharges.FindAll(i => predicate == null ? true : predicate(i));
        }
        #endregion

        #region BaseStation

        public void AddBaseStation(BaseStation new_baseStation)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListToXMLSerializer<BaseStation>(baseStationPath);
            ListBaseStation.Add(ListBaseStation.FindIndex(i => i.StationID == new_baseStation.StationID) == -1 ?
                           new_baseStation : throw new BaseStationException($"This id{new_baseStation.StationID}already exist"));

            XMLTools.SaveListToXMLSerializer(ListBaseStation, baseStationPath);
        }

        public void RemoveBaseStation(int stationID)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListToXMLSerializer<BaseStation>(baseStationPath);
            int index = ListBaseStation.FindIndex(i => i.StationID == stationID);
            if (index == -1)
            {
                throw new BaseStationException($"This station have not exist, Please try again.");
            }
            ListBaseStation.RemoveAt(index);
        }

        public void UpdateBaseStation(BaseStation baseStation)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListToXMLSerializer<BaseStation>(baseStationPath);
            int index = ListBaseStation.FindIndex(i => i.StationID == baseStation.StationID);
            if (index == -1)
            {
                throw new BaseStationException($"This BaseStation have not exist, Please Try again.");
            }
            ListBaseStation[index] = baseStation;
        }

        public BaseStation GetBaseStation(int Id)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListToXMLSerializer<BaseStation>(baseStationPath);

            BaseStation station = ListBaseStation.Find(i => i.StationID == Id);
            return station.StationID != default ? station : throw new BaseStationException("Station not found");
        }
        public IEnumerable<BaseStation> GetBaseStationByPredicate(Predicate<BaseStation> predicate = null)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListToXMLSerializer<BaseStation>(baseStationPath);

            return ListBaseStation.FindAll(i => predicate == null ? true : predicate(i));
        }
        #endregion

        #region Parcel
        public void AddParcel(Parcel newParcel)
        {
            XElement xElement = XMLTools.LoadListFromXMLElement(runNumbers);
            newParcel.ParcelId = int.Parse(xElement.Element("RunNumberParcel").Value) + 1;
            xElement.Element("RunNumberParcel").Value = newParcel.ParcelId.ToString();
            XMLTools.SaveListToXMLElement(xElement, runNumbers);

            List<Parcel> ListParcel = XMLTools.LoadListToXMLSerializer<Parcel>(parcelPath);
            ListParcel.Add(ListParcel.FindIndex(i => i.ParcelId == newParcel.ParcelId) == -1 ?
                           newParcel : throw new ParcelException($"This id{newParcel.ParcelId}already exist"));

            XMLTools.SaveListToXMLSerializer(ListParcel, parcelPath);
        }
        public void RemoveParcel(int parcelId)
        {
            List<Parcel> ListParcel = XMLTools.LoadListToXMLSerializer<Parcel>(parcelPath);

            int index = ListParcel.FindIndex(i => i.ParcelId == parcelId);
            ListParcel.RemoveAt(ListParcel.FindIndex(i => i.ParcelId == parcelId));
            if (index == -1)
            {
                throw new ParcelException($"This parcel have not exist, Please try again.");
            }
            ListParcel.RemoveAt(index);
        }
        public Parcel GetParcel(int Id)
        {
            List<Parcel> ListParcel = XMLTools.LoadListToXMLSerializer<Parcel>(parcelPath);
            object obj = ListParcel.Find(i => i.ParcelId == Id);

            if(obj != null)
            {
               return (Parcel)obj;
            }
            throw new ParcelException("Parcel not found");
        }
        public void UpdateParcel(Parcel parcel)
        {
            List<Parcel> ListParcel = XMLTools.LoadListToXMLSerializer<Parcel>(parcelPath);

            int index = ListParcel.FindIndex(i => i.ParcelId == parcel.ParcelId);
            if (index == -1)
            {
                throw new ParcelException($"This Parcel have not exist, Please try again.");
            }
            ListParcel[index] = parcel;
        }
        public IEnumerable<Parcel> GetPackagesByPredicate(Predicate<Parcel> predicate = null)
        {
            List<Parcel> ListParcel = XMLTools.LoadListToXMLSerializer<Parcel>(parcelPath);

            return ListParcel.FindAll(i => predicate == null ? true : predicate(i));
        }
        #endregion

        #region Customer
        public void RemoveCustomer(int customerId)
        {
            List<Customer> ListCustomer = XMLTools.LoadListToXMLSerializer<Customer>(customerPath);
            int index = ListCustomer.FindIndex(i => i.CustomerId == customerId);
            ListCustomer.RemoveAt(ListCustomer.FindIndex(i => i.CustomerId == customerId));
            if (index == -1)
            {
                throw new CustumerException($"This customer have not exist, Please try again.");
            }
            ListCustomer.RemoveAt(index);
        }
        public void UpdateCustomer(Customer customer)
        {
            List<Customer> ListCustomer = XMLTools.LoadListToXMLSerializer<Customer>(customerPath);

            int index = ListCustomer.FindIndex(i => i.CustomerId == customer.CustomerId);
            if (index == -1)
            {
                throw new CustumerException($"This Customer have not exist, Please try again.");
            }
            ListCustomer[index] = customer;
        }
        public void AddCustomer(Customer newCustomer)
        {
            List<Customer> ListCustomer = XMLTools.LoadListToXMLSerializer<Customer>(customerPath);
            ListCustomer.Add(ListCustomer.FindIndex(i => i.CustomerId == newCustomer.CustomerId) == -1 ?
                           newCustomer : throw new CustumerException($"This id{newCustomer.CustomerId}already exist"));

            XMLTools.SaveListToXMLSerializer(ListCustomer, customerPath);
        }
        public Customer GetCustomer(int Id)
        {
            List<Customer> ListCustomer = XMLTools.LoadListToXMLSerializer<Customer>(customerPath);

            Customer customer = ListCustomer.Find(i => i.CustomerId == Id);
            return customer.CustomerId != default ? customer : throw new CustumerException("Customer not found");
        }
        public IEnumerable<Customer> GetCustomersByPredicate(Predicate<Customer> predicate = null)
        {
            List<Customer> ListCustomer = XMLTools.LoadListToXMLSerializer<Customer>(customerPath);
            return ListCustomer.FindAll(i => predicate == null ? true : predicate(i));

        }
        #endregion

        #region User 
        public void AddUser(User newUser)
        {
            initXelement();

            XElement us = (from u in userXelement.Elements()
                           where int.Parse(u.Element("UserId").Value) == newUser.UserId
                           select u).FirstOrDefault();

            if (us != null)
            {
                new XElement("User",
                new XElement("UserId", newUser.UserId),
                new XElement("UserName", newUser.UserName),
                new XElement("FirstName", newUser.FirstName),
                new XElement("LastName", newUser.LastName),
                new XElement("Password", newUser.Password));

                userXelement.Add(us);
                XMLTools.SaveListToXMLElement(userXelement, userPath);
            }
            else
            {
                throw new UserException("User not found");
            }
        }
        public User GetUser(string userName)
        {
            XElement userRootElement = XMLTools.LoadListFromXMLElement(userPath);

            User user = (from users in userRootElement.Elements()
                         where users.Element("UserName").Value == userName
                         select new User()
                         {
                             UserId = Int32.Parse(users.Element("UserId").Value),
                             FirstName = users.Element("FirstName").Value,
                             LastName = users.Element("LastName").Value,
                             UserName = users.Element("UserName").Value,
                             Password = users.Element("Password").Value
                         }
                        ).FirstOrDefault();

            return user.UserName == userName ? user : throw new CheckIdException("User not found");
        }
        public void UpdateUser(User user)
        {
            XElement userRootElement = XMLTools.LoadListFromXMLElement(userPath);

            XElement us = (from u in userRootElement.Elements()
                           where int.Parse(u.Element("UserId").Value) == user.UserId
                           select u).FirstOrDefault();

            if (us != null)
            {
                us.Element("UserId").Value = user.UserId.ToString();
                us.Element("UserName").Value = user.UserName;
                us.Element("FirstName").Value = user.FirstName;
                us.Element("LastName").Value = user.LastName;
                us.Element("Password").Value = user.Password.ToString();

                XMLTools.SaveListToXMLElement(userRootElement, userPath);
            }
            else
            {
                throw new UserException("User not found");
            }
        }
        public void RemoveUser(int UserID)
        {
            XElement userRootElement = XMLTools.LoadListFromXMLElement(userPath);

            XElement us = (from u in userRootElement.Elements()
                           where int.Parse(u.Element("UserId").Value) == UserID
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

        #region Operations
        public void SetDroneForParcel(int parcelId, int droneId)
        {
            List<Parcel> ListParcel = XMLTools.LoadListToXMLSerializer<Parcel>(parcelPath);
            List<Drone> ListDrone = XMLTools.LoadListToXMLSerializer<Drone>(dronePath);

            int index = ListParcel.ToList().FindIndex(i => i.ParcelId == parcelId);
            Parcel parcel = ListParcel[index];
            int droneIndex = ListDrone.FindIndex(i => i.DroneID == droneId);
            if (index != -1 && droneIndex != -1)
            {
                parcel.DroneId = droneId;
                parcel.PickedUp = DateTime.Now;
                ListParcel[index] = parcel;
            }
            else
            {
                throw new CheckIdException("There is no match between the package and the drone");
            }
        }

        public void PackageCollectionByDrone(int parcelId)
        {
            List<Parcel> ListParcel = XMLTools.LoadListToXMLSerializer<Parcel>(parcelPath);

            int index = ListParcel.ToList().FindIndex(i => i.ParcelId == parcelId);
            Parcel parcel = ListParcel[index];
            if (index != -1)
            {
                parcel.PickedUp = DateTime.Now;
                ListParcel[index] = parcel;
            }
            else
            {
                throw new CheckIdException("Parcel not found");
            }
        }

        public void DeliveredPackageToCustumer(int parcelId)
        {
            List<Parcel> ListParcel = XMLTools.LoadListToXMLSerializer<Parcel>(parcelPath);

            int index = ListParcel.ToList().FindIndex(i => i.ParcelId == parcelId);
            Parcel parcel = ListParcel[index];
            if (index != -1)
            {
                parcel.Delivered = DateTime.Now;
                ListParcel[index] = parcel;
            }
            else
            {
                throw new CheckIdException("Parcel not found");
            }
        }

        public void ReleasingChargeDrone(int droneId, int baseStationId)
        {
            List<DroneCharge> ListDroneCharge = XMLTools.LoadListToXMLSerializer<DroneCharge>(droneChargesPath);

            int index = ListDroneCharge.FindIndex(i => i.DroneID == droneId && i.StationID == baseStationId);
            if (index != -1)
            {
                ListDroneCharge.RemoveAt(index);
            }
            else
            {
                throw new CheckIdException("Drone not found");
            }
        }

        public void MinusDroneCharge(int stationId)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListToXMLSerializer<BaseStation>(baseStationPath);

            BaseStation station = ListBaseStation.Find(x => x.StationID == stationId);
            int index = ListBaseStation.FindIndex(i => i.StationID == stationId);
            if (index != -1)
            {
                station.AvailableChargeSlots--;
                ListBaseStation[index] = station;
            }
            else
            {
                throw new CheckIdException("Station not found");
            }
        }

        public void PlusDroneCharge(int stationId)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListToXMLSerializer<BaseStation>(baseStationPath);

            BaseStation station = ListBaseStation.Find(x => x.StationID == stationId);
            int index = ListBaseStation.FindIndex(i => i.StationID == stationId);
            if (index != -1)
            {
                station.AvailableChargeSlots++;
                ListBaseStation[index] = station;
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
