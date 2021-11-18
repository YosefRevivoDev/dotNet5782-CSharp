using System;
using System.Collections.Generic;
using IDAL.DO;

//namespace DAL
//{
namespace DalObject
{
    internal class DataSource
    {
        internal static List<Drone> Drones = new List<Drone>();
        internal static List<BaseStation> Stations = new List<BaseStation>();
        internal static List<Customer> Clients = new List<Customer>();
        internal static List<Parcel> Packages = new List<Parcel>();
        internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();
        internal static Random rand = new Random();
        internal static DateTime currentTime = DateTime.Now;


        // Empty Constractor for Default
        static DataSource()
        {

        }
        internal class Config
        {
            // ------------- PowerConsumption by Drone -----------------//
            internal static double PowerConsumption_Available = 0.01;
            internal static double PowerConsumption_LightWeight = 0.04;
            internal static double PowerConsumption_MediumWeight = 0.07;
            internal static double PowerConsumption_HeavyWeight = 0.1;
            internal static double LoadingDrone = 0;

            //------------------RunID---------------------------//

            internal static int RunIdStation = 0;
            internal static int RunIdDrone = 0;
            internal static int RunCustomerId = 0;
            internal static int RunParcelId = 0;

            //---------------------static attributes--------------------



        }
        
        public static void Initialize()
        {
            Random random = new Random(DateTime.Now.Millisecond);
            string[] DronesModels = new string[5]
            {
                    "Alpha1", "Alpha2", "Alpha3", "Beta1", "Beta2"
            };
            #region initStation
            Stations.Add(new BaseStation { Name = "Base_A", StationID = config.RunIdStation++ });
            Stations.Add(new BaseStation { Name = "Base_B", StationID = config.RunIdStation++ });
            #endregion
            for (int i = 0; i < 5; i++)
            {
                Drones.Add(new Drone
                {
                    DroneID = ++Config.RunIdDrone,
                    Drone_Model = DronesModels[i],
                    Drone_weight = (WeightCategories)random.Next(1, 3)
                }
                );
            }

            string[] NameCustomers = new string[10] { "Tomer", "Yosef", "Yehuda", "Avi", "David", "Adi", "Moria", "Omer", "Ravit", "Eliyahu" };
            for (int i = 0; i < 10; i++)
            {
                Clients.Add(new Customer
                {
                    CustomerId = Config.RunCustomerId++,
                    Name = NameCustomers[i],
                    Phone = "05" + rand.Next(0, 9) + '-' + rand.Next(1000000, 10000000),
                    Latitude = rand.Next(0, 180),
                    Longtitude = rand.Next(0, 180)
                }
                 );
            }
            for (int i = 0; i < 10; i++)
            {
                Packages.Add(new Parcel
                {
                    ParcelId = Config.RunParcelId++,
                    SenderId = Clients[i].CustomerId,
                    TargetId = Clients[i + 1].CustomerId,
                    Parcel_priority = (Priorities)rand.Next(1, 3),
                    Parcel_weight = (WeightCategories)rand.Next(1, 3),
                    DroneId = Drones[i].DroneID,
                    intvation_date = currentTime,
                    Delivered = currentTime.AddDays(1),
                    Requested = currentTime.AddDays(1.5),
                    PickedUp = currentTime.AddDays(2),
                    Created = currentTime.AddDays(3),
                }
                );
            }
        }
    }
}


