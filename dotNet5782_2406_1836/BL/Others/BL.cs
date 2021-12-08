﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BO
{
    public partial class BL : IBL.IBL
    {
        IDAL.IDal dal;
        public List<DroneToList> DroneToList { get; set; }
        static Random random = new(DateTime.Now.Millisecond);
        public static double PowerConsumptionAvailable;
        public static double PowerConsumption_LightWeight;
        public static double PowerConsumption_MediumWeight;
        public static double PowerConsumption_HeavyWeight;
        public static double LoadingDrone;

        public BL()
        {
            dal = new DalObject.DalObject(); // Access to summon methods from Datasource
            DroneToList = new List<DroneToList>();
            InitDroneToLists();
        }
        //------------------------------------- Add functions------------------------//
        public void AddBaseStation(BaseStation newBaseStation)
        {
            IDAL.DO.BaseStation baseStation = new()
            {
                StationID = newBaseStation.ID,
                Name = newBaseStation.Name,
                Longtitude = newBaseStation.Location.Longtitude,
                Latitude = newBaseStation.Location.Latitude,
                AvailableChargeSlots = newBaseStation.AvailableChargingStations,// charge stations
            };
            try
            {
                dal.AddBaseStation(baseStation);
            }
            catch { }
        }
        public void AddNewDrone(DroneToList newDrone, int NumberOfStation)// 
        {
            try
            {
                IDAL.DO.BaseStation baseStation = dal.GetBaseStation(NumberOfStation);
                if (baseStation.AvailableChargeSlots > 0)
                {
                    IDAL.DO.DroneCharge droneCharge = new()
                    {
                        DroneID = newDrone.DroneID,
                        StationID = baseStation.StationID
                    };
                    newDrone.Status = DroneStatus.maintenance;
                    baseStation.AvailableChargeSlots--;
                    newDrone.CurrentLocation = new();
                    newDrone.CurrentLocation.Latitude = baseStation.Latitude;
                    newDrone.CurrentLocation.Longtitude = baseStation.Longtitude;
                    dal.UpdateBaseStation(baseStation);
                    dal.AddDroneCharge(droneCharge);
                }
                IDAL.DO.Drone drone = new()
                {
                    DroneID = newDrone.DroneID,
                    DroneModel = newDrone.DroneModel,
                    DroneWeight = (IDAL.DO.WeightCategories)newDrone.DroneWeight,
                };
                newDrone.BattaryStatus = random.Next(20, 41);
                dal.AddDrone(drone);
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
                dal.AddCustomer(customer);
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
                dal.AddParcel(parcel);
            }
            catch { }
        }

        //-----------------------Display list optaions------------------//

        public IEnumerable<BasetationToList> GetBasetationToLists()
        {
            List<BasetationToList> BLStation = new();
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
                    DroneWeight = (DroneWeightCategories)item.DroneWeight,
                });
            }

            double[] temp = dal.RequetPowerConsumption();
            PowerConsumptionAvailable = temp[0];
            PowerConsumption_LightWeight = temp[1];
            PowerConsumption_MediumWeight = temp[2];
            PowerConsumption_HeavyWeight = temp[3];

            List<IDAL.DO.BaseStation> baseStations = dal.GetBaseStationByPredicate().ToList();

            List<IDAL.DO.Parcel> parcels =
                dal.GetPackagesByPredicate(x => x.DroneId != 0).ToList();// Sorts packages that belong to the drone but are not provided
            foreach (var item in this.DroneToList)
            {
                int index = parcels.FindIndex(x => x.DroneId == item.DroneID && x.Delivered == DateTime.MinValue);
                if (index != -1)
                {
                    item.NumOfPackageDelivered = parcels[index].ParcelId;
                    item.Status = DroneStatus.busy;

                    IDAL.DO.Customer SenderCustomer = dal.GetCustomer(parcels[index].SenderId);
                    IDAL.DO.Customer TargetCustomer = dal.GetCustomer(parcels[index].TargetId);

                    Location Senderlocation = new()
                    {
                        Latitude = SenderCustomer.Latitude,
                        Longtitude =
                        SenderCustomer.Longtitude
                    };
                    Location Targetlocation = new()
                    {
                        Latitude = TargetCustomer.Latitude,
                        Longtitude =
                        TargetCustomer.Longtitude
                    };

                    // מצב סוללה נדרשת בין השולח ליעד בהתאם לגודל החבילה
                    double battarySenderToTarget = Distance(TargetCustomer.Latitude, SenderCustomer.Latitude,
                                   TargetCustomer.Longtitude, SenderCustomer.Longtitude);

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
                    // מצב סוללה += הנדרשת בין ליעד לתחנת בסיס הקרובה ביותר ליעד 
                    battarySenderToTarget += IndexOfMinDistancesBetweenLocations
                        (baseStations, Targetlocation).Item2 * PowerConsumptionAvailable;

                    if (parcels[index].PickedUp == DateTime.MinValue)
                    {
                        if (baseStations.Count == 0)
                        {
                            throw new Exception("There are no stations in the area");
                        }
                        // קביעת מיקום הרחפן בתחנה הקרובה ביותר לשולח במצב שהחבילה לא נאספה
                        int indexMinDistance = IndexOfMinDistancesBetweenLocations(baseStations, Senderlocation).Item1;
                        item.CurrentLocation.Longtitude = baseStations[indexMinDistance].Longtitude;
                        item.CurrentLocation.Latitude = baseStations[indexMinDistance].Latitude;

                        // מצב סוללה הנדרשת בין מיקום הרחפן
                        // שזה מיקום התחנת בסיס הקרובה ביותר לשולח) לבין השולח)
                        battarySenderToTarget += Distance(item.CurrentLocation.Latitude, SenderCustomer.Latitude,
                           item.CurrentLocation.Longtitude, SenderCustomer.Longtitude) * PowerConsumptionAvailable;
                    }

                    else
                    {
                        //
                        item.CurrentLocation.Latitude = SenderCustomer.Latitude;
                        item.CurrentLocation.Longtitude = SenderCustomer.Longtitude;
                    }
                    item.BattaryStatus = (random.NextDouble() * (100 - battarySenderToTarget)) + battarySenderToTarget;// לשאול את יהודה
                }
            }
        }

        //public void GetCustomerToList()
        public IEnumerable<CustomerToList> GetCustomerToList()
        {
            List<IDAL.DO.Customer> customers = dal.GetCustomersByPredicate().ToList();
            List<CustomerToList> BLCustomer = new();

            foreach (var item in customers)
            {
                BLCustomer.Add(new CustomerToList
                {
                    CustomerId = item.CustomerId,
                    NameCustomer = item.Name,
                    Phone = item.Phone,
                    SendParcelAndSupplied = dal.GetPackagesByPredicate(x => x.SenderId == item.CustomerId && x.Delivered != DateTime.MinValue).ToList().Count,
                    SendParcelAndNotSupplied = dal.GetPackagesByPredicate(x => x.SenderId == item.CustomerId && x.Delivered == DateTime.MinValue).ToList().Count,
                    ParcelsReciever = dal.GetPackagesByPredicate(x => x.TargetId == item.CustomerId && x.Delivered == DateTime.Now).ToList().Count,
                    ParcelOweyToCustomer = dal.GetPackagesByPredicate(x => x.TargetId == item.CustomerId && x.PickedUp == DateTime.MinValue).ToList().Count
                });
            }
            return BLCustomer;
        }

        public IEnumerable<ParcelToList> GetParcelToLists()
        {
            List<IDAL.DO.Parcel> parcels = dal.GetPackagesByPredicate().ToList();
            List<ParcelToList> BLparcels = new();

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
            BaseStation BLbaseStation = new();
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
            Drone BLdrone = new();
            IDAL.DO.Drone drone = dal.GetDrone(id);

            if (drone.DroneID == -1) { throw new Exception("This Drone have not exist, please try again."); }

            return BLdrone;
        }

        public Customer GetCustomer(int id)
        {
            IDAL.DO.Customer DalCustomer = dal.GetCustomer(id);
            Customer BLCustomer = new();
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
            Parcel BLParcel = new();
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
                BaseStation station = new();

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
                            IndexOfMinDistancesBetweenLocations(Dalstations, droneToCharge.CurrentLocation).Item2;
            if (droneToCharge.BattaryStatus - distanceBetweenStationToDrone * PowerConsumptionAvailable < 0)
            {
                throw new Exception("WRONG!, There are not enough battery to complete this mission");
            }
            droneToCharge.BattaryStatus -=
                distanceBetweenStationToDrone * PowerConsumptionAvailable;
            droneToCharge.CurrentLocation =
                BLStations[IndexOfMinDistancesBetweenLocations(Dalstations, droneToCharge.CurrentLocation).Item1].Location;
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

        public void AssignmentOfPackageToDrone(int droneId) //שיוך חבילה לרחפן
        {
            DroneToList drone = DroneToList.Find(x => x.DroneID == droneId);
            List<IDAL.DO.Parcel> parcels = dal.GetPackagesByPredicate(x => (int)x.Parcel_weight < (int)drone.DroneWeight).OrderBy(i => (int)i.ParcelPriority).ThenBy(i => (int)i.Parcel_weight).ToList();// לבדוק עם יהודה לגבי תנאי הסינון
            if (drone.Status == DroneStatus.available)
            {
                foreach (var parcel in parcels)
                {
                    if (batteryStatusBetweenAllLocation(parcel, drone))
                    {
                        drone.Status = DroneStatus.busy;
                        drone.NumOfPackageDelivered = parcel.ParcelId;
                        dal.SetDroneForParcel(parcel.ParcelId, droneId);
                        break;
                    }
                }
                if (drone.Status != DroneStatus.busy)
                {
                    throw new Exception("There is no Parcel ");
                }
            }
            else
                throw new Exception("The Drone is not available");
        }

        private bool batteryStatusBetweenAllLocation(IDAL.DO.Parcel parcel, DroneToList drones)
        {

            double BattaryStatus = Distance(drones.CurrentLocation.Latitude,
                                   GetCustomer(parcel.SenderId).LocationCustomer.Latitude,
                                   drones.CurrentLocation.Longtitude, GetCustomer(parcel.SenderId).LocationCustomer.Longtitude)
                                   * PowerConsumptionAvailable;
            double diSenderToTarget = Distance(GetCustomer(parcel.SenderId).LocationCustomer.Latitude, GetCustomer(parcel.TargetId).LocationCustomer.Latitude,
                GetCustomer(parcel.SenderId).LocationCustomer.Longtitude, GetCustomer(parcel.TargetId).LocationCustomer.Longtitude);
            switch ((WeightCategories)parcel.Parcel_weight)
            {
                case WeightCategories.light:
                    BattaryStatus += diSenderToTarget * PowerConsumption_LightWeight;
                    break;
                case WeightCategories.medium:
                    BattaryStatus += diSenderToTarget * PowerConsumption_MediumWeight;
                    break;
                case WeightCategories.heavy:
                    BattaryStatus += diSenderToTarget * PowerConsumption_HeavyWeight;
                    break;
                default:
                    break;
            }
            List<BaseStation> baseStationsBL = new List<BaseStation>();
            List<IDAL.DO.BaseStation> baseStationsDAL = dal.GetBaseStationByPredicate().ToList();

            BattaryStatus += IndexOfMinDistancesBetweenLocations(baseStationsDAL, GetCustomer(parcel.TargetId).LocationCustomer).Item2;
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
            IDAL.DO.Parcel parcels = dal.GetParcel(drones.NumOfPackageDelivered);
            if (parcels.PickedUp == DateTime.MinValue)
            {
                Location location = GetCustomer(parcels.SenderId).LocationCustomer;
                drones.BattaryStatus -= Distance(drones.CurrentLocation.Latitude, location.Latitude, drones.CurrentLocation.Longtitude, location.Longtitude) * PowerConsumptionAvailable;
                drones.CurrentLocation = location;
                dal.PackageCollectionByDrone(parcels.ParcelId, droneID);
            }
            else
            {
                throw new Exception("This Parcel have been collected");
            }
        }
    }
}