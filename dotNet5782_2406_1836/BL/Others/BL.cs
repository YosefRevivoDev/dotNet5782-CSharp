using System;
using System.Collections.Generic;
using System.Linq;

namespace BO
{
    public class BL : IBL
    {
        readonly IDAL.IDal dal; // Dal object to invite DAL functions 
        public List<DroneToList> DroneToList { get; set; } //Dynamic drone's list at BL layer
        static Random random = new(DateTime.Now.Millisecond);
        static double PowerConsumptionAvailable;
        static double PowerConsumption_LightWeight;
        static double PowerConsumption_MediumWeight;
        static double PowerConsumption_HeavyWeight;
        static double LoadingDrone;

        public BL()
        {
            dal = new DalObject.DalObject(); // Access to summon methods from Datasource
            DroneToList = new List<DroneToList>();
            InitDroneToLists(); // Initialize the list from DAL layer 
        }
        //------------------------------------- Add functions------------------------//
        public void AddBaseStation(BaseStation newBaseStation)
        {
            //Set drone information from DAL layer 
            IDAL.DO.BaseStation baseStation = new()
            {
                StationID = newBaseStation.ID,
                Name = newBaseStation.Name,
                Longtitude = newBaseStation.Location.Longtitude,
                Latitude = newBaseStation.Location.Latitude,
                AvailableChargeSlots = newBaseStation.AvailableChargingStations,
            };
            try
            {
                dal.Add_BaseStation(baseStation);
            }
            catch { }
        }
        public void AddNewDrone(DroneToList newDrone, int NumberOfStation) 
        {
            try
            {
                //Get bastation from Dal with ID key 
                IDAL.DO.BaseStation baseStation = dal.GetBaseStation(NumberOfStation); 

                if (baseStation.AvailableChargeSlots > 0)
                {
                    IDAL.DO.DroneCharge droneCharge = new IDAL.DO.DroneCharge
                    {
                        DroneID = newDrone.DroneID,
                        StationID = baseStation.StationID
                    };
                    newDrone.Status = DroneStatus.maintenance;
                    baseStation.AvailableChargeSlots--;
                    newDrone.CurrentLocation = new Location();
                    newDrone.CurrentLocation.Latitude = baseStation.Latitude;
                    newDrone.CurrentLocation.Longtitude = baseStation.Longtitude;
                    dal.UpdateBaseStation(baseStation);
                    dal.Add_DroneCharge(droneCharge);
                }
                IDAL.DO.Drone drone = new()
                {
                    DroneID = newDrone.DroneID,
                    DroneModel = newDrone.DroneModel,
                    DroneWeight = (IDAL.DO.WeightCategories)newDrone.DroneWeight,
                };
                newDrone.BattaryStatus = random.Next(20, 41);
                dal.Add_Drone(drone);
                DroneToList.Add(newDrone);
            }
            catch { }

        }

        public void AddNewCustomer(Customer newCustomer)
        {
            IDAL.DO.Customer customer = new()
            {
                CustomerId = newCustomer.CustomerId,
                Name = newCustomer.Name,
                Phone = newCustomer.PhoneCustomer,
                Latitude = newCustomer.LocationCustomer.Latitude,
                Longtitude = newCustomer.LocationCustomer.Longtitude
            };
            try
            {
                dal.Add_Customer(customer);
            }
            catch { }

        }
        public void AddNewParcel(Parcel newParcel, int SenderId, int TargetId)
        {
            try
            {
                newParcel.Drone = null;
                IDAL.DO.Parcel parcel = new()
                {
                    SenderId = SenderId,
                    TargetId = TargetId,
                    Parcel_weight = (IDAL.DO.WeightCategories)newParcel.weight,
                    ParcelPriority = (IDAL.DO.Priorities)newParcel.Priority,
                    Created = DateTime.Now,
                    Assignment = DateTime.MinValue,
                    PickedUp = DateTime.MinValue,
                    Delivered = DateTime.MinValue
                };
                dal.Add_Parcel(parcel);
            }
            catch { }
        }

        //-----------------------Display list optaions------------------//

        public IEnumerable<BasetationToList> GetBasetationToLists()
        {
            List<BasetationToList> BLStation = new List<BasetationToList>();
            List<IDAL.DO.BaseStation> DalStation = dal.GetBaseStationByPredicate().ToList();

            foreach (var item in DalStation)
            {
                BLStation.Add(new BasetationToList
                {
                    ID = item.StationID,
                    Name = item.Name,
                    AvailableChargingStations = item.AvailableChargeSlots,
                    NotAvailableChargingStations = dal.GetDroneChargesByPredicate(x => x.StationID == item.StationID).ToList().Count,    
                });
            }
            return BLStation;
        }
        public void InitDroneToLists()
        {
            List<IDAL.DO.Drone> DalDrone = dal.GetDronesByPredicate().ToList();

            foreach (var item in DalDrone)
            {
                DroneToList.Add(new DroneToList
                {
                    DroneID = item.DroneID,
                    DroneModel = item.DroneModel,
                    DroneWeight = (BO.WeightCategories)item.DroneWeight,
                });
            }

            //Arrray of requet power consumption
            double[] temp = dal.RequetPowerConsumption();
            PowerConsumptionAvailable = temp[0];
            PowerConsumptionAvailable= temp[1];
            PowerConsumption_LightWeight= temp[2];
            PowerConsumption_MediumWeight= temp[3];
            PowerConsumption_HeavyWeight= temp[4];

            //get basetation list from Dal layer and create a new Bl list
            List<IDAL.DO.BaseStation> baseStations = dal.GetBaseStationByPredicate().ToList();

            // Sorts packages that belong to the drone but are not provided
            List<IDAL.DO.Parcel> parcels =
                dal.GetPackagesByPredicate(x => x.DroneId != 0).ToList();

            foreach (var item in this.DroneToList)//For all DroneToList do 
            {
                //Index of parcel that assigned but not sent
                int index = parcels.FindIndex(x => x.DroneId == item.DroneID && x.Delivered == DateTime.MinValue);

                if (index != -1)//if have a parcel that assigned but not sent, do 
                {
                    //get the parcel ID that assigned to the drone & set to DroneToList parcel ID that not delivered and change status to busy
                    item.NumOfPackageDelivered = parcels[index].ParcelId; 
                    item.Status = DroneStatus.busy;

                    //Set parmaters to SenderCustomer & TargetCustomer

                    IDAL.DO.Customer SenderCustomer = dal.GetCustomer(parcels[index].SenderId);
                    IDAL.DO.Customer TargetCustomer = dal.GetCustomer(parcels[index].TargetId);

                    Location Senderlocation = new Location
                    {
                        Latitude = SenderCustomer.Latitude,
                        Longtitude =
                        SenderCustomer.Longtitude
                    };
                    Location Targetlocation = new Location
                    {
                        Latitude = TargetCustomer.Latitude,
                        Longtitude =
                        TargetCustomer.Longtitude
                    };

                    // Battery status is required between the sender and the destination depending on the size of the package
                    double battarySenderToTarget = BO.HelpFunction.Distance(TargetCustomer.Latitude, SenderCustomer.Latitude,
                                   TargetCustomer.Longtitude, SenderCustomer.Longtitude);

                    // Case of parcel weight 
                    switch (parcels[index].Parcel_weight)
                    {
                        case IDAL.DO.WeightCategories.light:
                            battarySenderToTarget *= PowerConsumption_LightWeight;
                            break;
                        case IDAL.DO.WeightCategories.medium:
                            battarySenderToTarget *= PowerConsumption_MediumWeight;
                            break;
                        case IDAL.DO.WeightCategories.heavy:
                            battarySenderToTarget *= PowerConsumption_HeavyWeight;
                            break;
                        default:
                            break;
                    }
                    // Battery status + = required between the destination and the base station closest to the destination 
                    battarySenderToTarget += BO.HelpFunction.IndexOfMinDistancesBetweenLocations
                        (baseStations, Targetlocation).Item2 * PowerConsumptionAvailable;

                    if (parcels[index].PickedUp == DateTime.MinValue)//parcel that  not sent
                    {
                        if (baseStations.Count == 0) // לבדוק אם זה false
                        {
                            throw new Exception("There are no stations in the area");
                        }
                        // Determine the position of the skimmer at the station closest to the sender in the situation where the package was not collected
                        int indexMinDistance = BO.HelpFunction.IndexOfMinDistancesBetweenLocations(baseStations, Senderlocation).Item1;
                        item.CurrentLocation.Longtitude = baseStations[indexMinDistance].Longtitude;
                        item.CurrentLocation.Latitude = baseStations[indexMinDistance].Latitude;

                        // Battery status required between glider position
                        // which is the location of the base station closest to the sender) and the sender)
                        battarySenderToTarget += BO.HelpFunction.Distance(item.CurrentLocation.Latitude, SenderCustomer.Latitude,
                           item.CurrentLocation.Longtitude, SenderCustomer.Longtitude) * PowerConsumptionAvailable;
                    }

                    else
                    {
                        //parcel that sent
                        item.CurrentLocation.Latitude = SenderCustomer.Latitude;
                        item.CurrentLocation.Longtitude = SenderCustomer.Longtitude;
                    }
                    item.BattaryStatus = (random.NextDouble() * (100 - battarySenderToTarget)) + battarySenderToTarget;// לשאול את יהודה
                }
            }
        }

        public IEnumerable<CustomerToList> GetCustomerToList()
        {
            List<IDAL.DO.Customer> customers = dal.GetCustomersByPredicate().ToList();
            List<CustomerToList> BLCustomer = new List<CustomerToList>();

            foreach (var item in customers)
            {
                BLCustomer.Add(new CustomerToList
                {
                    CustomerId = item.CustomerId,
                    NameCustomer = item.Name,
                    Phone = item.Phone,
                    //SendParcelAndSupplied => Check if SenderId == item.CustomerId & Delivered != Min date 
                    SendParcelAndSupplied = dal.GetPackagesByPredicate(x => x.SenderId == item.CustomerId && x.Delivered != DateTime.MinValue).ToList().Count,
                    //SendParcelAndNotSupplied => Check if SenderId == CustomerId & Delivered == Min date 
                    SendParcelAndNotSupplied = dal.GetPackagesByPredicate(x => x.SenderId == item.CustomerId && x.Delivered == DateTime.MinValue).ToList().Count,
                    //ParcelsReciever => Check if TargetId == CustomerId & Delivered == Min date 
                    ParcelsReciever = dal.GetPackagesByPredicate(x => x.TargetId == item.CustomerId && x.Delivered == DateTime.Now).ToList().Count,
                    //ParcelOweyToCustomer => Check if TargetId == item.CustomerId & Delivered == Min date 
                    ParcelOweyToCustomer = dal.GetPackagesByPredicate(x => x.TargetId == item.CustomerId && x.PickedUp == DateTime.MinValue).ToList().Count
                });
            }
            return BLCustomer;
        }

        public IEnumerable<ParcelToList> GetParcelToLists()
        {
            List<IDAL.DO.Parcel> parcels = dal.GetPackagesByPredicate().ToList();
            List<ParcelToList> BLparcels = new List<ParcelToList>();

            foreach (var item in parcels)
            {
                BLparcels.Add(new ParcelToList
                {
                    Id = item.ParcelId,
                    SenderId = item.SenderId,
                    TargetId = item.TargetId,
                    Weight = (BO.WeightCategories)item.Parcel_weight,
                    Priority = (BO.Priorities)item.ParcelPriority,
                    //ParcelStatus = , // לבדוק עם יהודה מה צריך לעשות פה ,
                    ParcelAreNotAssighmentToDrone = dal.GetPackagesByPredicate(x => x.DroneId == item.DroneId && x.Assignment == DateTime.MinValue).ToList().Count
                });
            }
            return BLparcels;
        }

        // ----------------- Display Object ---------------------//

        public BaseStation GetBaseStation(int stationID)
        {
            IDAL.DO.BaseStation DalBaseStation = dal.GetBaseStation(stationID);
            BaseStation BLbaseStation = new BaseStation();
            BLbaseStation.Location = new();
            BLbaseStation.ID = DalBaseStation.StationID;
            BLbaseStation.Location.Latitude = DalBaseStation.Latitude;
            BLbaseStation.Location.Longtitude = DalBaseStation.Latitude;
            BLbaseStation.Name = DalBaseStation.Name;
            BLbaseStation.AvailableChargingStations = DalBaseStation.AvailableChargeSlots;
            BLbaseStation.droneCharges = new List<DroneCharge>();

            List<IDAL.DO.DroneCharge> DroneCharges = dal.GetDroneChargesByPredicate(x => x.StationID == stationID).ToList();
            foreach (var item in DroneCharges)
            {
                BLbaseStation.droneCharges.Add(new DroneCharge
                {
                    DroneID = item.DroneID,
                    BattaryStatus = DroneToList.Find(x => x.DroneID == item.DroneID).BattaryStatus
                });
            }
            return BLbaseStation;
        }

        public Drone GetDrone(int id)
        {
            //      Drone BLdrone = DroneToList[DroneToList.FindIndex(x => x.DroneID == id)];
            Drone BLdrone = new Drone();
            IDAL.DO.Drone drone = dal.GetDrone(id);

            if (drone.DroneID == -1) { throw new Exception("This Drone have not exist, please try again."); }

            return BLdrone;
        }

        public Customer GetCustomer(int id)
        {
            IDAL.DO.Customer DalCustomer = dal.GetCustomer(id);
            Customer BLCustomer = new Customer();
            BLCustomer.LocationCustomer = new();
            BLCustomer.CustomerId = DalCustomer.CustomerId;
            BLCustomer.NameCustomer = DalCustomer.Name;
            BLCustomer.PhoneCustomer = DalCustomer.Phone;
            BLCustomer.LocationCustomer.Latitude = DalCustomer.Latitude;
            BLCustomer.LocationCustomer.Longtitude = DalCustomer.Longtitude;

            //packages that the customer send
            BLCustomer.PackagesFromCustomer = new List<Parcel>();
            List<IDAL.DO.Parcel> DalPackagesFromCustomer = dal.GetPackagesByPredicate(x => x.SenderId == id).ToList();
            foreach (var item in DalPackagesFromCustomer)
            {
                BLCustomer.PackagesFromCustomer.Add(GetParcel(item.ParcelId));
            }

            //packages that the customer recieve
            BLCustomer.PackagesToCustomer = new List<Parcel>();
            List<IDAL.DO.Parcel> DalPackagesToCustomer = dal.GetPackagesByPredicate(x => x.TargetId == id).ToList();
            foreach (var item in DalPackagesToCustomer)
            {
                BLCustomer.PackagesToCustomer.Add(GetParcel(item.SenderId));
            }
            return BLCustomer;
        }

        public Parcel GetParcel(int id)
        {
            IDAL.DO.Parcel DalParcel = dal.GetParcel(id);
            Parcel BLParcel = new Parcel();
            DroneToList drone = DroneToList.Find(i => i.DroneID == DalParcel.DroneId);
            BLParcel.Id = DalParcel.ParcelId;
            BLParcel.Sender = GetCustomer(DalParcel.SenderId);
            BLParcel.Target = GetCustomer(DalParcel.TargetId);
            BLParcel.weight = (BO.WeightCategories)DalParcel.Parcel_weight;
            BLParcel.Priority = (BO.Priorities)DalParcel.ParcelPriority;
            BLParcel.CreateTime = DalParcel.Created;
            BLParcel.Requested = DalParcel.Assignment;
            BLParcel.PickedUp = DalParcel.PickedUp;
            BLParcel.Delivered = DalParcel.Delivered;
            BLParcel.Drone.DroneID = drone.DroneID;
            BLParcel.Drone.BattaryStatus = drone.BattaryStatus;
            BLParcel.Drone.CorrentLocation = new Location
            {
                Latitude = drone.CurrentLocation.Latitude,
                Longtitude = drone.CurrentLocation.Longtitude
            }; 
            return BLParcel;
        }

        //-------------------------------------------- UPDATE FUNC ------------------------------// 

        public void UpadateDrone(int id, string newNameModel)
        {
            try
            {
                IDAL.DO.Drone drones = dal.GetDrone(id);
                drones.DroneModel = newNameModel;
                dal.UpdateDrone(drones);
            }
            catch (IDAL.DO.DroneException)
            {

                throw new Exception("");
            }
            DroneToList.Find(x => x.DroneID == id).DroneModel = newNameModel;
        }

        public void UpdateBaseStation(int stationId, string newNameStation, int sumOfChargestation)
        {
            try
            {
                IDAL.DO.BaseStation baseStation = dal.GetBaseStation(stationId);
                BaseStation station = new BaseStation();

                baseStation.Name = newNameStation;

                //check sum of available ChargeSlots + sum of unavailable ChargeSlots
                if (baseStation.AvailableChargeSlots + stationId < DroneToList.Count)
                    baseStation.AvailableChargeSlots = sumOfChargestation - baseStation.AvailableChargeSlots + DroneToList.Count;//לבדוק איך סוכמים

                dal.UpdateBaseStation(baseStation);
            }
            catch (IDAL.DO.BaseStationException)
            {

                throw new Exception(" ");
            }
        }

        public void UpdateCustomr(int customerId, string newNameCustomer, string newPhoneCustomer)
        {
            try
            {
                IDAL.DO.Customer customer = dal.GetCustomer(customerId);
                customer.Name = newNameCustomer;
                customer.Phone = newPhoneCustomer;

                dal.UpdateCustomer(customer);
            }
            catch (IDAL.DO.CustumerException)
            {

                throw new Exception(" ");
            }
            // האם להוסיף למערך כלשהו?
        }

        public void SendDroneToCharge(int droneId)
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
            List<IDAL.DO.BaseStation> Dalstations = dal.GetBaseStationByPredicate(x => x.AvailableChargeSlots > 0).ToList();
            List<BaseStation> BLStations = new();
            foreach (var item in Dalstations)
            {
                BLStations.Add(new BaseStation
                {
                    ID = item.StationID,
                    Name = item.Name,
                    AvailableChargingStations = item.AvailableChargeSlots,
                    Location = new Location { Latitude = item.Latitude, Longtitude = item.Longtitude }
                });
            }
            double distanceBetweenStationToDrone =
                            HelpFunction.IndexOfMinDistancesBetweenLocations(Dalstations, droneToCharge.CurrentLocation).Item2;
            if (droneToCharge.BattaryStatus - distanceBetweenStationToDrone * PowerConsumptionAvailable < 0)
            {
                throw new Exception("WRONG!, There are not enough battery to complete this mission");
            }
            droneToCharge.BattaryStatus -=
                distanceBetweenStationToDrone * PowerConsumptionAvailable;
            droneToCharge.CurrentLocation =
                BLStations[HelpFunction.IndexOfMinDistancesBetweenLocations(Dalstations, droneToCharge.CurrentLocation).Item1].Location;
            droneToCharge.Status = DroneStatus.maintenance;





            IDAL.DO.Drone drone = dal.GetDrone(droneId);
            if (true)
            {

            }
        }

        public void ReleaseDroneFromCharge(int droneId, DateTime dateTime)
        {
            DroneToList drone = DroneToList.Find(x => x.DroneID == droneId);
            if (drone == null)
            {
                throw new Exception("That Drone has not found");
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

           

        }

        public void PackageCollectionByDrone(int pacel_id_associate, int drone_id_associate)
        {
            throw new NotImplementedException();
        }

        public void DeliveredPackageToCustumer(int parcelIdAssociate, int droneIdAssociate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BasetationToList> GetBasetationToLists(Predicate<BasetationToList> predicate = null)
        {
            throw new NotImplementedException();
        }
    }

}
