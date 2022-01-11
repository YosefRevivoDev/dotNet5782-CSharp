using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using DAL;

namespace BL
{
    public partial class BL : IBL
    {
        //[MethodImpl(MethodImplOptions.Synchronized)]

         
        readonly DalApi.IDal dal; // Dal object to invite DAL functions 
        public List<DroneToList> DroneToList { get; set; } //Dynamic drone's list at BL layer
        static Random random = new(DateTime.Now.Millisecond);
        public static double PowerConsumptionAvailable;
        public static double PowerConsumptionLightWeight;
        public static double PowerConsumptionMediumWeight;
        public static double PowerConsumptionHeavyWeight;
        public static double LoadingDrone;

        public BL()
        {
            dal = new DalObject(); // Access to summon methods from Datasource
            DroneToList = new List<DroneToList>();
            InitDroneToLists(); // Initialize the list from DAL layer 
        }

        #region Add Object
        public void AddBaseStation(BaseStation newBaseStation)
        {
            //Set drone information from DAL layer 
            DO.BaseStation baseStation = new()
            {
                StationID = newBaseStation.ID,
                Name = newBaseStation.Name,
                Longtitude = newBaseStation.location.Longtitude,
                Latitude = newBaseStation.location.Latitude,
                AvailableChargeSlots = newBaseStation.AvailableChargingStations,
            };
            try
            {
                dal.AddBaseStation(baseStation);
            }
            catch { }
        }
        public void AddNewDrone(Drone newDrone, int NumberOfStation)
        {
            try
            {
                //Get bastation from Dal with ID key 
                DO.BaseStation baseStation = dal.GetBaseStation(NumberOfStation);

                if (baseStation.AvailableChargeSlots > 0)
                {
                    DO.DroneCharge droneCharge = new()
                    {
                        DroneID = newDrone.DroneID,
                        StationID = baseStation.StationID
                    };
                    newDrone.Status = DroneStatus.maintenance;
                    baseStation.AvailableChargeSlots--;
                    newDrone.CurrentLocation = new Location()
                    {
                        Latitude = baseStation.Latitude,
                        Longtitude = baseStation.Longtitude
                    };
                    dal.UpdateBaseStation(baseStation);
                    dal.AddDroneCharge(droneCharge);
                }
                DO.Drone drone = new()
                {
                    DroneID = newDrone.DroneID,
                    DroneModel = newDrone.DroneModel,
                    DroneWeight = (DO.WeightCategories)newDrone.DroneWeight,

                };
                newDrone.BattaryStatus = random.Next(20, 41);
                dal.AddDrone(drone);
                //DroneToList.Add(newDrone);
            }
            catch { }

        }

        public void AddNewCustomer(Customer newCustomer)
        {
            DO.Customer customer = new()
            {
                CustomerId = newCustomer.CustomerId,
                Name = newCustomer.NameCustomer,
                Phone = newCustomer.PhoneCustomer,
                Latitude = newCustomer.LocationCustomer.Latitude,
                Longtitude = newCustomer.LocationCustomer.Longtitude
            };
            try
            {
                dal.AddCustomer(customer);
            }
            catch { }

        }
        public void AddNewParcel(Parcel newParcel, int SenderId, int TargetId)
        {
            try
            {
                newParcel.Drone = null;
                DO.Parcel parcel = new()
                {
                    SenderId = SenderId,
                    TargetId = TargetId,
                    ParcelWeight = (DO.WeightCategories)newParcel.Weight,
                    ParcelPriority = (DO.Priorities)newParcel.Priority,
                    Created = DateTime.Now,
                    Assignment = null,
                    PickedUp = null,
                    Delivered = null
                };
                dal.AddParcel(parcel);
            }
            catch { }
        }
        #endregion

        #region Display LiST
        public IEnumerable<BaseStationToList> GetBasetationToLists()
        {
            List<BaseStationToList> BLStation = new();
            List<DO.BaseStation> DalStation = dal.GetBaseStationByPredicate().ToList();

            foreach (var item in DalStation)
            {
                BLStation.Add(new BaseStationToList
                {
                    ID = item.StationID,
                    Name = item.Name,
                    AvailableChargingStations = item.AvailableChargeSlots,
                    NotAvailableChargingStations = dal.GetDroneChargesByPredicate(x => x.StationID == item.StationID).ToList().Count,
                    location = new()
                    {
                        Latitude = item.Latitude,
                        Longtitude = item.Longtitude
                    }
                });
            };

            return BLStation;
        }

        public IEnumerable<ParcelToList> GetParcelToLists()
        {
            List<DO.Parcel> parcels = dal.GetPackagesByPredicate().ToList();
            List<ParcelToList> BLparcels = new();

            foreach (var item in parcels)
            {
                BLparcels.Add(new ParcelToList
                {
                    Id = item.ParcelId,
                    SenderId = item.SenderId,
                    TargetId = item.TargetId,
                    Weight = (WeightCategories)item.ParcelWeight,
                    Priority = (Priorities)item.ParcelPriority,
                    ParcelAreNotAssighmentToDrone = dal.GetPackagesByPredicate(x => x.DroneId == item.DroneId && x.Assignment == null).ToList().Count
                });
            }
            return BLparcels;
        }

        public IEnumerable<CustomerToList> GetCustomerToList()
        {
            List<DO.Customer> customers = dal.GetCustomersByPredicate().ToList();
            List<CustomerToList> BLCustomer = new();

            foreach (var item in customers)
            {
                BLCustomer.Add(new CustomerToList
                {
                    CustomerId = item.CustomerId,
                    NameCustomer = item.Name,
                    Phone = item.Phone,
                    //SendParcelAndSupplied => Check if SenderId == item.CustomerId & Delivered != Min date 
                    SendParcelAndSupplied = dal.GetPackagesByPredicate(x => x.SenderId == item.CustomerId && x.Delivered != null).ToList().Count,
                    //SendParcelAndNotSupplied => Check if SenderId == CustomerId & Delivered == Min date 
                    SendParcelAndNotSupplied = dal.GetPackagesByPredicate(x => x.SenderId == item.CustomerId && x.Delivered == null).ToList().Count,
                    //ParcelsReciever => Check if TargetId == CustomerId & Delivered == Min date 
                    ParcelsReciever = dal.GetPackagesByPredicate(x => x.TargetId == item.CustomerId && x.Delivered == DateTime.Now).ToList().Count,
                    //ParcelOweyToCustomer => Check if TargetId == item.CustomerId & Delivered == Min date 
                    ParcelOweyToCustomer = dal.GetPackagesByPredicate(x => x.TargetId == item.CustomerId && x.PickedUp == null).ToList().Count
                });
            }
            return BLCustomer;
        }
        #endregion

        public void InitDroneToLists()
        {
            List<DO.Drone> DalDrone = dal.GetDronesByPredicate().ToList();

            foreach (var item in DalDrone)
            {
                DroneToList.Add(new DroneToList
                {
                    DroneID = item.DroneID,
                    DroneModel = item.DroneModel,
                    DroneWeight = (WeightCategories)item.DroneWeight,
                });
            }

            //Arrray of requet power consumption
            double[] temp = dal.RequetPowerConsumption();
            PowerConsumptionAvailable = temp[0];
            PowerConsumptionAvailable = temp[1];
            PowerConsumptionLightWeight = temp[2];
            PowerConsumptionMediumWeight = temp[3];
            PowerConsumptionHeavyWeight = temp[4];

            List<DO.BaseStation> baseStations = dal.GetBaseStationByPredicate().ToList();//get basetation list from Dal layer and create a new Bl list
            List<DO.Parcel> parcels = dal.GetPackagesByPredicate(x => x.DroneId > 0).ToList();// Sorts packages that belong to the drone but are not provided
            List<Customer> customer = new();
            List<DO.Customer> customers = dal.GetCustomersByPredicate().ToList();

            //רשימת תעודות זהות של לקוחות שיש חבילות שסופקו להם
            List<int> parcelThatDeliveredId = parcels.FindAll(x => x.Delivered != null).
                Select(x => x.TargetId).ToList();

            foreach (var item in DroneToList)//For all DroneToList do 
            {
                int index = parcels.FindIndex(x => x.DroneId == item.DroneID && x.Delivered == null);//Index of parcel that assigned but not sent1
                item.CurrentLocation = new();
                if (index != -1)//if have a parcel that assigned but not sent, do 
                {
                    item.Status = DroneStatus.busy;//get the parcel ID that assigned to the drone & set to DroneToList parcel ID that not delivered and change status to busy
                    item.NumOfPackageDelivered = parcels[index].ParcelId;

                    DO.Customer SenderCustomer = dal.GetCustomer(parcels[index].SenderId); //Set parmaters to SenderCustomer & TargetCustomer
                    DO.Customer TargetCustomer = dal.GetCustomer(parcels[index].TargetId);

                    Location Senderlocation = new()
                    {
                        Latitude = SenderCustomer.Latitude,
                        Longtitude = SenderCustomer.Longtitude
                    };
                    Location Targetlocation = new()
                    {
                        Latitude = TargetCustomer.Latitude,
                        Longtitude = TargetCustomer.Longtitude
                    };

                    double battarySenderToTarget = HelpFunction.Distance(TargetCustomer.Latitude, SenderCustomer.Latitude,
                                   TargetCustomer.Longtitude, SenderCustomer.Longtitude);// Battery status is required between the sender and the destination depending on the size of the package

                    switch (parcels[index].ParcelWeight)// Case of parcel weight 
                    {
                        case DO.WeightCategories.light:
                            battarySenderToTarget *= PowerConsumptionLightWeight;
                            break;
                        case DO.WeightCategories.medium:
                            battarySenderToTarget *= PowerConsumptionMediumWeight;
                            break;
                        case DO.WeightCategories.heavy:
                            battarySenderToTarget *= PowerConsumptionHeavyWeight;
                            break;
                        default:
                            break;
                    }
                    battarySenderToTarget += HelpFunction.IndexOfMinDistancesBetweenLocations
                        (baseStations, Targetlocation).Item2 * PowerConsumptionAvailable;// Battery status + = required between the destination and the base station closest to the destination 

                    if (parcels[index].PickedUp == null)//parcel that  not sent
                    {
                        if (baseStations.Count == 0)
                        {
                            throw new Exception("There are no stations in the area");
                        }
                        // Determine the position of the drone at the station closest to the sender in the situation where the package was not collected
                        int indexMinDistance = HelpFunction.IndexOfMinDistancesBetweenLocations(baseStations, Senderlocation).Item1;
                        item.CurrentLocation.Longtitude = baseStations[indexMinDistance].Longtitude;
                        item.CurrentLocation.Latitude = baseStations[indexMinDistance].Latitude;

                        // Battery status required between glider position
                        // which is the location of the base station closest to the sender) and the sender)
                        battarySenderToTarget += HelpFunction.Distance(item.CurrentLocation.Latitude, SenderCustomer.Latitude,
                           item.CurrentLocation.Longtitude, SenderCustomer.Longtitude) * PowerConsumptionAvailable;
                    }
                    else//parcel that sent

                    {
                        item.CurrentLocation.Latitude = SenderCustomer.Latitude;
                        item.CurrentLocation.Longtitude = SenderCustomer.Longtitude;
                    }
                    //  item.BattaryStatus = (random.NextDouble() * (100 - battarySenderToTarget)) + battarySenderToTarget;
                    item.BattaryStatus = random.Next(20, 80);
                }
                else
                {
                    //Index of parcel that assigned but not sent1
                    DroneStatus result = (DroneStatus)random.Next(1, 2);
                    switch (result)
                    {
                        case DroneStatus.available:

                            int indexOfTargetCustomer = parcelThatDeliveredId[random.Next(0, parcelThatDeliveredId.Count)];
                            DO.Customer customer1 = dal.GetCustomer(indexOfTargetCustomer);
                            item.CurrentLocation.Longtitude = customer1.Longtitude;
                            item.CurrentLocation.Latitude = customer1.Latitude;

                            item.BattaryStatus = random.Next((int)(HelpFunction.IndexOfMinDistancesBetweenLocations
                            (baseStations, item.CurrentLocation).Item2 * PowerConsumptionAvailable), 100);
                            break;
                        case DroneStatus.maintenance:

                            int indexRand = random.Next(0, baseStations.Count);
                            item.CurrentLocation = new Location()
                            {
                                Latitude = baseStations[indexRand].Latitude,
                                Longtitude = baseStations[indexRand].Longtitude
                            };
                            item.BattaryStatus = random.Next(0, 20);
                            break;
                        default:
                            break;
                    }

                }
            }
        }

        #region Get Object
        public BaseStation GetBaseStation(int stationID)
        {
            DO.BaseStation DalBaseStation = dal.GetBaseStation(stationID);
            
            BaseStation BLbaseStation = new();
            Location location = new();
            BLbaseStation.location = new();
            BLbaseStation.ID = DalBaseStation.StationID;
            BLbaseStation.location.Latitude = DalBaseStation.Latitude;
            BLbaseStation.location.Longtitude = DalBaseStation.Longtitude;
            BLbaseStation.Name = DalBaseStation.Name;
            BLbaseStation.AvailableChargingStations = DalBaseStation.AvailableChargeSlots;
            BLbaseStation.droneCharges = new List<DroneInCharging>();

            List<DO.DroneCharge> DroneCharges = dal.GetDroneChargesByPredicate(x => x.StationID == stationID).ToList();
            foreach (var item in DroneCharges)
            {
                BLbaseStation.droneCharges.Add(new DroneInCharging
                {
                    DroneID = item.DroneID,
                    BattaryStatus = DroneToList.Find(x => x.DroneID == item.DroneID).BattaryStatus
                });
            }
            return BLbaseStation;
        }

        public Drone GetDrone(int id)
        {
            DroneToList droneToList = DroneToList.Find(x => x.DroneID == id);

            if (droneToList == null)
            {
                throw new Exception("This Drone have not exist, please try again.");
            }

            Drone BLdrone = new();
            BLdrone.DroneID = droneToList.DroneID;
            BLdrone.DroneModel = droneToList.DroneModel;
            BLdrone.DroneWeight = droneToList.DroneWeight;
            BLdrone.BattaryStatus = droneToList.BattaryStatus;
            BLdrone.CurrentLocation = droneToList.CurrentLocation;
            BLdrone.Status = droneToList.Status;

            BLdrone.ParcelInDeliverd = GetParcelInDeliverd(droneToList.NumOfPackageDelivered);
            return BLdrone;
        }

        private ParcelInDeliver GetParcelInDeliverd(int NumOfPackageId)
        {
            Parcel parcel = GetParcel(NumOfPackageId);

            ParcelInDeliver ParcelInDeliverd = new ParcelInDeliver();

            ParcelInDeliverd.ID = parcel.Id;
            ParcelInDeliverd.Sender = parcel.Sender;
            ParcelInDeliverd.Target = parcel.Target;
            ParcelInDeliverd.Priorities = parcel.Priority;
            ParcelInDeliverd.WeightCategories = parcel.Weight;

            DO.Customer SenderCustomer = dal.GetCustomer(parcel.Sender.CustomerId);
            DO.Customer TargetCustomer = dal.GetCustomer(parcel.Target.CustomerId);
            ParcelInDeliverd.CollectionLocation = new Location { Latitude = SenderCustomer.Latitude, Longtitude = SenderCustomer.Longtitude };
            ParcelInDeliverd.DeliveryDestination = new Location { Latitude = TargetCustomer.Latitude, Longtitude = TargetCustomer.Longtitude };

            ParcelInDeliverd.TransportDistance =
                HelpFunction.Distance(ParcelInDeliverd.CollectionLocation.Latitude,
                 ParcelInDeliverd.DeliveryDestination.Latitude,
                 ParcelInDeliverd.CollectionLocation.Longtitude,
                 ParcelInDeliverd.DeliveryDestination.Longtitude);

            return ParcelInDeliverd;
        }

        private ParcelStatus getparcelStatus(DO.Parcel parcel)
        {
            if (parcel.Delivered != null)
            {
                return ParcelStatus.provided;
            }
            if (parcel.PickedUp != null)
            {
                return ParcelStatus.collected;
            }
            if (parcel.Assignment != null)
            {
                return ParcelStatus.associated;
            }
            return ParcelStatus.Defined;
        }

        public ParcelAtCustomer GetParcelAtCustomer(DO.Parcel parcel)
        {
            ParcelAtCustomer parcelAtCustomer = new ParcelAtCustomer();
            parcelAtCustomer.Id = parcel.ParcelId;
            parcelAtCustomer.Priority = (global::BL.Priorities)parcel.ParcelPriority;
            parcelAtCustomer.Weight = (global::BL.WeightCategories)parcel.ParcelWeight;
            parcelAtCustomer.ParcelStatus = getparcelStatus(parcel);

            DO.Customer SenderCustomer = dal.GetCustomer(parcel.SenderId);
            parcelAtCustomer.Sender = new CustomerInParcel();
            parcelAtCustomer.Sender.CustomerId = SenderCustomer.CustomerId;
            parcelAtCustomer.Sender.NameCustomer = SenderCustomer.Name;

            DO.Customer Targetcustomer = dal.GetCustomer(parcel.TargetId);
            parcelAtCustomer.Target = new CustomerInParcel();
            parcelAtCustomer.Sender.CustomerId = Targetcustomer.CustomerId;
            parcelAtCustomer.Target.NameCustomer = Targetcustomer.Name;

            return parcelAtCustomer;
        }

        private IEnumerable<ParcelAtCustomer> GetParcelAtCustomers(IEnumerable<DO.Parcel> DalPackagesIdCustomer,
            int id, bool flag)
        {
            return from package in DalPackagesIdCustomer
                   where flag ? package.SenderId == id : package.TargetId == id
                   select GetParcelAtCustomer(package);
        }

        public Customer GetCustomer(int id)
        {
            DO.Customer DalCustomer = dal.GetCustomer(id);
            Customer BLCustomer = new();

            BLCustomer.LocationCustomer = new();
            BLCustomer.CustomerId = DalCustomer.CustomerId;
            BLCustomer.NameCustomer = DalCustomer.Name;
            BLCustomer.PhoneCustomer = DalCustomer.Phone;
            BLCustomer.LocationCustomer.Latitude = DalCustomer.Latitude;
            BLCustomer.LocationCustomer.Longtitude = DalCustomer.Longtitude;

            //packages that the customer send
            BLCustomer.PackagesFromCustomer = new List<ParcelAtCustomer>();
            BLCustomer.PackagesToCustomer = new List<ParcelAtCustomer>();

            IEnumerable<DO.Parcel> DalPackagesIdCustomer = dal.GetPackagesByPredicate
                (x => x.SenderId == id || x.TargetId == id);

            //packages that the customer recieve
            BLCustomer.PackagesFromCustomer = GetParcelAtCustomers(DalPackagesIdCustomer, id, true).ToList();

            //packages that the customer get
            BLCustomer.PackagesToCustomer = GetParcelAtCustomers(DalPackagesIdCustomer, id, false).ToList();

            return BLCustomer;
        }

        public Parcel GetParcel(int id)
        {
            DO.Parcel DalParcel = dal.GetParcel(id);

            Parcel BLParcel = new();

            BLParcel.Id = DalParcel.ParcelId;
            BLParcel.Sender = GetCustomer(DalParcel.SenderId);
            BLParcel.Target = GetCustomer(DalParcel.TargetId);
            BLParcel.Weight = (WeightCategories)DalParcel.ParcelWeight;
            BLParcel.Priority = (Priorities)DalParcel.ParcelPriority;
            BLParcel.CreateTime = DalParcel.Created;
            BLParcel.Requested = DalParcel.Assignment;
            BLParcel.PickedUp = DalParcel.PickedUp;
            BLParcel.Delivered = DalParcel.Delivered;


            if (DalParcel.DroneId != 0)
            {
                DroneToList drone = DroneToList.Find(i => i.DroneID == DalParcel.DroneId);
                BLParcel.Drone = new DroneInParcel();
                BLParcel.Drone.DroneID = drone.DroneID;
                BLParcel.Drone.BattaryStatus = drone.BattaryStatus;
                BLParcel.Drone.CorrentLocation = new Location()
                {
                    Latitude = drone.CurrentLocation.Latitude,
                    Longtitude = drone.CurrentLocation.Longtitude
                };
            }

            return BLParcel;
        }

        public User GetUser(string userName)
        {

            try
            {
                DO.User DalUser = dal.GetUser(userName);
                User BlUser = new User();
                BlUser.FirstName = DalUser.FirstName;
                BlUser.LastName = DalUser.LastName;
                BlUser.Password = DalUser.Password;
                BlUser.UserId = DalUser.UserId;
                BlUser.UserName = DalUser.UserName;

                return BlUser;
            }
            catch (Exception)
            {

                throw new Exception("The User not exist");
            }
        }
        #endregion

        #region Update Object
        public void UpdateDrone(int id, string newNameModel)
        {
            try
            {
                DO.Drone drones = dal.GetDrone(id);
                drones.DroneModel = newNameModel;
                dal.UpdateDrone(drones);
                DroneToList.Find(x => x.DroneID == id).DroneModel = newNameModel;
            }
            catch (DalApi.DO.DroneException)
            {
                throw new Exception("");
            }

        }

        public void UpdateBaseStation(int stationId, string newNameStation, int sumOfChargestation)
        {
            try
            {
                DO.BaseStation baseStation = dal.GetBaseStation(stationId);
                BaseStation station = new();

                baseStation.Name = newNameStation;

                //check sum of available ChargeSlots + sum of unavailable ChargeSlots
                if (baseStation.AvailableChargeSlots + stationId < DroneToList.Count)
                    baseStation.AvailableChargeSlots = sumOfChargestation - baseStation.AvailableChargeSlots + DroneToList.Count;//לבדוק איך סוכמים

                dal.UpdateBaseStation(baseStation);
            }
            catch (DalApi.DO.BaseStationException)
            {

                throw new Exception(" ");
            }
        }

        public void UpdateCustomr(int customerId, string newNameCustomer, string newPhoneCustomer)
        {
            try
            {
                DO.Customer customer = dal.GetCustomer(customerId);
                customer.Name = newNameCustomer;
                customer.Phone = newPhoneCustomer;

                dal.UpdateCustomer(customer);
            }
            catch (DalApi.DO.CustumerException)
            {

                throw new Exception(" ");
            }
        }
        #endregion

        #region Drone operation
        public void SendDroneToCharge(int droneId, int stationId)
        {
            DroneToList droneToCharge = DroneToList.Find(x => x.DroneID == droneId);
            if (droneToCharge == null)
            {
                throw new Exception("WRONG!!!, This Drone have not exist");
            }
            if (droneToCharge.Status != DroneStatus.available)
            {
                throw new Exception("Only available Drone can be sending to charge");
            }

            DO.BaseStation baseStation = dal.GetBaseStation(stationId);
            BaseStation _baseStation = new BaseStation()
            {
                ID = baseStation.StationID,
                Name = baseStation.Name,
                AvailableChargingStations = baseStation.AvailableChargeSlots,
                location = new Location { Latitude = baseStation.Latitude, Longtitude = baseStation.Longtitude }
            };

            double distanceBetweenStationToDrone = HelpFunction.Distance(_baseStation.location.Latitude
                          , droneToCharge.CurrentLocation.Latitude, _baseStation.location.Longtitude,
                         droneToCharge.CurrentLocation.Longtitude) * PowerConsumptionAvailable;

            if (distanceBetweenStationToDrone == default)
            {
                throw new Exception("WRONG!, There are not enough battery to complete this mission");
            }

            droneToCharge.BattaryStatus -= distanceBetweenStationToDrone;
            droneToCharge.CurrentLocation = _baseStation.location;
            droneToCharge.Status = DroneStatus.maintenance;
            dal.MinusDroneCharge(stationId);// Lowering the number of charging stations by 1
            dal.ChargeDrone(droneId, stationId);//  Create an instance in the datasource
        }

        public void ReleaseDroneFromCharge(int droneId, int stationId, DateTime dateTime)
        {
            DroneToList drone = DroneToList.Find(x => x.DroneID == droneId);
            if (drone == null)
            {
                throw new Exception("That Drone has not found, Please try again");
            }
            if (drone.Status != DroneStatus.maintenance)
            {
                throw new Exception("Only Drone in maintanance can be released");
            }

            double TimeInCharged = dateTime.Hour + (dateTime.Minute % 60 + dateTime.Second % 3600);
            double SumOfCharged = TimeInCharged * LoadingDrone + drone.BattaryStatus;
            if (SumOfCharged >= 100)
            {
                SumOfCharged = 100;
            }
            drone.BattaryStatus = SumOfCharged;
            drone.Status = DroneStatus.available;

            dal.PlusDroneCharge(stationId);  // Raising the number of charging stations by 1

            dal.ReleasingChargeDrone(droneId, stationId);  // remove an instance in the datasource

        }

        public void AssignmentOfPackageToDrone(int droneId, ParcelInDeliver parcelInDeliver) //שיוך חבילה לרחפן
        {
            DroneToList drone = DroneToList.Find(x => x.DroneID == droneId);
            List<DO.Parcel> parcels = dal.GetPackagesByPredicate(x => (int)x.ParcelWeight < (int)drone.DroneWeight).OrderBy(i => (int)i.ParcelPriority)
                .ThenBy(i => (int)i.ParcelWeight).ToList();// לבדוק עם יהודה לגבי תנאי הסינון
            if (drone.Status == DroneStatus.available)
            {
                foreach (var parcel in parcels)
                {
                    if (BatteryStatusBetweenAllLocation(parcel, drone))
                    {
                        drone.Status = DroneStatus.busy;
                        drone.NumOfPackageDelivered = parcel.ParcelId;
                        dal.SetDroneForParcel(parcel.ParcelId, droneId);
                        break;
                    }
                    else
                    {
                        throw new Exception("There is no Parcel ");
                    }
                }
            }
            else
                throw new Exception("The Drone is not available");
        }

        private bool BatteryStatusBetweenAllLocation(DO.Parcel parcel, DroneToList drones)
        {

            double BattaryStatus = HelpFunction.Distance(drones.CurrentLocation.Latitude,
                                   GetCustomer(parcel.SenderId).LocationCustomer.Latitude,
                                   drones.CurrentLocation.Longtitude, GetCustomer(parcel.SenderId).LocationCustomer.Longtitude)
                                   * PowerConsumptionAvailable;
            double diSenderToTarget = HelpFunction.Distance(GetCustomer(parcel.SenderId).LocationCustomer.Latitude, GetCustomer(parcel.TargetId).LocationCustomer.Latitude,
                GetCustomer(parcel.SenderId).LocationCustomer.Longtitude, GetCustomer(parcel.TargetId).LocationCustomer.Longtitude);
            switch ((WeightCategories)parcel.ParcelWeight)
            {
                case WeightCategories.light:
                    BattaryStatus += diSenderToTarget * PowerConsumptionLightWeight;
                    break;
                case WeightCategories.medium:
                    BattaryStatus += diSenderToTarget * PowerConsumptionMediumWeight;
                    break;
                case WeightCategories.heavy:
                    BattaryStatus += diSenderToTarget * PowerConsumptionHeavyWeight;
                    break;
                default:
                    break;
            }
            List<BaseStation> baseStationsBL = new List<BaseStation>();
            List<DO.BaseStation> baseStationsDAL = dal.GetBaseStationByPredicate().ToList();

            BattaryStatus += HelpFunction.IndexOfMinDistancesBetweenLocations(baseStationsDAL, GetCustomer(parcel.TargetId).LocationCustomer).Item2;
            if (drones.BattaryStatus - BattaryStatus < 0)
            {
                return false;
            }
            return true;
        }


        public void CollectParcelByDrone(int droneID)
        {
            DroneToList drones = DroneToList.Find(x => x.DroneID == droneID);
            if (drones == default)
            {
                throw new Exception("This Drone have not exist");
            }
            if (drones.NumOfPackageDelivered == 0)
            {
                throw new Exception("This Drone is not assinged");
            }
            DO.Parcel parcels = dal.GetParcel(drones.NumOfPackageDelivered);
            if (parcels.PickedUp == null)
            {
                Location location = GetCustomer(parcels.SenderId).LocationCustomer;
                drones.BattaryStatus -= HelpFunction.Distance(drones.CurrentLocation.Latitude, location.Latitude, drones.CurrentLocation.Longtitude, location.Longtitude) * PowerConsumptionAvailable;
                drones.CurrentLocation = location;
                dal.PackageCollectionByDrone(parcels.ParcelId, droneID);
            }
            else
            {
                throw new Exception("This Parcel have been collected");
            }
        }
        #endregion

        public IEnumerable<DroneToList> GetDroneToListsBLByPredicate(Predicate<DroneToList> predicate = null)
        {
            return DroneToList.FindAll(i => predicate == null ? true : predicate(i));
        }
        #region Remove Object
        public void RemoveBaseStationBL(int id)
        {
            //IDAL.DO.BaseStation baseStationRemove = dal.GetBaseStation(id);
            dal.RemoveBaseStation(id);
        }

        public void RemoveDroneBL(int id)
        {
            //IDAL.DO.BaseStation baseStationRemove = dal.GetBaseStation(id);
            dal.RemoveDrone(id);
        }

        public void RemoveCustomerBL(int id)
        {
            //IDAL.DO.BaseStation baseStationRemove = dal.GetBaseStation(id);
            dal.RemoveCustomer(id);
        }

        public void RemoveParcelBL(int id)
        {
            //IDAL.DO.BaseStation baseStationRemove = dal.GetBaseStation(id);
            dal.RemoveParcel(id);
        }
        #endregion
    }
}