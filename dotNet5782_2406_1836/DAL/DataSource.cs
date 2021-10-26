using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DAL
{
    namespace DalObject
    {
        static class DataSource
        {
            internal static Drone[] Drones = new Drone[10];
            internal static Station[] Stations = new Station[5];
            internal static customer[] Clients = new customer[100];
            internal static Parcel[] Packages = new Parcel[1000];
            internal static Random rand = new Random();
            static DataSource()
            {

            }
            internal class config
            {
                internal static int Index_Drone = 0;
                internal static int Index_Station = 0;
                internal static int Index_customer = 0;
                internal static int Index_Parcel = 0;
                internal static int Parcel_RunNum = 0;

                internal static int RunIdStation = 0;
                internal static int RunIdDrone = 0;
                internal static int RunIdClient = 0;
                internal static int RunCustomerId = 0;
            }

            public static void Initialize()
            {
                Random random = new Random(DateTime.Now.Millisecond);
                string[] DronesModels = new string[5]
                {
                    "Alpha1", "Alpha2", "Alpha3", "Beta1", "Beta2"
                };
                #region initStation
                Stations[config.Index_Station++] = new Station { Name = "fff", StationID = config.RunIdStation++ };
                Stations[config.Index_Station++] = new Station { Name = "aaa", StationID = config.RunIdStation++ };
                #endregion 
                for (int i = 0; i < 5; i++)
                {
                    Drones[config.Index_Drone++] = new Drone { DroneID = ++config.RunIdDrone , Drone_Model= DronesModels[i],
                    Drone_weight = (WeightCategories)random.Next(1,3)};
                }
                string[] NameCustomers = new string[10] { "Tomer", "Yosef", "Yehuda", "Avi", "David", "Adi", "Moria", "Omer", "Ravit", "Eliyahu" };
                for (int i = 0; i < 10; i++)
                {
                    Clients[config.Index_customer++] = new IDAL.DO.Customer
                    {
                        Id = config.RunCustomerId++,
                        Name = NameCustomers[i],
                        Phone = "05" + rand.Next(0, 9) + '-' + rand.Next(1000000, 10000000),
                        Latitude = rand.Next(0, 180),
                        Longtitude = rand.Next(0, 180)
                    };
                }
                // לעשות אתחול מהיר למחלקת חבילות ולקוחות
                //(name enum) randon.next(0 , 4)
                //  DateTime dateTime = new DateTime(2021, rand.Next(1, 13), rand.Next(0, 31));
            }
        }

    }
    
}
