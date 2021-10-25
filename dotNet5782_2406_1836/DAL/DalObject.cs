using DAL;
using DAL.DalObject;
using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DAL.DalObject.DataSource;

namespace DAL
{
    namespace DalObject
    {
        public class DalObject
        {
            public DalObject() { DataSource.Initialize(); }

            /// <summary>
            /// ////////////////////////////////////////////add funcrions
            /// </summary>
            public static void Add_Station(string name, double longitude, double latitude, int numChargeSlots)
            {
                Stations[config.Index_Station++] = new Station(config.RunIdStation++, name, longitude, latitude, numChargeSlots);
            }
            public static void Add_Drone()
            {
                Drone dr = new Drone();
                int num_DroneID, num_weight, num_DroneStatus;
                double num_battary;

                Console.WriteLine("Please enter Drone ID : ");
                int.TryParse(Console.ReadLine(), out num_DroneID);
                dr.DroneID = num_DroneID;
                Console.WriteLine("Please enter drone model: ");
                dr.Drone_Model = Console.ReadLine();
                Console.WriteLine("Please enter Drone weight: ");
                int.TryParse(Console.ReadLine(), out num_weight);
                dr.Drone_weight = (WeightCategories)num_weight;
                Console.WriteLine("Please enter Drone status: ");
                int.TryParse(Console.ReadLine(), out num_DroneStatus);
                dr.status = (DroneStatus)num_DroneStatus;
                Console.WriteLine("Please enter Drone battary: ");
                double.TryParse(Console.ReadLine(), out num_battary);
                dr.Battary = num_battary;
                //   Drones[config.Index_Drone++] = new Drone { DroneID = dr.DroneID , StationID = config.RunIdStation++ };
            }


            public static void Add_Customer()
            {
                customer cs = new customer();
                int num;
                double d1, d2;
                Console.WriteLine("Please enter ID: ");
                int.TryParse(Console.ReadLine(), out num);
                cs.Id = num;
                Console.WriteLine("Please enter name: ");
                cs.Name = Console.ReadLine();
                Console.WriteLine("Please enter phone number: ");
                cs.Phone = Console.ReadLine();
                Console.WriteLine("Please enter Longtitude: ");
                double.TryParse(Console.ReadLine(), out d1);
                cs.Longtitude = d1;
                Console.WriteLine("Please enter Latitude: ");
                double.TryParse(Console.ReadLine(), out d2);
                cs.Latitude = d2;
            }

            public static Station GetStation(int stationId)
            {
                foreach (Station station in Stations)
                {
                    if(station.StationID == stationId)
                    {
                        return station;
                    }
                }

                return null;
            }

            public static Station[] GetStations()
            {
                Station[] copyArr = new Station[Stations.Length];
                Array.Copy(Stations, copyArr, Stations.Length);
                return copyArr;
           
            }

            public static void setDroneForParcel(int parcelId, int droneId)
            {
                for(int i = 0; i < Packages.Length; i++)
                {
                    if(Packages[i].Id == parcelId)
                    {
                        Packages[i].Dronled = droneId;
                    }
                } 
            }

            
            /// <summary>
            /// ////////////////////////////////////////////////////////////objects display
            /// </summary>
            /// <param name="cs"></param>
            /// <returns></returns>
            /// 



            public static void SetWeight(WeightCategories Weight)//לבדוק איך עושים השמה למשקל - enum 
            {
                switch (Weight)
                {
                    case WeightCategories.Easy:
                        return;
                    case WeightCategories.Medium:
                        return;
                    case WeightCategories.Heavy:
                        return;
                }
            }
            ////////////////////////////////////////////////////////lists display
            ///

        }
    }
}

