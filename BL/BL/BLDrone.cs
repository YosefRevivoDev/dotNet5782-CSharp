using BlApi;
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
        public void newSimulator(int droneId, Action updateDrone, Func<bool> cancellationThreading)
        {
            new Simulator(this, droneId, updateDrone, cancellationThreading);
        }

        public void InitDroneToLists()// initialize drone
        {
            //Apply all drone properties by tempPower array
            double[] tempPower = dal.RequetPowerConsumption();
            PowerConsumptionAvailable = tempPower[0];
            PowerConsumptionLightWeight = tempPower[1];
            PowerConsumptionMediumWeight = tempPower[2];
            PowerConsumptionHeavyWeight = tempPower[3];
            LoadingDrone = tempPower[4];

            // Sorts packages that belong to the drone but are not provided
            List<DO.Parcel> parcels = dal.GetPackagesByPredicate(x => x.DroneId > 0).ToList();

            //List of IDs of customers who have packages provided to them
            List<int> parcelThatDeliveredId = parcels.FindAll(x => x.Delivered != null).
                Select(x => x.TargetId).ToList();

            //For all DroneToList do 
            foreach (DroneToList drone in DroneToList)
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
                    else //Parcel that picked
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
                        //Get parcel that delivered 
                        List<DO.Parcel> droneParcels = ParcelOfDelivery.FindAll(i => i.Delivered != DateTime.MinValue);

                        //Get a randomal index of droneParcels's list 
                        index = random.Next(0, droneParcels.Count);

                        if (droneParcels.Count > 0)//Not null
                        {
                            DO.Customer target = customers.Find(c => c.CustomerId == droneParcels[index].TargetId);
                            Location location = new() { Latitude = target.Latitude, Longtitude = target.Longtitude };
                            drone.CurrentLocation = location;
                        }

                        //Calculate battary status between current location & nearest station. 
                        battarySenderToTarget = helpFunction.DistanceBetweenLocations(drone.CurrentLocation, LocationOfTheNearestStation(drone.CurrentLocation, baseStations)) * PowerConsumptionAvailable;
                        drone.BattaryStatus = (random.NextDouble() * (100.0 - battarySenderToTarget)) + 30.0;
                        if (drone.BattaryStatus > 100.0)
                            drone.BattaryStatus = 100.0;
                    }
                }
            }
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
                //Get basetation that macth the same ID (NumberOfStation).
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
                    Status = DroneStatus.maintenance,
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
        /// Update drone by id & name model 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newNameModel"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(int id, string newNameModel)
        {
            try
            {
                DO.Drone drones = dal.GetDrone(id);

                if (newNameModel != drones.DroneModel)
                {
                    drones.DroneModel = newNameModel;
                }
                else
                {
                    throw new DroneNotUpdate("Error");
                }
                dal.UpdateDrone(drones);
                DroneToList.Find(x => x.DroneID == id).DroneModel = newNameModel;
            }
            catch (DO.CheckIdException Ex)
            {
                throw new CheckIdException("ERORR" + Ex);
            }
            catch (DO.CheckIfIdNotException Ex)
            {
                throw new CheckIfIdNotExceptions("ERORR", Ex);
            }
        }

        /// <summary>
        /// GetDrone by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Drone GetDrone(int id)
        {
            try
            {

                DroneToList droneToList = DroneToList.Find(x => x.DroneID == id);

                if (droneToList == default)
                {
                    throw new CheckIfIdNotExceptions("לא נמצא רחפן תואם, אנא נסה שוב");
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
                throw new CheckIdException("ERROR", ex);
            }
            catch (DO.CheckIfIdNotException ex)
            {
                throw new CheckIfIdNotExceptions("רחפן לא קיים, נסה שוב", ex);
            }

        }

        /// <summary>
        /// Get droneToListsBL By Predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<DroneToList> GetDroneToListsBLByPredicate(Predicate<DroneToList> predicate = null)
        {
            return DroneToList.Where(i => predicate == null ? true : predicate(i)).ToList();
        }

        /// <summary>
        /// Send drone to charge by ID.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public bool SendDroneToCharge(int droneId)
        {
            try
            {
                DroneToList droneFromListToCharge = DroneToList.Find(x => x.DroneID == droneId);

                if (droneFromListToCharge.BattaryStatus >= 100)
                {
                    throw new ChargExeptions("הבטרייה כבר מלאה");

                }
                if (droneFromListToCharge.Status != DroneStatus.available || droneFromListToCharge.DroneID == 0)
                {
                    throw new ChargExeptions("רק רחפן פנוי יכול לגשת לטעינה");
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

                    //If drone's battary is not enought to pass the deliver
                    if (droneFromListToCharge.BattaryStatus < (searchNearStation * PowerConsumptionAvailable))
                    {
                        throw new ChargExeptions("אין מספיק בטרייה");
                    }
                    else
                    {
                        //Initalize all reflactions for pass the dliver
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
                throw new CheckIdException("לא נמצא רחפן תואם", Ex);
            }
            catch (DO.CheckIfIdNotException Ex)
            {
                throw new CheckIfIdNotExceptions("ERORR", Ex);
            }

        }

        /// <summary>
        /// Release drone from charge by id 
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public bool ReleaseDroneFromCharge(int droneId)
        {
            try
            {
                DroneToList drone = DroneToList.Find(x => x.DroneID == droneId);
                if (drone.Status != DroneStatus.maintenance || drone.DroneID == 0)
                {
                    throw new Exception("לא נמצא רחפן מתאים, אנא נסה שוב");
                }
                else
                {

                    int droneIndex = DroneToList.FindIndex(i => i.DroneID == droneId);

                    DO.DroneCharge droneCarge = dal.GetDroneChargeByDrone(droneId);
                    TimeSpan chargingTime = DateTime.Now - droneCarge.CurrentTime;

                    //Calculate battary status after charge/
                    drone.BattaryStatus = Math.Min(100, (double)drone.BattaryStatus + chargingTime.TotalSeconds * LoadingDrone);
                    drone.Status = DroneStatus.available;
                    DroneToList[droneIndex] = drone;

                    dal.PlusDroneCharge(droneCarge.StationID);
                    dal.ReleasingChargeDrone(droneId);
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

        /// <summary>
        /// Assignment of package to drone
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public void AssignmentOfPackageToDrone(int droneId)
        {
            try
            {

                DO.Drone drone1 = dal.GetDrone(droneId);

                DroneToList droneToList = DroneToList.Find(x => x.DroneID == droneId);
                int FindDrone = DroneToList.FindIndex(i => i.DroneID == droneId);

                if (droneToList.Status != DroneStatus.available) // Checks if drone is available.
                    throw new ParcelAssociationExeptions("רחפן לא פנוי למשלוח");

                List<DO.Parcel> parcels = dal.GetPackagesByPredicate(Parcel => Parcel.Assignment == DateTime.MinValue).ToList();

                DO.Parcel parcel = (from Parcel in parcels
                                    orderby Parcel.ParcelPriority descending
                                    orderby Parcel.ParcelWeight descending
                                    where BatteryStatusBetweenAllLocation(droneToList, Parcel)
                                    select Parcel).FirstOrDefault();

                if (parcel.ParcelId != 0)
                {
                    DroneToList[FindDrone].Status = DroneStatus.busy;
                    DroneToList[FindDrone].NumOfPackageDelivered = parcel.ParcelId;
                    dal.SetDroneForParcel(parcel.ParcelId, droneId);
                    return;
                }

                throw new ParcelAssociationExeptions("לא נמצאה חבילה מתאימה");
            }
            catch (DO.CheckIdException ex)
            {
                throw new CheckIdException("ERORR", ex);
            }
            catch (ParcelAssociationExeptions ex)
            {
                throw new ParcelAssociationExeptions(ex.Message);
            }
            catch (DO.CheckIfIdNotException ex)
            {
                throw new CheckIfIdNotExceptions("ERORR", ex);
            }
        }

        /// <summary>
        /// Calculate batterys status between all Location
        /// </summary>
        /// <param name="drones"></param>
        /// <param name="parcel"></param>
        /// <returns></returns>
        private bool BatteryStatusBetweenAllLocation(DroneToList drones, DO.Parcel parcel)
        {
            //Set location - sender & terget customers
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

            //Choose weight according to parcel weight
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

            //Calculate if battary enought to pass the mission 
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

        /// <summary>
        /// Collect parcel by drone by drone ID
        /// </summary>
        /// <param name="droneID"></param>
        /// <returns></returns>
        public bool CollectParcelByDrone(int droneID)
        {
            try
            {
                DO.Parcel parcelsInDrone = dal.GetPackagesByPredicate(i => i.DroneId == droneID && i.PickedUp == DateTime.MinValue).FirstOrDefault();


                //Find parcel SenderId of parcelsInDrone that match the same customer 
                DO.Customer sender = dal.GetCustomer(parcelsInDrone.SenderId);

                Location sanderLocation = new() { Latitude = sender.Latitude, Longtitude = sender.Longtitude };

                int index = DroneToList.FindIndex(i => i.DroneID == droneID);

                //Calculate battaryStatus for go to the sender and reduce from battaru overall.
                DroneToList[index].BattaryStatus -= helpFunction.DistanceBetweenLocations(DroneToList[index].CurrentLocation, sanderLocation) * PowerConsumptionAvailable;

                //Equal the location to destination.
                DroneToList[index].CurrentLocation = sanderLocation;
                dal.PackageCollectionByDrone(parcelsInDrone.ParcelId);

                parcelsInDrone.PickedUp = DateTime.Now;
                dal.UpdateParcel(parcelsInDrone);
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

        /// <summary>
        /// Delivery parcel to customer by drone ID.
        /// </summary>
        /// <param name="droneID"></param>
        /// <returns></returns>
        public bool DeliveryParcelToCustomer(int droneID)
        {
            try
            {
                DO.Parcel parcelInDrone = dal.GetPackagesByPredicate(i => i.DroneId == droneID && i.PickedUp != DateTime.MinValue && i.Delivered == DateTime.MinValue).FirstOrDefault();
                DO.Customer sander = dal.GetCustomer(parcelInDrone.SenderId);
                Location sanderLocation = new() { Latitude = sander.Latitude, Longtitude = sander.Longtitude };

                DO.Customer target = dal.GetCustomer(parcelInDrone.TargetId);
                Location targetLocation = new() { Latitude = target.Latitude, Longtitude = target.Longtitude };

                int index = DroneToList.FindIndex(i => i.DroneID == droneID);

                double weight = PowerConsumptionLightWeight;

                //Match the current wegiht 
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

                //Initalize all reflactions for drone that delivered parcel to customer 
                DroneToList[index].BattaryStatus -= helpFunction.DistanceBetweenLocations(sanderLocation, targetLocation) * weight;
                DroneToList[index].CurrentLocation = targetLocation;
                DroneToList[index].Status = DroneStatus.available;
                DroneToList[index].NumOfPackageDelivered = 0;
                dal.DeliveredPackageToCustumer(parcelInDrone.ParcelId);

                parcelInDrone.Delivered = DateTime.Now;
                dal.UpdateParcel(parcelInDrone);

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

        /// <summary>
        /// Remove droneBL by ID.
        /// </summary>
        /// <param name="id"></param>
        public void RemoveDroneBL(int id)
        {
            dal.RemoveDrone(id);
        }

        /// <summary>
        /// Get drone at parcel by find droneId
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
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

        public int GetTheIdOfCloseStation(int idDrone)
        {
            Drone drone = GetDrone(idDrone);
            List<DO.BaseStation> stationWithFreeSlots = dal.GetBaseStationByPredicate().ToList();
            DO.BaseStation closeStation = TheNearestOfStation(drone.CurrentLocation, stationWithFreeSlots);
            return closeStation.StationID;
        }
    }
}

