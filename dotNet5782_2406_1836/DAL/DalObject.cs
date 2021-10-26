//using DAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
//using static DAL.DalObject.DataSource;


namespace DalObject
{
    public class DalObject
    {
        public DalObject() { DataSource.Initialize(); }

        /// <summary>
        /// ////////////////////////////////////////////add funcrions
        /// </summary>
        public void Add_BaseStation(BaseStation new_baseStation)
        {
            DataSource.Stations[DataSource.config.Index_Station++] = new_baseStation;
        }

        public void Add_Drone(Drone new_drone)
        {
            DataSource.Drones[DataSource.config.Index_Drone++] = new_drone;
        }

        public void Add_Customer(Customer new_customer)
        {
            DataSource.Clients[DataSource.config.Index_customer++] = new_customer;
        }

        public void Add_Parcel(Parcel new_parcel)
        {
            DataSource.Packages[DataSource.config.Index_Parcel] = new_parcel;
        }
        
        public Drone? GetDrone (int Id)
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





        public void Display(Display display)
        {
            switch (display)
            {
                case IDAL.DO.Display.BaseStation:
                    for (int i = 0; i < DataSource.Stations.Length; i++)
                    {
                        Console.WriteLine(DataSource.Stations[i]);
                    }
                    break;
                case IDAL.DO.Display.Drons:
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


