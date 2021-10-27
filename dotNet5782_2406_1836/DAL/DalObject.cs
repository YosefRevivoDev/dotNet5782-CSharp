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

        /// <summary>
        /// ////////////////////////////////////////////add funcrions
        /// </summary>
        /// 

        //---------------------------------------------------ADD FUNCTIONS---------------------------------------//
        public static void Add_BaseStation(BaseStation new_baseStation)
        {
            DataSource.Stations[DataSource.config.Index_Station++] = new_baseStation;
        }

        public static void Add_Drone(Drone new_drone)
        {
            DataSource.Drones[DataSource.config.Index_Drone++] = new_drone;
        }

        public static void Add_Customer(Customer new_customer)
        {
            DataSource.Clients[DataSource.config.Index_customer++] = new_customer;
        }

        public static void Add_Parcel(Parcel new_parcel)
        {
            DataSource.Packages[DataSource.config.Index_Parcel] = new_parcel;
        }
        

        //-------------------------------------------RETURN OBJ BY ID (GET FUNCTION)----------------------------------------//

        public static Drone? GetDrone (int Id)
        {
            for (int i = 0; i < DataSource.Drones.Length ; i++)
            {
                if (Id == DataSource.Drones[i].DroneID)
                {
                    return DataSource.Drones[i];
                }
            }
            return null;
        }
        public static BaseStation? GetBaseStation (int Id)
        {
            for (int i = 0; i < DataSource.Stations.Length; i++)
            {
                if (Id == DataSource.Stations[i].StationID)
                {
                    return DataSource.Stations[i];
                }
            }
            return null;
        }

        public static Customer? GetCustomer (int Id)
        {
            for (int i = 0; i < DataSource.Clients.Length; i++)
            {
                if (Id == DataSource.Clients[i].CustomerId)
                {
                    return DataSource.Clients[i];
                }
            }
            return null;
        }

        public static Parcel? GetParcel (int Id)
        {
            for (int i = 0; i < DataSource.Packages.Length; i++)
            {
                if (Id == DataSource.Packages[i].ParcelId)
                {
                    return DataSource.Packages[i];
                }
            }
            return null;
        }

        //--------------------------------------------------UPDATE FUNCTION---------------------------------------//

        public static void SetDroneForParcel(int parcelId, int droneId)
        {
            for (int i = 0; i < Packages.Length ; i++)
            {
                if(Packages[i].ParcelId== parcelId)
                {
                    Packages[i].DroneId = droneId;
                }
            }
        }












        //--------------------------------------------------DISPLAY FUNCTION---------------------------------------//
        public static BaseStation[] copyArray()
        {
            BaseStation[] copyArr = new BaseStation[Stations.lenght];
            Array.copy(Stations, copyArr, Stations.lenght);
            return copyArr;
        }

        public static void Display(Display display)
        {
            switch (display)
            {
                case IDAL.DO.Display.BaseStation:
                    for (int i = 0; i < DataSource.Stations.Length; i++)
                    {
                        Console.WriteLine(DataSource.Stations[i]);
                    }
                    break;
                case IDAL.DO.Display.Drones:
                    for (int i = 0; i < DataSource.Drones.Length; i++)
                    {
                        Console.WriteLine(DataSource.Drones[i]);
                    }
                    break;
                case IDAL.DO.Display.Customers:
                    for (int i = 0; i < DataSource.Clients.Length; i++)
                    {
                        Console.WriteLine(DataSource.Clients[i]);
                    }
                    break;
                case IDAL.DO.Display.Parcels:
                    for (int i = 0; i <DataSource.Packages.Length; i++)
                    {
                        Console.WriteLine(DataSource.Packages[i]);
                    }
                    break;
                case IDAL.DO.Display.EXIT:
                    break;
                default:
                    break;
            }
        }
    }
}


