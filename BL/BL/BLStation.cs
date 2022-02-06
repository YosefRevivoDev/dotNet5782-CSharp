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
        /// <summary>
        /// AddBaseStation by StationID, Name, AvailableChargeSlots, location
        /// </summary>
        /// <param name="newBaseStation"></param>
        public void AddBaseStation(BaseStation newBaseStation)
        {
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


        /// <summary>
        /// GetBaseStation by ID
        /// </summary>
        /// <param name="id"></param>
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

        /// <summary>
        /// RemoveBaseStation from BL & Dal layer
        /// </summary>
        /// <param name="id"></param>
        public void RemoveBaseStationBL(int id)
        {
            //IDAL.DO.BaseStation baseStationRemove = dal.GetBaseStation(id);
            dal.RemoveBaseStation(id);
        }

        /// <summary>
        /// Update BaseStation by stationId NameStation & sum of chargestation
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="newNameStation"></param>
        /// <param name="sumOfChargestation"></param>
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
            catch (DO.BaseStationException)
            {

                throw new Exception(" ");
            }
        }

        /// <summary>
        /// Get BasetationToLists By Predicate
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public IEnumerable<BaseStationToList> GetBasetationToListsByPredicate(Predicate<BaseStationToList> p = null)
        {
            try
            {
                List<BaseStationToList> stationToLists = new();

                List<DO.BaseStation> stations = dal.GetBaseStationByPredicate().ToList();
                foreach (DO.BaseStation item in stations)
                {
                    stationToLists.Add(GetBaseStationToList(item.StationID));
                }
                return stationToLists.Where(i => p == null ? true : p(i)).ToList();
            }
            catch (DO.CheckIfIdNotException Ex)
            {
                throw new CheckIfIdNotException("ERORR", Ex);
            }
        }


        /// <summary>
        /// get nearest between customer to stations
        /// </summary>
        /// <param name="customerLocation"></param>
        /// <param name="stations"></param>
        /// <returns></returns>
        public Location LocationOfTheNearestStation(Location customerLocation, List<DO.BaseStation> stations)
        {
            DO.BaseStation nearStation = stations[0];
            double distance = double.MaxValue;

            for (int i = 0; i < stations.Count; i++)
            {
                Location stationLocation = new() { Latitude = stations[i].Latitude, Longtitude = stations[i].Longtitude };
                double distance1 = helpFunction.DistanceBetweenLocations(stationLocation, customerLocation);
                if (distance1 < distance)
                {
                    distance = distance1;
                    nearStation = stations[i];
                }
            }
            Location location = new() { Longtitude = nearStation.Longtitude, Latitude = nearStation.Latitude };
            return location;
        }

        /// <summary>
        /// help func that get BaseStationToList by stationID
        /// </summary>
        /// <param name="stationID"></param>
        /// <returns></returns>
        public BaseStationToList GetBaseStationToList(int stationID)
        {
            try
            {
                BaseStation station = GetBaseStation(stationID);
                BaseStationToList baseStationToList = new()
                {
                    ID = station.ID,
                    Name = station.Name,
                    AvailableChargingStations = station.AvailableChargingStations
                };

                //inial NotAvailableChargingStations list by prdicate
                if (station.droneCharges == null)
                {
                    baseStationToList.NotAvailableChargingStations = station.droneCharges.Count;
                }
                else
                {
                    baseStationToList.NotAvailableChargingStations = station.droneCharges.Count;
                }
                return baseStationToList;
            }
            catch (DO.CheckIfIdNotException Ex)
            {
                throw new CheckIfIdNotException("ERORR", Ex);
            }

        }

        /// <summary>
        /// help func that get nearest between customer to stations
        /// </summary>
        /// <param name="customerLocation"></param>
        /// <param name="stations"></param>
        /// <returns></returns>
        private DO.BaseStation TheNearestOfStation(Location customerLocation, List<DO.BaseStation> stations)
        {
            DO.BaseStation nearStation = stations[0];
            double distance = double.MaxValue;

            for (int i = 0; i < stations.Count; i++)
            {
                Location stationLocation = new() { Latitude = stations[i].Latitude, Longtitude = stations[i].Longtitude };
                double distance1 = helpFunction.DistanceBetweenLocations(stationLocation, customerLocation);
                if (distance1 < distance)
                {
                    distance = distance1;
                    nearStation = stations[i];
                }
            }
            Location location = new() { Longtitude = nearStation.Longtitude, Latitude = nearStation.Latitude };
            return nearStation;
        }

    }
}
