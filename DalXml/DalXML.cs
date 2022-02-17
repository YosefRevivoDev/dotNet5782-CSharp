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
using System.Runtime.CompilerServices;

namespace DAL
{

    sealed class DalXml : IDal
    {
        string baseStationPath = @"BaseStationXml.xml";
        string dronePath = @"DroneXml.xml";
        string parcelPath = @"ParcelXml.xml";
        string customerPath = @"CustomerXml.xml";
        string droneChargesPath = @"DroneChargesXml.xml";
        string userPath = @"UserXml.xml";
        string runNumbers = @"RunNumbers.xml";

        static readonly DalXml instance = new DalXml();

        public static IDal Instance { get => instance; }

        private DalXml()  {

            //var p = Task.Factory.StartNew(() =>
            //{

            //    var c = GetCustomersByPredicate();

            //    List<Parcel> parcels = new List<Parcel>();
            //    Random random = new Random();
            //    for (int i = 200; i < 250; i++)
            //    {
            //        Parcel parcel = new Parcel();
            //        parcel.ParcelId = i;
            //        parcel.Created =  DateTime.Now;
            //        parcel.Assignment = parcel.Created.AddDays(1);
            //        parcel.PickedUp = parcel.Assignment.AddDays(1);
            //        parcel.Delivered = parcel.PickedUp.AddDays(1);
            //        parcel.ParcelPriority = (Priorities)random.Next(1, 4);
            //        parcel.ParcelWeight = (WeightCategories)random.Next(1, 4);
            //        var p = 0;
            //        var t = 0;
            //        do
            //        {
            //            p = c.ToList()[random.Next(0, c.Count())].CustomerId;
            //            t = c.ToList()[random.Next(0, c.Count())].CustomerId;
            //        } while (p == t);

            //        parcel.SenderId = p;
            //        parcel.TargetId = t;
            //        parcels.Add(parcel);
            //    }

            //    XMLTools.SaveListToXMLSerializer(parcels, parcelPath);
            //});

            //p.Wait();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]

        #region Drone
        public void UpdateDrone(Drone drone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);

            int index = drones.FindIndex(i => i.DroneID == drone.DroneID);
            if (index == -1)
            {
                throw new CheckIdException($"לא נמצא רחפן תואם, אנא נסה שוב");
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
                throw new CheckIdException("רחפן זה כבר קיים, אנא נסה שוב:" + newDrone.DroneID);
            }
        }
        public void RemoveDrone(int droneID)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);

            int index = drones.FindIndex(i => i.DroneID == droneID);

            if (index != -1)
            {
                drones.RemoveAt(index);
                XMLTools.SaveListToXMLSerializer(drones, dronePath);
            }
            throw new DroneException($"This drone have not exist, Please try again.");

        }
        public Drone GetDrone(int Id)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);

            Drone drone = drones.Find(i => i.DroneID == Id);
            return drone.DroneID != default ? drone : throw new CheckIfIdNotException("לא נמצא רחפן תואם");
        }
        public IEnumerable<Drone> GetDronesByPredicate(Predicate<Drone> predicate = null)
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);

            return ListDrone.Where(i => predicate == null ? true : predicate(i)).ToList();
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

        public DroneCharge GetDroneChargeByStation(int baseStationId)
        {

            List<DroneCharge> ListDroneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargesPath);

            DroneCharge drone = ListDroneCharge.Find(d => d.StationID == baseStationId);
            return drone.StationID != default ? drone : throw new CheckIfIdNotException("לא נמצאה תחנה מתאימה");
            //if (drone != -1)
            //{
            //    throw new DroneChargeException("The Station have not exists");
            //}
            //AddDroneCharge(new DroneCharge { StationID = baseStationId });
        }
        public DroneCharge GetDroneChargeByDrone(int droneId)
        {
            List<DroneCharge> ListDroneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargesPath);

            DroneCharge droneCharge = ListDroneCharge.Find(i => i.DroneID == droneId);
            return droneCharge.StationID != default ? droneCharge : throw new CheckIfIdNotException("לא נמצא רחפן תואם, אנא נסה שוב");

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
                           new_baseStation : throw new CheckIdException($"תחנה זו{new_baseStation.StationID} כבר קיימת במערכת"));

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
            if (index != -1)
            {
                ListBaseStation.RemoveAt(index);
                XMLTools.SaveListToXMLSerializer(ListBaseStation, baseStationPath);
            }
            throw new CheckIfIdNotException($"תחנה לא נמצאה , אנא נסה שוב");

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
                throw new CheckIdException($"תחנה זו כבר קיימת במערכת, אנא נסה שוב");
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
            return (object)station != null ? station : throw new CheckIfIdNotException("לא נמצאה תחנה מתאימה");
        }


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

            newParcel.ParcelId = int.Parse(xElement.Element("RunNumberParcel").Value);

            xElement.Element("RunNumberParcel").Value = (newParcel.ParcelId + 1).ToString();

            XMLTools.SaveListToXMLElement(xElement, runNumbers);

            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            ListParcel.Add(ListParcel.FindIndex(i => i.ParcelId == newParcel.ParcelId) == -1 ?
                           newParcel : throw new ParcelException($"חבילה זו {newParcel.ParcelId} כבר קיימת במערכת"));

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

            if (index != -1)
            {
                ListParcel.RemoveAt(index);
                XMLTools.SaveListToXMLSerializer(ListParcel, parcelPath);
            }
            //else
            //{
            //    throw new ParcelException($"This parcel have not exist, Please try again.");
            //}
        }

        /// <summary>
        /// GetParcel by id. 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Parcel GetParcel(int Id)
        {
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            Parcel parcel = ListParcel.Find(i => i.ParcelId == Id);
            return (object)parcel != null ? parcel : throw new CheckIfIdNotException("Parcel not found");
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

            return ListParcel.FindAll(i => predicate == null ? true : predicate(i));
        }
        #endregion

        #region Customer
        /// <summary>
        /// RemoveCustomer by id.
        /// </summary>
        /// <param name="customerId"></param>
        public void RemoveCustomer(int customerId)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);

            int index = ListCustomer.FindIndex(i => i.CustomerId == customerId);

            if (index != -1)
            {
                ListCustomer.RemoveAt(index);
                XMLTools.SaveListToXMLSerializer(ListCustomer, customerPath);
            }
            //throw new CheckIdException($"This customer have not exist, Please try again.");

        }

        /// <summary>
        /// Update Customer by obj.
        /// </summary>
        /// <param name="customer"></param>
        //public void UpdateCustomer(Customer customer)
        //{
        //    List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);

        //    int index = ListCustomer.FindIndex(i => i.CustomerId == customer.CustomerId);
        //    if (index == -1)
        //    {
        //        throw new CustumerException($"This Customer have not exist, Please try again.");
        //    }
        //    ListCustomer[index] = customer;
        //    XMLTools.SaveListToXMLSerializer(ListCustomer, customerPath);
        //}

        /// <summary>
        /// AddCustomer by obj.
        /// </summary>
        /// <param name="newCustomer"></param>
        public void AddCustomer(Customer newCustomer)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            ListCustomer.Add(ListCustomer.FindIndex(i => i.CustomerId == newCustomer.CustomerId) == -1 ?
                           newCustomer : throw new CustumerException($"לקוח זה {newCustomer.CustomerId} כבר קיים במערכת"));

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
            object obj = ListCustomer.Find(i => i.CustomerId == Id);
            if (obj != null)
            {
                return (Customer)obj;
            }
            throw new CheckIfIdNotException("לקוח לא קיים");
        }

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
                throw new CheckIdException("משתמש לא קיים במערכת");
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

            return user.UserId == userID ? user : throw new CheckIdException("משתמש לא קיים, אנא נסה שוב");
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

        #region Operations
        /// <summary>
        /// Set drone for parcel by parcel Id & drone Id
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public bool SetDroneForParcel(int parcelId, int droneId)
        {
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);

            Parcel parcel = ListParcel.Find(Parcel => Parcel.ParcelId == parcelId);
            int index = ListParcel.FindIndex(i => i.ParcelId == parcelId);
            int droneIndex = ListDrone.FindIndex(i => i.DroneID == droneId);

            if (index != -1 && droneIndex != -1)
            {
                parcel.DroneId = droneId;
                parcel.Assignment = DateTime.Now;
                ListParcel[index] = parcel;
                XMLTools.SaveListToXMLSerializer(ListParcel, parcelPath);
                return true;
            }
            else
            {
                throw new CheckIdException("לא נמצאה חבילה מתאימה לשיוך");
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
                throw new CheckIdException("החבילה לא נמצאה");
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

            Parcel parcel = ListParcel.Find(p => p.ParcelId == parcelId);
            int index = ListParcel.FindIndex(i => i.ParcelId == parcelId);
                
            if (index != -1)
            {
                parcel.Delivered = DateTime.Now;
                ListParcel[index] = parcel;
                XMLTools.SaveListToXMLSerializer(ListParcel, parcelPath);
                return true;
            }
            else
            {
                throw new CheckIdException("החבילה לא נמצאה");
            }
        }


        /// <summary>
        /// Releasing ChargeDrone by drone Id & baseStation Id find.
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="baseStationId"></param>
        public void ReleasingChargeDrone(int droneId)
        {
            List<DroneCharge> ListDroneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargesPath);

            int index = ListDroneCharge.FindIndex(i => i.DroneID == droneId);
            if (index != -1)
            {
                ListDroneCharge.RemoveAt(index);
                XMLTools.SaveListToXMLSerializer(ListDroneCharge, droneChargesPath);
            }
            else
            {
                throw new CheckIdException("הרחפן לא נמצא");
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
                throw new CheckIdException("התחנה לא נמצאה");
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
                throw new CheckIdException("התחנה לא נמצאה");
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
        public static double PowerConsumptionAvailable = 0.07;
        public static double PowerConsumptionLightWeight = 0.09;
        public static double PowerConsumptionMediumWeight = 0.15;
        public static double PowerConsumptionHeavyWeight = 0.3;
        public static double LoadingDrone = 2;
    }
}


//< p >
//   < Parcel >
//     < ParcelId > 6 </ ParcelId >
//     < SenderId > 349927608 </ SenderId >
//     < TargetId > 309865341 </ TargetId >
//     < ParcelWeight > medium </ ParcelWeight >
//     < ParcelPriority > fast </ ParcelPriority >
//     < DroneId > 4 </ DroneId >
//     < Created > 2022 - 01 - 25T10: 32:15.2526439 + 02:00 </ Created >

//               < Assignment > 2022 - 01 - 27T10: 32:15.2522151 + 02:00 </ Assignment >

//                         < PickedUp > 2022 - 02 - 13T17: 33:11.0868411 + 02:00 </ PickedUp >

//                                   < Delivered > 2022 - 02 - 09T13: 42:26.485767 + 02:00 </ Delivered >

//                                           </ Parcel >

//                                           < Parcel >

//                                             < ParcelId > 35 </ ParcelId >

//                                             < SenderId > 200786113 </ SenderId >

//                                             < TargetId > 203548976 </ TargetId >

//                                             < ParcelWeight > medium </ ParcelWeight >

//                                             < ParcelPriority > emergency </ ParcelPriority >

//                                             < DroneId > 3 </ DroneId >

//                                             < Created > 2022 - 01 - 25T10: 32:15.2522151 + 02:00 </ Created >

//                                                       < Assignment > 2022 - 02 - 13T01: 01:53.4615899 + 02:00 </ Assignment >

//                                                                 < PickedUp > 2022 - 02 - 09T13: 40:33.34183 + 02:00 </ PickedUp >

//                                                                           < Delivered > 2022 - 02 - 09T12: 53:15.6735575 + 02:00 </ Delivered >

//                                                                                   </ Parcel >

//                                                                                   < Parcel >

//                                                                                     < ParcelId > 1 </ ParcelId >

//                                                                                     < SenderId > 201344879 </ SenderId >

//                                                                                     < TargetId > 203548976 </ TargetId >

//                                                                                     < ParcelWeight > light </ ParcelWeight >

//                                                                                     < ParcelPriority > emergency </ ParcelPriority >

//                                                                                     < DroneId > 14 </ DroneId >

//                                                                                     < Created > 2022 - 01 - 25T10: 32:15.2526393 + 02:00 </ Created >

//                                                                                               < Assignment > 2022 - 02 - 13T17: 40:41.6184472 + 02:00 </ Assignment >

//                                                                                                         < PickedUp > 2022 - 02 - 13T17: 40:44.7497471 + 02:00 </ PickedUp >

//                                                                                                                   < Delivered > 2022 - 02 - 13T17: 40:58.0755171 + 02:00 </ Delivered >

//                                                                                                                           </ Parcel >

//                                                                                                                           < Parcel >

//                                                                                                                             < ParcelId > 2 </ ParcelId >

//                                                                                                                             < SenderId > 200996586 </ SenderId >

//                                                                                                                             < TargetId > 200786113 </ TargetId >

//                                                                                                                             < ParcelWeight > medium </ ParcelWeight >

//                                                                                                                             < ParcelPriority > regular </ ParcelPriority >

//                                                                                                                             < DroneId > 10 </ DroneId >

//                                                                                                                             < Created > 2022 - 01 - 25T10: 32:15.2526414 + 02:00 </ Created >

//                                                                                                                                       < Assignment > 2022 - 02 - 05T20: 25:56.4167653 + 02:00 </ Assignment >

//                                                                                                                                                 < PickedUp > 2022 - 02 - 07T20: 25:56.4167653 + 02:00 </ PickedUp >

//                                                                                                                                                           < Delivered > 2022 - 02 - 09T13: 50:46.4117676 + 02:00 </ Delivered >

//                                                                                                                                                                   </ Parcel >

//                                                                                                                                                                   < Parcel >

//                                                                                                                                                                     < ParcelId > 4 </ ParcelId >

//                                                                                                                                                                     < SenderId > 309865341 </ SenderId >

//                                                                                                                                                                     < TargetId > 200532406 </ TargetId >

//                                                                                                                                                                     < ParcelWeight > light </ ParcelWeight >

//                                                                                                                                                                     < ParcelPriority > emergency </ ParcelPriority >

//                                                                                                                                                                     < DroneId > 7 </ DroneId >

//                                                                                                                                                                     < Created > 2022 - 01 - 25T10: 32:15.2526426 + 02:00 </ Created >

//                                                                                                                                                                               < Assignment > 2022 - 02 - 13T21: 06:00.8547032 + 02:00 </ Assignment >

//                                                                                                                                                                                         < PickedUp > 2022 - 02 - 13T21: 06:08.6124737 + 02:00 </ PickedUp >

//                                                                                                                                                                                                   < Delivered > 2022 - 02 - 13T21: 06:25.5466145 + 02:00 </ Delivered >

//                                                                                                                                                                                                           </ Parcel >

//                                                                                                                                                                                                           < Parcel >

//                                                                                                                                                                                                             < ParcelId > 5 </ ParcelId >

//                                                                                                                                                                                                             < SenderId > 349927608 </ SenderId >

//                                                                                                                                                                                                             < TargetId > 309865341 </ TargetId >

//                                                                                                                                                                                                             < ParcelWeight > light </ ParcelWeight >

//                                                                                                                                                                                                             < ParcelPriority > regular </ ParcelPriority >

//                                                                                                                                                                                                             < DroneId > 10 </ DroneId >

//                                                                                                                                                                                                             < Created > 2022 - 01 - 25T10: 32:15.2526434 + 02:00 </ Created >

//                                                                                                                                                                                                                       < Assignment > 2022 - 02 - 13T21: 42:56.14156 + 02:00 </ Assignment >

//                                                                                                                                                                                                                                 < PickedUp > 2022 - 02 - 14T15: 39:40.5927063 + 02:00 </ PickedUp >

//                                                                                                                                                                                                                                           < Delivered > 0001 - 01 - 01T00: 00:00 </ Delivered >

//                                                                                                                                                                                                                                                 </ Parcel >

//                                                                                                                                                                                                                                                 < Parcel >

//                                                                                                                                                                                                                                                   < ParcelId > 7 </ ParcelId >

//                                                                                                                                                                                                                                                   < SenderId > 203548976 </ SenderId >

//                                                                                                                                                                                                                                                   < TargetId > 309865341 </ TargetId >

//                                                                                                                                                                                                                                                   < ParcelWeight > medium </ ParcelWeight >

//                                                                                                                                                                                                                                                   < ParcelPriority > emergency </ ParcelPriority >

//                                                                                                                                                                                                                                                   < DroneId > 9 </ DroneId >

//                                                                                                                                                                                                                                                   < Created > 2022 - 01 - 25T10: 32:15.2526445 + 02:00 </ Created >

//                                                                                                                                                                                                                                                             < Assignment > 2022 - 02 - 13T21: 31:27.334908 + 02:00 </ Assignment >

//                                                                                                                                                                                                                                                                       < PickedUp > 2022 - 02 - 13T21: 31:30.82565 + 02:00 </ PickedUp >

//                                                                                                                                                                                                                                                                                 < Delivered > 2022 - 02 - 13T21: 31:43.3442452 + 02:00 </ Delivered >

//                                                                                                                                                                                                                                                                                         </ Parcel >

//                                                                                                                                                                                                                                                                                         < Parcel >

//                                                                                                                                                                                                                                                                                           < ParcelId > 8 </ ParcelId >

//                                                                                                                                                                                                                                                                                           < SenderId > 311890522 </ SenderId >

//                                                                                                                                                                                                                                                                                           < TargetId > 200786113 </ TargetId >

//                                                                                                                                                                                                                                                                                           < ParcelWeight > medium </ ParcelWeight >

//                                                                                                                                                                                                                                                                                           < ParcelPriority > fast </ ParcelPriority >

//                                                                                                                                                                                                                                                                                           < DroneId > 3 </ DroneId >

//                                                                                                                                                                                                                                                                                           < Created > 2022 - 01 - 25T10: 32:15.252645 + 02:00 </ Created >

//                                                                                                                                                                                                                                                                                                     < Assignment > 2022 - 02 - 13T11: 39:41.6765284 + 02:00 </ Assignment >

//                                                                                                                                                                                                                                                                                                               < PickedUp > 2022 - 02 - 14T15: 37:43.1493631 + 02:00 </ PickedUp >

//                                                                                                                                                                                                                                                                                                                         < Delivered > 2022 - 02 - 13T11: 40:51.8355765 + 02:00 </ Delivered >

//                                                                                                                                                                                                                                                                                                                                 </ Parcel >

//                                                                                                                                                                                                                                                                                                                                 < Parcel >

//                                                                                                                                                                                                                                                                                                                                   < ParcelId > 9 </ ParcelId >

//                                                                                                                                                                                                                                                                                                                                   < SenderId > 200996586 </ SenderId >

//                                                                                                                                                                                                                                                                                                                                   < TargetId > 200786113 </ TargetId >

//                                                                                                                                                                                                                                                                                                                                   < ParcelWeight > medium </ ParcelWeight >

//                                                                                                                                                                                                                                                                                                                                   < ParcelPriority > regular </ ParcelPriority >

//                                                                                                                                                                                                                                                                                                                                   < DroneId > 7 </ DroneId >

//                                                                                                                                                                                                                                                                                                                                   < Created > 2022 - 01 - 25T10: 32:15.2526456 + 02:00 </ Created >

//                                                                                                                                                                                                                                                                                                                                             < Assignment > 2022 - 02 - 13T11: 42:46.2373053 + 02:00 </ Assignment >

//                                                                                                                                                                                                                                                                                                                                                       < PickedUp > 2022 - 02 - 13T21: 31:30.824678 + 02:00 </ PickedUp >

//                                                                                                                                                                                                                                                                                                                                                                 < Delivered > 2022 - 02 - 13T11: 42:54.5434244 + 02:00 </ Delivered >

//                                                                                                                                                                                                                                                                                                                                                                         </ Parcel >

//                                                                                                                                                                                                                                                                                                                                                                         < Parcel >

//                                                                                                                                                                                                                                                                                                                                                                           < ParcelId > 11 </ ParcelId >

//                                                                                                                                                                                                                                                                                                                                                                           < SenderId > 301242853 </ SenderId >

//                                                                                                                                                                                                                                                                                                                                                                           < TargetId > 349927608 </ TargetId >

//                                                                                                                                                                                                                                                                                                                                                                           < ParcelWeight > medium </ ParcelWeight >

//                                                                                                                                                                                                                                                                                                                                                                           < ParcelPriority > emergency </ ParcelPriority >

//                                                                                                                                                                                                                                                                                                                                                                           < DroneId > 6 </ DroneId >

//                                                                                                                                                                                                                                                                                                                                                                           < Created > 2022 - 01 - 25T10: 32:15.2526467 + 02:00 </ Created >

//                                                                                                                                                                                                                                                                                                                                                                                     < Assignment > 2022 - 02 - 13T01: 09:16.2161336 + 02:00 </ Assignment >

//                                                                                                                                                                                                                                                                                                                                                                                               < PickedUp > 2022 - 02 - 13T17: 33:11.1077973 + 02:00 </ PickedUp >

//                                                                                                                                                                                                                                                                                                                                                                                                         < Delivered > 0001 - 01 - 01T00: 00:00 </ Delivered >

//                                                                                                                                                                                                                                                                                                                                                                                                               </ Parcel >

//                                                                                                                                                                                                                                                                                                                                                                                                               < Parcel >

//                                                                                                                                                                                                                                                                                                                                                                                                                 < ParcelId > 13 </ ParcelId >

//                                                                                                                                                                                                                                                                                                                                                                                                                 < SenderId > 200786113 </ SenderId >

//                                                                                                                                                                                                                                                                                                                                                                                                                 < TargetId > 203548976 </ TargetId >

//                                                                                                                                                                                                                                                                                                                                                                                                                 < ParcelWeight > light </ ParcelWeight >

//                                                                                                                                                                                                                                                                                                                                                                                                                 < ParcelPriority > regular </ ParcelPriority >

//                                                                                                                                                                                                                                                                                                                                                                                                                 < DroneId > 3 </ DroneId >

//                                                                                                                                                                                                                                                                                                                                                                                                                 < Created > 2022 - 01 - 25T10: 32:15.2526478 + 02:00 </ Created >

//                                                                                                                                                                                                                                                                                                                                                                                                                           < Assignment > 2022 - 02 - 14T13: 32:50.4123897 + 02:00 </ Assignment >

//                                                                                                                                                                                                                                                                                                                                                                                                                                     < PickedUp > 0001 - 01 - 01T00: 00:00 </ PickedUp >

//                                                                                                                                                                                                                                                                                                                                                                                                                                             < Delivered > 0001 - 01 - 01T00: 00:00 </ Delivered >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                   </ Parcel >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                   < Parcel >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                     < ParcelId > 14 </ ParcelId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                     < SenderId > 200532406 </ SenderId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                     < TargetId > 200786113 </ TargetId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                     < ParcelWeight > light </ ParcelWeight >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                     < ParcelPriority > regular </ ParcelPriority >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                     < DroneId > 8 </ DroneId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                     < Created > 2022 - 01 - 25T10: 32:15.2526483 + 02:00 </ Created >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                               < Assignment > 2022 - 02 - 14T15: 19:57.6463861 + 02:00 </ Assignment >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                         < PickedUp > 2022 - 02 - 14T15: 37:43.1540273 + 02:00 </ PickedUp >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   < Delivered > 2022 - 02 - 14T15: 37:45.9406709 + 02:00 </ Delivered >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           </ Parcel >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           < Parcel >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             < ParcelId > 15 </ ParcelId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             < SenderId > 301242853 </ SenderId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             < TargetId > 349927608 </ TargetId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             < ParcelWeight > light </ ParcelWeight >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             < ParcelPriority > fast </ ParcelPriority >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             < DroneId > 3 </ DroneId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             < Created > 2022 - 01 - 25T10: 32:15.2526488 + 02:00 </ Created >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       < Assignment > 2022 - 02 - 13T21: 13:06.3898276 + 02:00 </ Assignment >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 < PickedUp > 2022 - 02 - 13T21: 13:13.0396208 + 02:00 </ PickedUp >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           < Delivered > 2022 - 02 - 13T21: 13:14.1561475 + 02:00 </ Delivered >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   </ Parcel >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   < Parcel >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     < ParcelId > 16 </ ParcelId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     < SenderId > 301242853 </ SenderId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     < TargetId > 349927608 </ TargetId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     < ParcelWeight > light </ ParcelWeight >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     < ParcelPriority > fast </ ParcelPriority >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     < DroneId > 8 </ DroneId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     < Created > 2022 - 01 - 25T10: 32:15.2526493 + 02:00 </ Created >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               < Assignment > 2022 - 02 - 13T21: 14:21.4815838 + 02:00 </ Assignment >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         < PickedUp > 2022 - 02 - 13T21: 31:08.3951776 + 02:00 </ PickedUp >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   < Delivered > 2022 - 02 - 13T21: 31:10.8158786 + 02:00 </ Delivered >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           </ Parcel >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           < Parcel >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             < ParcelId > 17 </ ParcelId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             < SenderId > 309865341 </ SenderId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             < TargetId > 200532406 </ TargetId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             < ParcelWeight > light </ ParcelWeight >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             < ParcelPriority > regular </ ParcelPriority >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             < DroneId > 5 </ DroneId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             < Created > 2022 - 01 - 25T10: 32:15.2526499 + 02:00 </ Created >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       < Assignment > 2022 - 02 - 14T15: 37:02.7687641 + 02:00 </ Assignment >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 < PickedUp > 2022 - 02 - 14T15: 39:40.6002418 + 02:00 </ PickedUp >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           < Delivered > 2022 - 02 - 14T15: 39:41.7536347 + 02:00 </ Delivered >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   </ Parcel >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   < Parcel >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     < ParcelId > 19 </ ParcelId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     < SenderId > 209433871 </ SenderId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     < TargetId > 349927608 </ TargetId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     < ParcelWeight > light </ ParcelWeight >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     < ParcelPriority > fast </ ParcelPriority >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     < DroneId > 34 </ DroneId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     < Created > 2022 - 01 - 25T10: 32:15.2526509 + 02:00 </ Created >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               < Assignment > 2022 - 02 - 13T21: 16:30.7825749 + 02:00 </ Assignment >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         < PickedUp > 0001 - 01 - 01T00: 00:00 </ PickedUp >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 < Delivered > 0001 - 01 - 01T00: 00:00 </ Delivered >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       </ Parcel >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       < Parcel >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         < ParcelId > 27 </ ParcelId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         < SenderId > 0 </ SenderId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         < TargetId > 0 </ TargetId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         < ParcelWeight > medium </ ParcelWeight >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         < ParcelPriority > regular </ ParcelPriority >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         < DroneId > 0 </ DroneId >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         < Created > 2022 - 02 - 14T13: 19:24.5080942 + 02:00 </ Created >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   < Assignment > 0001 - 01 - 01T00: 00:00 </ Assignment >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           < PickedUp > 0001 - 01 - 01T00: 00:00 </ PickedUp >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   < Delivered > 0001 - 01 - 01T00: 00:00 </ Delivered >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         </ Parcel >

//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       </ p >
