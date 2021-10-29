using System;
using System.Collections.Generic;
using IDAL.DO;

//namespace DAL
//{
    namespace DalObject
    {
        internal class DataSource
        {
             internal static Drone[] Drones = new Drone[10];
            internal static BaseStation[] Stations = new BaseStation[5];
            internal static Customer[] Clients = new Customer[100];
            internal static Parcel[] Packages = new Parcel[1000];
            internal static Random rand = new Random();
            internal static DateTime currentTime = DateTime.Now;
            internal static List<DroneCharge> DroneCharges
            = new List<DroneCharge>();


        // Empty Constractor for Default
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
                internal static int RunCustomerId = 0;
                internal static int RunParcelId = 0;
            }

            public static void Initialize()
            {
                Random random = new Random(DateTime.Now.Millisecond);
                string[] DronesModels = new string[5]
                {
                    "Alpha1", "Alpha2", "Alpha3", "Beta1", "Beta2"
                };
                #region initStation
                 Stations[config.Index_Station++] = new BaseStation {Name = "Base_A", StationID = config.RunIdStation++};
                 Stations[config.Index_Station++] = new BaseStation {Name = "Base_B", StationID = config.RunIdStation++};
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
                        CustomerId = config.RunCustomerId++,
                        Name = NameCustomers[i],
                        Phone = "05" + rand.Next(0, 9) + '-' + rand.Next(1000000, 10000000),
                        Latitude = rand.Next(0, 180),
                        Longtitude = rand.Next(0, 180)
                    };
                }
                for (int i = 0; i < 10; i++)
                {
                   Packages[config.Index_Parcel++] = new IDAL.DO.Parcel
                   {
                      ParcelId = config.RunParcelId++,
                      SenderId = Clients[i].CustomerId,
                      TargetId = Clients[i + 1].CustomerId,
                      Parcel_priority = (Priorities)rand.Next(1, 3),
                      Parcel_weight = (WeightCategories)rand.Next(1, 3),
                      DroneId = Drones[i].DroneID,
                      intvation_date = currentTime,
                      Delivered = currentTime.AddDays(1),
                      Requested = currentTime.AddDays(1.5),
                      PickedUp = currentTime.AddDays(2),
                      Scheduled = currentTime.AddDays(3),
                   };
                }
            }
        }
    }
    

