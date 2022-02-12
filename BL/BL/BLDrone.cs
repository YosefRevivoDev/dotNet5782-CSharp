﻿using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BL : IBL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdDrone"></param>
        /// <param name="updateDrone"></param>
        /// <param name="cancellationThreading"></param>
        public void newSimulator(int IdDrone, Action updateDrone, Func<bool> cancellationThreading)
        {
            // new DroneSimulator(this, IdDrone, updateDrone, cancellationThreading);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newDrone"></param>
        /// <param name="NumberOfStation"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewDrone(Drone newDrone, int NumberOfStation)
        {
            try
            {
                DO.BaseStation baseStation = dal.GetBaseStation(NumberOfStation);

                newDrone.CurrentLocation = new Location()
                {
                    Latitude = baseStation.Latitude,
                    Longtitude = baseStation.Longtitude
                };

                DO.Drone drone = new()
                {
                    DroneID = newDrone.DroneID,
                    DroneModel = newDrone.DroneModel,
                    DroneWeight = (DO.WeightCategories)newDrone.DroneWeight,

                };

                dal.AddDrone(drone);
                DroneToList.Add(new DroneToList()
                {
                    DroneID = newDrone.DroneID,
                    DroneModel = newDrone.DroneModel,
                    DroneWeight = newDrone.DroneWeight,
                    CurrentLocation = newDrone.CurrentLocation,
                    BattaryStatus = random.NextDouble() * 20.0 + 20.0,
                    Status = DroneStatus.available,
                });          

                DO.DroneCharge droneCharge = new()
                {
                    DroneID = newDrone.DroneID,
                    StationID = baseStation.StationID
                };

                 dal.AddDroneCharge(droneCharge);
                dal.MinusDroneCharge(NumberOfStation);
            }
            catch (DO.CheckIdException ex)
            {
                throw new CheckIdException("ERORR", ex);
            }
            catch (DO.CheckIfIdNotException ex)
            {
                throw new CheckIfIdNotExceptions("ERORR", ex);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newNameModel"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(int id, string newNameModel)
        {
            try
            {
                DO.Drone drones = dal.GetDrone(id);
                drones.DroneModel = newNameModel;
                dal.UpdateDrone(drones);
                DroneToList.Find(x => x.DroneID == id).DroneModel = newNameModel;
            }
            catch (DO.DroneException ex)
            {
                throw new Exception("שגיאה");
            }

        }

        public Drone GetDrone(int id)
        {
            try
            {
                
                DroneToList droneToList = DroneToList.Find(x => x.DroneID == id);

                if (droneToList == default)
                {
                    throw new CheckIfIdNotExceptions("This Drone have not exist, please try again.");
                }
                
                Drone BLdrone = new()
                {
                    DroneID = droneToList.DroneID,
                    DroneModel = droneToList.DroneModel,
                    DroneWeight = droneToList.DroneWeight,
                    BattaryStatus = droneToList.BattaryStatus,
                    CurrentLocation = droneToList.CurrentLocation,
                    Status = droneToList.Status,
                };

                if (droneToList.NumOfPackageDelivered != 0)
                {
                    BLdrone.ParcelInDeliverd = GetParcelInDeliverd(droneToList.CurrentLocation, droneToList.NumOfPackageDelivered);
                }
                return BLdrone;
            }
            catch (DO.CheckIdException ex)
            {
                throw new CheckIdException("ERORR" ,ex);
            }
            catch (DO.CheckIfIdNotException ex)
            {
                throw new CheckIfIdNotExceptions("ERORR", ex);
            }

        }

        public void InitDroneToLists()// initialize drone
        {
            double[] tempPower = dal.RequetPowerConsumption();
            PowerConsumptionAvailable = tempPower[0];
            PowerConsumptionLightWeight = tempPower[1];
            PowerConsumptionMediumWeight = tempPower[2];
            PowerConsumptionHeavyWeight = tempPower[3];
            LoadingDrone = tempPower[4];

            List<DO.Parcel> parcels = dal.GetPackagesByPredicate(x => x.DroneId > 0).ToList();// Sorts packages that belong to the drone but are not provided

            //רשימת תעודות זהות של לקוחות שיש חבילות שסופקו להם
            List<int> parcelThatDeliveredId = parcels.FindAll(x => x.Delivered != null).
                Select(x => x.TargetId).ToList();

            foreach (DroneToList drone in DroneToList) //For all DroneToList do 
            {
                double battarySenderToTarget;

                int findIndex = parcels.FindIndex(x => x.DroneId == drone.DroneID && x.Delivered == DateTime.MinValue);//Index of parcel that assigned but not sent1
                drone.CurrentLocation = new();
                if (findIndex != -1)//if have a parcel that assigned but not sent, do 
                {

                    drone.Status = DroneStatus.busy;//get the parcel ID that assigned to the drone & set to DroneToList parcel ID that not delivered and change status to busy
                    drone.NumOfPackageDelivered = parcels[findIndex].ParcelId;

                    
                    DO.Customer SenderCustomer = customers.Find(x => x.CustomerId == parcels[findIndex].SenderId); //Set parmaters to SenderCustomer & TargetCustomer
                    DO.Customer TargetCustomer = customers.Find(x => x.CustomerId == parcels[findIndex].TargetId);

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

                    if (parcels[findIndex].PickedUp == DateTime.MinValue)
                    {
                        drone.CurrentLocation = LocationOfTheNearestStation(Senderlocation, baseStations);// Battery status is required between the sender and the destination depending on the size of the package
                        battarySenderToTarget = (helpFunction.DistanceBetweenLocations(drone.CurrentLocation, Senderlocation) * PowerConsumptionAvailable)
                        + (helpFunction.DistanceBetweenLocations(Senderlocation, Targetlocation) * tempPower[(int)ParcelOfDelivery[findIndex].ParcelWeight])
                        + (helpFunction.DistanceBetweenLocations(Targetlocation, LocationOfTheNearestStation(Targetlocation, baseStations)) * PowerConsumptionAvailable);
                        drone.BattaryStatus = (random.NextDouble() * (100.0 - battarySenderToTarget)) + battarySenderToTarget;
                    }
                    else
                    {
                        drone.CurrentLocation = Senderlocation;
                        battarySenderToTarget = (helpFunction.DistanceBetweenLocations(Senderlocation, Targetlocation) *
                            tempPower[(int)ParcelOfDelivery[findIndex].ParcelWeight])
                        + (helpFunction.DistanceBetweenLocations(Targetlocation, LocationOfTheNearestStation(Targetlocation, baseStations)) * PowerConsumptionAvailable);
                        drone.BattaryStatus = (random.NextDouble() * (100.0 - battarySenderToTarget)) + battarySenderToTarget;
                        
                    }
                }
                else
                {
                    int index;

                    DO.DroneCharge droneCarge = new();
                    try
                    {
                        droneCarge = dal.GetDroneChargeByDrone(drone.DroneID);
                    }
                    catch (DO.CheckIfIdNotException)
                    {
                        droneCarge.DroneID = 0;
                    }
                    if (droneCarge.DroneID == 0)
                    {
                        drone.Status = DroneStatus.available;
                    }

                    else
                    {
                        drone.Status = DroneStatus.maintenance;
                        DO.BaseStation station = dal.GetBaseStation(droneCarge.StationID);
                        drone.CurrentLocation = new() { Longtitude = station.Longtitude, Latitude = station.Latitude };
                        drone.BattaryStatus = random.NextDouble() * 20.0;
                    }
                    if (drone.Status == DroneStatus.available)
                    {
                        List<DO.Parcel> droneParcel = ParcelOfDelivery.FindAll(i => i.Delivered != DateTime.MinValue);

                        index = random.Next(0, droneParcel.Count);
                        if (droneParcel.Count > 0)
                        {
                            DO.Customer target = customers.Find(c => c.CustomerId == droneParcel[index].TargetId);
                            Location location = new() { Latitude = target.Latitude, Longtitude = target.Longtitude };
                            drone.CurrentLocation = location;
                        }

                        battarySenderToTarget = helpFunction.DistanceBetweenLocations(drone.CurrentLocation, LocationOfTheNearestStation(drone.CurrentLocation, baseStations)) * PowerConsumptionAvailable;
                        drone.BattaryStatus = (random.NextDouble() * (100.0 - battarySenderToTarget)) + 30.0;
                        if (drone.BattaryStatus > 100.0)
                            drone.BattaryStatus = 100.0;
                    }
                }
            }
        }

        public IEnumerable<DroneToList> GetDroneToListsBLByPredicate(Predicate<DroneToList> predicate = null)
        {
            return DroneToList.Where(i => predicate == null ? true : predicate(i)).ToList();
        }

        public bool SendDroneToCharge(int droneId)
        {
            try
            {
                DroneToList droneFromListToCharge = DroneToList.Find(x => x.DroneID == droneId);
                if (droneFromListToCharge.Status != DroneStatus.available || droneFromListToCharge.DroneID == 0)
                {
                    throw new ChargExeptions("Only available Drone can be sending to charge");
                }
                else
                {
                    List<DO.BaseStation> freeChargeSlots = dal.GetBaseStationByPredicate(i => i.AvailableChargeSlots > 0).ToList();
                    DO.BaseStation nearBaseStation = TheNearestOfStation(droneFromListToCharge.CurrentLocation, freeChargeSlots);
                    Location locationOfStation = new()
                    {
                        Latitude = nearBaseStation.Latitude,
                        Longtitude = nearBaseStation.Longtitude
                    };

                    double searchNearStation = helpFunction.DistanceBetweenLocations(droneFromListToCharge.CurrentLocation, locationOfStation);
                    if (droneFromListToCharge.BattaryStatus < (searchNearStation * PowerConsumptionAvailable))
                    {
                        throw new ChargExeptions("אין מספיק בטרייה");
                    }
                    else
                    {
                        int droneIndex = DroneToList.FindIndex(i => i.DroneID == droneId);
                        droneFromListToCharge.BattaryStatus -= searchNearStation * PowerConsumptionAvailable;
                        droneFromListToCharge.CurrentLocation = locationOfStation;
                        droneFromListToCharge.Status = DroneStatus.maintenance;
                        DroneToList[droneIndex] = droneFromListToCharge;

                        dal.MinusDroneCharge(nearBaseStation.StationID);
                        dal.AddDroneCharge(new()
                        {
                            DroneID = droneFromListToCharge.DroneID,
                            StationID = nearBaseStation.StationID,
                            CurrentTime = DateTime.Now
                        });
                        return true;
                    }
                }
            }

            catch (DO.CheckIdException Ex)
            {
                throw new CheckIdException("ERORR", Ex);
            }
            catch (DO.CheckIfIdNotException Ex)
            {
                throw new CheckIfIdNotExceptions("ERORR", Ex);
            }

        }

        public bool ReleaseDroneFromCharge(int droneId)
        {
            try
            {
                DroneToList drone = DroneToList.Find(x => x.DroneID == droneId);
                if (drone.Status != DroneStatus.maintenance || drone.DroneID == 0)
                {
                    throw new Exception("That Drone has not found, Please try again");
                }
                else
                {
                    int droneIndex = DroneToList.FindIndex(i => i.DroneID == droneId);
                   
                    DO.DroneCharge droneCarge = dal.GetDroneChargeByDrone(droneId);
                    TimeSpan chargingTime = DateTime.Now - droneCarge.CurrentTime;

                    drone.BattaryStatus = Math.Min(100, (double)drone.BattaryStatus + chargingTime.TotalSeconds * LoadingDrone);
                    drone.Status = DroneStatus.available;
                    DroneToList[droneIndex] = drone;

                    dal.PlusDroneCharge(droneCarge.StationID);
                    dal.ReleasingChargeDrone(droneId, droneCarge.StationID);
                   // dal.MinusDroneCharge(droneId);
                    return true;
                }
            }
            catch (DO.CheckIdException Ex)
            {
                throw new CheckIdException("ERORR", Ex);
            }
            catch (DO.CheckIfIdNotException Ex)
            {
                throw new CheckIfIdNotExceptions("ERORR", Ex);
            }

        }

        public bool AssignmentOfPackageToDrone(int droneId)//שיוך חבילה לרחפן
        {
            try
            {
                DO.Parcel parcel = new();
                DO.Drone drone1 = dal.GetDrone(droneId);

                DroneToList droneToList = DroneToList.Find(x => x.DroneID == droneId);
                int FindDrone = DroneToList.FindIndex(i => i.DroneID == droneId);

                if (droneToList.Status != DroneStatus.available) // Checks if drone is available.
                    throw new ParcelAssociationExeptions("רחפן לא פנוי למשלוח");
                
                parcel = (from Parcel in dal.GetPackagesByPredicate().ToList()
                          where Parcel.Assignment == DateTime.MinValue
                          orderby Parcel.ParcelPriority descending
                          orderby Parcel.ParcelWeight descending
                          where BatteryStatusBetweenAllLocation(droneToList, Parcel)
                          select Parcel).FirstOrDefault();

                
                if (parcel.ParcelId != 0)
                {
                    DroneToList[FindDrone].Status = DroneStatus.busy;
                    DroneToList[FindDrone].NumOfPackageDelivered = parcel.ParcelId;
                    dal.SetDroneForParcel(parcel.ParcelId, droneId);
                    return true;
                }
                else
                {
                    throw new ParcelAssociationExeptions("לא נמצאה חבילה מתאימה עבור הרחפן המבוקש");
                }
            }
            catch (DO.CheckIdException ex)
            {
                throw new CheckIdException("ERORR", ex);
            }
            catch (DO.CheckIfIdNotException ex)
            {
                throw new CheckIfIdNotExceptions("ERORR", ex);
            }
        }

        private bool BatteryStatusBetweenAllLocation(DroneToList drones, DO.Parcel parcel)
        {
            DO.Customer senderCustomer = dal.GetCustomer(parcel.SenderId);
            Location senderCustLocation = new()
            {
                Latitude = senderCustomer.Latitude,
                Longtitude = senderCustomer.Longtitude
            };
            DO.Customer targetCustomer = dal.GetCustomer(parcel.TargetId);
            Location targetCustLocation = new()
            {
                Longtitude = targetCustomer.Longtitude,
                Latitude = targetCustomer.Latitude
            };

            double weight = PowerConsumptionAvailable;
            switch ((WeightCategories)parcel.ParcelWeight)
            {
                case WeightCategories.light:
                    weight = PowerConsumptionLightWeight;
                    break;
                case WeightCategories.medium:
                    weight = PowerConsumptionMediumWeight;
                    break;
                case WeightCategories.heavy:
                    weight = PowerConsumptionHeavyWeight;
                    break;
                default:
                    break;
            }

            if (helpFunction.DistanceBetweenLocations(drones.CurrentLocation,
                senderCustLocation) * PowerConsumptionAvailable + (helpFunction.DistanceBetweenLocations
                (senderCustLocation, targetCustLocation) * weight +
                helpFunction.DistanceBetweenLocations(targetCustLocation,
                helpFunction.IndexOfMinDistancesBetweenLocations(targetCustLocation, baseStations))
                * PowerConsumptionAvailable) <= drones.BattaryStatus)
                return true;
            else

                return false;
        }

        public bool CollectParcelByDrone(int droneID)
        {
            try
            {
                DO.Parcel parcelsInDrone = dal.GetPackagesByPredicate(i => i.DroneId == droneID && i.PickedUp == DateTime.MinValue).First();

                DO.Customer sander = dal.GetCustomer(parcelsInDrone.SenderId);
                Location sanderLocation = new() { Latitude = sander.Latitude, Longtitude = sander.Longtitude };

                int index = DroneToList.FindIndex(i => i.DroneID == droneID);

                DroneToList[index].BattaryStatus -= helpFunction.DistanceBetweenLocations(DroneToList[index].CurrentLocation, sanderLocation) * PowerConsumptionAvailable;
                DroneToList[index].CurrentLocation = sanderLocation;
                dal.PackageCollectionByDrone(parcelsInDrone.DroneId);
                return true;
            }
            catch (DO.CheckIdException Ex)
            {
                throw new CheckIdException("ERORR", Ex);
            }
            catch (DO.CheckIfIdNotException Ex)
            {
                throw new CheckIfIdNotExceptions("ERORR", Ex);
            }
        }

        public bool DeliveryParcelToCustomer(int droneID)
        {
            try
            {
                DO.Parcel parcelInDrone = dal.GetPackagesByPredicate(i => i.DroneId == droneID && i.PickedUp != DateTime.MinValue && i.Delivered == DateTime.MinValue).First();

                DO.Customer sander = dal.GetCustomer(parcelInDrone.SenderId);
                Location sanderLocation = new() { Latitude = sander.Latitude, Longtitude = sander.Longtitude };

                DO.Customer target = dal.GetCustomer(parcelInDrone.TargetId);
                Location targetLocation = new() { Latitude = target.Latitude, Longtitude = target.Longtitude };

                int index = DroneToList.FindIndex(i => i.DroneID == droneID);

                double weight = PowerConsumptionLightWeight;
                switch (parcelInDrone.ParcelWeight)
                {
                    case DO.WeightCategories.light:
                        weight = PowerConsumptionLightWeight;
                        break;
                    case DO.WeightCategories.medium:
                        weight = PowerConsumptionMediumWeight;
                        break;
                    case DO.WeightCategories.heavy:
                        weight = PowerConsumptionHeavyWeight;
                        break;
                    default:
                        break;
                }

                DroneToList[index].BattaryStatus -= helpFunction.DistanceBetweenLocations(sanderLocation, targetLocation) * weight;
                DroneToList[index].CurrentLocation = targetLocation;
                DroneToList[index].Status = DroneStatus.available;
                DroneToList[index].NumOfPackageDelivered = 0;
                dal.DeliveredPackageToCustumer(parcelInDrone.ParcelId);

                return true;
            }
            catch (DO.CheckIdException ex)
            {

                throw new CheckIdException("ERORR", ex);
            }
            catch (DO.CheckIfIdNotException ex)
            {

                throw new CheckIfIdNotExceptions("ERORR", ex);
            }
        }

        public void RemoveDroneBL(int id)
        {
            //IDAL.DO.BaseStation baseStationRemove = dal.GetBaseStation(id);
            dal.RemoveDrone(id);
        }
        public DroneInParcel GetDroneAtParcel(int droneId)
        {
            DroneToList droneToList = DroneToList.Find(i => i.DroneID == droneId);
            if (droneToList.DroneID == 0)
            {
                throw new CheckIfIdNotExceptions("לא נמצא רחפן תואם");

            }

            DroneInParcel droneInParcel = new()
            {
                DroneID = droneToList.DroneID,
                BattaryStatus = droneToList.BattaryStatus,
                CorrentLocation = droneToList.CurrentLocation
            };
            return droneInParcel;
        }
    }
}

