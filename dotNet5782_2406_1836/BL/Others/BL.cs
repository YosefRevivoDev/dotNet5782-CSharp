using System;
using System.Collections.Generic;
using System.Linq;

namespace BO
{
    public class BL : IBL.IBL
    {
       static DalObject.DalObject dalobj = new();
        readonly IDAL.IDal dal;
        public List<DroneToList> DroneToLists { get; }
        static Random random = new(DateTime.Now.Millisecond);
        static double PowerConsumption_Available;
        static double PowerConsumption_LightWeight;
        static double PowerConsumption_MediumWeight;
        static double PowerConsumption_HeavyWeight;
        static double LoadingDrone;

        public BL()
        {

            dal = new DalObject.DalObject(); // Access to summon methods from Datasource
            DroneToLists = new List<DroneToList>();
            double[] temp = dal.RequetPowerConsumption();//לשים את שאר המשתנים
            PowerConsumption_Available = temp[0];


            List<IDAL.DO.Drone> DroneList = dal.GetDronesByPredicate().ToList();
            List<IDAL.DO.BaseStation> baseStations = dal.GetBaseStationByPredicate().ToList();

            List<IDAL.DO.Parcel> parcels =
                dal.GetPackagesByPredicate(x => x.DroneId != 0).ToList();// Sorts packages that belong to the drone but are not provided
            foreach (var item in DroneToLists)
            {
                int index = parcels.FindIndex(x => x.DroneId == item.DroneID && x.Delivered == DateTime.MinValue);
                if (index != -1)
                {
                    item.Status = DroneStatus.busy;

                    IDAL.DO.Customer SenderCustomer = dal.GetCustomer(parcels[index].SenderId);
                    IDAL.DO.Customer TargetCustomer = dal.GetCustomer(parcels[index].TargetId);

                    Location Senderlocation = new Location { Latitude = SenderCustomer.Latitude, Longtitude =
                        SenderCustomer.Longtitude };
                    Location Targetlocation = new Location { Latitude = TargetCustomer.Latitude, Longtitude =
                        TargetCustomer.Longtitude };

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
                    item.BattaryStatus =  (random.NextDouble() * (100 - battarySenderToTarget)) + battarySenderToTarget;

                    // מתחילים מצב סוללה
                    

                    List<IDAL.DO.Parcel> parcel = dal.GetPackagesByPredicate(x => x.Delivered == DateTime.Now).ToList();
                    int i = parcel.FindIndex(x => x.DroneId == item.DroneID);
                  

                    if (true)
                    {

                    }


                }

            }    
        }
        //-------------------------------------bl functions------------------------//
        static public void AddBaseStation(BaseStation newBaseStation)
        {
            IDAL.DO.BaseStation baseStation = new()
            {
                StationID = newBaseStation.ID,
                Name = newBaseStation.Name,
                Longtitude = newBaseStation.Location.Longtitude,
                Latitude = newBaseStation.Location.Latitude,
                ChargeSlots = newBaseStation.AvailableChargingStations,
            };
            try
            {
                dalobj.Add_BaseStation(baseStation);
            }
            catch { }
        }
        public static void AddNewDrone(Drone newDrone, int chargingStationFree)// 
        {
            IDAL.DO.Drone drone = new()
            {
                DroneID = newDrone.DroneID,
                Drone_Model = newDrone.Drone_Model,
                Drone_weight = (IDAL.DO.WeightCategories)newDrone.Drone_weight,
            };
            try
            {
                dalobj.Add_Drone(drone);
            }
            catch { }
            try
            {
                newDrone.BattaryStatus = random.Next(20, 41);
                newDrone.Status = DroneStatus.maintenance;
                newDrone.CurrentLocation.Latitude = dalobj.GetBaseStation(chargingStationFree).Latitude;
                newDrone.CurrentLocation.Longtitude = dalobj.GetBaseStation(chargingStationFree).Longtitude;


            }
            catch { }
                   
        }
        
        public static void AddNewCustomer (Customer newCustomer)
        {
            IDAL.DO.Customer customer = new()
            {
                CustomerId = newCustomer.CustomerId,
                Name = newCustomer.Name,
                Phone = newCustomer.Phone,
                Latitude = newCustomer.LocationCustomer.Latitude,
                Longtitude = newCustomer.LocationCustomer.Longtitude
            };
            try
            {
                dalobj.Add_Customer(customer);
            }
            catch { }
            
        }
        public static void AddNewParcel(Parcel newParcel, DateTime requested, DateTime CreateTime, DateTime pickedUp, DateTime delivered )
        {
            IDAL.DO.Parcel parcel = new()
            {
                SenderId = newParcel.SenderId,
                TargetId = newParcel.TargetId,
                Parcel_weight = (IDAL.DO.WeightCategories)newParcel.weight,
                Parcel_priority = (IDAL.DO.Priorities)newParcel.Priority
            };
            try
            {
                dalobj.Add_Parcel(parcel);
            }
            catch { }
            try
            {
                newParcel.CreateTime = DateTime.Now;
                newParcel.Requested = DateTime.MinValue;
                newParcel.PickedUp = DateTime.MinValue;
                newParcel.Delivered = DateTime.MinValue;

                IDAL.DO.Drone? dronetest=null;
                newParcel.Drone = (IDAL.DO.Drone)dronetest;
                
            }
            catch { }
            
        }
    }

}
