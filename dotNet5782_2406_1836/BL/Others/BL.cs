using System;
using System.Collections.Generic;
using System.Linq;
using IBL;

namespace BO
{
    public class BL : IBL.IBL
    {
        readonly IDAL.IDal dal;
        public List<DroneToList> DroneToList { get; set; }
        static Random random = new(DateTime.Now.Millisecond);
        static double PowerConsumption_Available;
        static double PowerConsumption_LightWeight;
        static double PowerConsumption_MediumWeight;
        static double PowerConsumption_HeavyWeight;
        static double LoadingDrone;

        public BL()
        {
            dal = new DalObject.DalObject(); // Access to summon methods from Datasource
            DroneToList = new List<DroneToList>();
            GetDroneToLists();
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
                ChargeSlots = newBaseStation.AvailableChargingStations,// charge stations
            };
            try
            {
                dal.Add_BaseStation(baseStation);
            }
            catch { }
        }
        public void AddNewDrone(DroneToList newDrone, int NumberOfStation)// 
        {
            try
            {
                IDAL.DO.BaseStation baseStation = dal.GetBaseStation(NumberOfStation);
                if (baseStation.ChargeSlots > 0)
                {
                    IDAL.DO.DroneCharge droneCharge = new IDAL.DO.DroneCharge
                    {
                        DroneID = newDrone.DroneID,
                        StationID = baseStation.StationID
                    };
                    newDrone.Status = DroneStatus.maintenance;
                    baseStation.ChargeSlots--;
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
        public void AddNewParcel(Parcel newParcel)
        {
            try
            {
                newParcel.Drone = null;
                IDAL.DO.Parcel parcel = new()
                {
                    SenderId = newParcel.SenderId,
                    TargetId = newParcel.TargetId,
                    Parcel_weight = (IDAL.DO.WeightCategories)newParcel.weight,
                    ParcelPriority = (IDAL.DO.Priorities)newParcel.Priority,
                    Created = DateTime.Now,
                    Assignment = DateTime.MinValue,
                    PickedUp = DateTime.MinValue,
                    Delivered = DateTime.MinValue
                };
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
                    AvailableChargingStations = item.ChargeSlots,
                    NotAvailableChargingStations = dal.GetDroneChargesByPredicate(x => x.StationID == item.StationID).ToList().Count
                });
            }
            return BLStation;
        }
        public void GetDroneToLists()
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

            double[] temp = dal.RequetPowerConsumption();//לשים את שאר המשתנים
            PowerConsumption_Available = temp[0];

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

                    // מצב סוללה נדרשת בין השולח ליעד בהתאם לגודל החבילה
                    double battarySenderToTarget = BO.HelpFunction.Distance(TargetCustomer.Latitude, SenderCustomer.Latitude,
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
                    battarySenderToTarget += BO.HelpFunction.IndexOfMinDistancesBetweenLocations
                        (baseStations, Targetlocation).Item2 * PowerConsumption_Available;

                    if (parcels[index].PickedUp == DateTime.MinValue)
                    {
                        if (baseStations.Count == 0) // לבדוק אם זה false
                        {
                            throw new Exception("There are no stations in the area");
                        }
                        // קביעת מיקום הרחפן בתחנה הקרובה ביותר לשולח במצב שהחבילה לא נאספה
                        int indexMinDistance = BO.HelpFunction.IndexOfMinDistancesBetweenLocations(baseStations, Senderlocation).Item1;
                        item.CurrentLocation.Longtitude = baseStations[indexMinDistance].Longtitude;
                        item.CurrentLocation.Latitude = baseStations[indexMinDistance].Latitude;

                        // מצב סוללה הנדרשת בין מיקום הרחפן
                        // שזה מיקום התחנת בסיס הקרובה ביותר לשולח) לבין השולח)
                        battarySenderToTarget += BO.HelpFunction.Distance(item.CurrentLocation.Latitude, SenderCustomer.Latitude,
                           item.CurrentLocation.Longtitude, SenderCustomer.Longtitude) * PowerConsumption_Available;
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
            List<CustomerToList> BLCustomer = new List<CustomerToList>();

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
                    ParcelStatus = , // לבדוק עם יהודה מה צריך לעשות פה ,
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
            BLbaseStation.ID = DalBaseStation.StationID;
            BLbaseStation.Location.Latitude = DalBaseStation.Latitude;
            BLbaseStation.Location.Longtitude = DalBaseStation.Latitude;
            BLbaseStation.Name = DalBaseStation.Name;
            BLbaseStation.AvailableChargingStations = DalBaseStation.ChargeSlots;
            BLbaseStation.droneCharges = new List<DroneCharge>();

            List<IDAL.DO.DroneCharge> DroneCharges = dal.GetDroneChargesByPredicate(x => x.StationID == stationID).ToList();
            foreach (var item in DroneCharges)
            {
                BLbaseStation.droneCharges.Add(new DroneCharge 
                                            { DroneID = item.DroneID, BattaryStatus = 
                                            DroneToList.Find(x=>x.DroneID == item.DroneID).BattaryStatus });
            }
            return BLbaseStation;
        }

        public Drone GetDrone (int id)
        {
            
             
            return BLdrone;
        }

        public Customer GetCustomer (int id)
        {
            IDAL.DO.Customer DalCustomer = dal.GetCustomer(id);
            Customer BLCustomer = new Customer();

            BLCustomer.CustomerId = DalCustomer.CustomerId;
            BLCustomer.NameCustomer = DalCustomer.Name;
            BLCustomer.PhoneCustomer = DalCustomer.Phone;
            BLCustomer.LocationCustomer.Latitude = DalCustomer.Latitude;
            BLCustomer.LocationCustomer.Longtitude = DalCustomer.Longtitude;


            List<Parcel> PackagesToCustomer = new List<Parcel>();
            List<Parcel> PackagesFromCustomer = new List<Parcel>();

            return BLCustomer;
        }

        public Parcel GetParcel(int id)
        {
            IDAL.DO.Parcel DalParcel = dal.GetParcel(id);
            Parcel BLParcel = new Parcel();

            BLParcel.Id = DalParcel.ParcelId;
            BLParcel.SenderId = DalParcel.SenderId;
            BLParcel.TargetId = DalParcel.TargetId;
            BLParcel.weight = (BO.WeightCategories)DalParcel.Parcel_weight;
            BLParcel.Priority = (BO.Priorities)DalParcel.ParcelPriority;
            BLParcel.Drone.DroneID = DalParcel.DroneId;
            BLParcel.CreateTime = DalParcel.Created;
            BLParcel.Requested = DalParcel.Assignment;
            BLParcel.PickedUp = DalParcel.PickedUp;
            BLParcel.Delivered = DalParcel.Delivered;

            return BLParcel;
        }











        //public BaseStation GetBaseStation(int stationID)
        //{
        //    throw new NotImplementedException();
        //}

        //public Drone GetDrone(int droneID)
        //{
        //    throw new NotImplementedException();
        //}

        //public Customer GetCustomer(int customerID)
        //{
        //    throw new NotImplementedException();
        //}

        //public Parcel GetParcel(int parcelID)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<BasetationToList> GetBasetationToLists(Predicate<BasetationToList> predicate = null)
        //{
        //    throw new NotImplementedException();
        //}
    }

}
