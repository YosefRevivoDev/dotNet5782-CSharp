//using DAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using DalObject;
using static DalObject.DataSource;


namespace DalObject
{
    public class DalObject
    {
        public DalObject() { DataSource.Initialize(); }
        //---------------------------------------------------ADD FUNCTIONS---------------------------------------//
        public void Add_BaseStation(BaseStation new_baseStation)
        {
            DataSource.Stations[DataSource.config.Index_Station++] = new_baseStation;
        }

        public void Add_Drone(Drone new_drone)
        {
            DataSource.Drones[DataSource.config.Index_Drone++] = new_drone;
        }

        public  void Add_Customer(Customer new_customer)
        {
            DataSource.Clients[DataSource.config.Index_customer++] = new_customer;
        }

        public void Add_Parcel(Parcel new_parcel)
        {
            DataSource.Packages[DataSource.config.Index_Parcel] = new_parcel;
        }

        public void Add_DroneCharge(DroneCharge droneCharge)
        {
            DataSource.DroneCharges.Add(droneCharge);
        }

        //-------------------------------------------RETURN OBJ BY ID (GET FUNCTION)----------------------------------------//

        public Drone GetDrone (int Id)
        {
            for (int i = 0; i < DataSource.Drones.Length ; i++)
            {
                if (Id == DataSource.Drones[i].DroneID)
                {
                    return DataSource.Drones[i];
                }
            }
            return default;
        }
        public BaseStation GetBaseStation (int Id)
        {
            for (int i = 0; i < DataSource.Stations.Length; i++)
            {
                if (Id == DataSource.Stations[i].StationID)
                {
                    return DataSource.Stations[i];
                }
            }
            return default;
        }
        public Customer GetCustomer(int Id)
        {
            for (int i = 0; i < DataSource.Clients.Length; i++)
            {
                if (Id == DataSource.Clients[i].CustomerId)
                {
                    return DataSource.Clients[i];
                }
            }
            return default;
        }
        public Parcel GetParcel (int Id)
        {
            for (int i = 0; i < DataSource.Packages.Length; i++)
            {
                if (Id == DataSource.Packages[i].ParcelId)
                {
                    return DataSource.Packages[i];
                }
            }
            return default;
        }

        //--------------------------------------------------UPDATE FUNCTION---------------------------------------//

        public void SetDroneForParcel(int parcelId, int droneId)
        {
            for (int i = 0; i < Packages.Length ; i++)
            {
                if(Packages[i].ParcelId == parcelId)
                {
                    Packages[i].DroneId = droneId;
                    return;
                }
            }
        }

        public void PackageCollectionByDrone(int parcelId, int droneId)
        { 
            int index = DataSource.Packages.ToList().
                FindIndex(i => i.ParcelId == parcelId);
            DataSource.Packages[index].DroneId = droneId;
            DataSource.Packages[index].PickedUp = DateTime.Now;

            DataSource.Drones[DataSource.Drones.ToList().
             FindIndex(i => i.DroneID == droneId)].status = DroneStatus.busy;
        }

        public void DliveredPackageTOCustumer(int parcelId, int droneId)
        {
            int index = DataSource.Packages.ToList().
              FindIndex(i => i.ParcelId == parcelId);
            DataSource.Packages[index].DroneId = 0;
            DataSource.Packages[index].Delivered = DateTime.Now;

            DataSource.Drones[DataSource.Drones.ToList().
             FindIndex(i => i.DroneID == droneId)].status = DroneStatus.available;
        }

        public void ChargeDrone(int droneId, int baseStationId)
        {
            DataSource.Drones[DataSource.Drones.ToList().
              FindIndex(i => i.DroneID == droneId)].status = DroneStatus.maintenance;

            Add_DroneCharge(new DroneCharge { DroneID = droneId, StationID = baseStationId });
        }
        //--------------------------------------------------DISPLAY FUNCTION---------------------------------------//

        public BaseStation[] GetBaseStation()
        {
            return DataSource.Stations.Take(DataSource.Stations.Length).ToArray();
        }
        public Drone[] GetDrones()
        {
            return DataSource.Drones.Take(DataSource.Drones.Length).ToArray();
        }
        public Parcel[] GetPackages()
        {
            return DataSource.Packages.Take(DataSource.Packages.Length).ToArray();
        }
        public Customer[] GetCustomers()
        {
            return DataSource.Clients.Take(DataSource.Clients.Length).ToArray();
        }
    }
}


