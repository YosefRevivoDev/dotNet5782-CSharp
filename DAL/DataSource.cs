using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DS
{
    static class DataSource
    {
        internal static List<Drone> Drones = new List<Drone>();
        internal static List<BaseStation> Stations = new List<BaseStation>();
        internal static List<Customer> Clients = new List<Customer>();
        internal static List<Parcel> Packages = new List<Parcel>();
        internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();
        internal static List<User> users = new List<User>();
        internal static Random rand = new Random();
        internal static DateTime currentTime = DateTime.Now;


        // Empty Constractor for Default
        static DataSource()
        {
            Initialize();
        }
        internal class Config
        {
            // ------------- PowerConsumption by Drone -----------------//
            internal static double PowerConsumptionAvailable = 0.01;
            internal static double PowerConsumptionLightWeight = 0.04;
            internal static double PowerConsumptionMediumWeight = 0.07;
            internal static double PowerConsumptionHeavyWeight = 0.1;
            internal static double LoadingDrone = 2;

            //------------------RunID---------------------------//

            internal static int RunIdStation = 1;
            internal static int RunIdDrone = 1;
            internal static int RunCustomerId = 1;
            internal static int RunParcelId = 1;
            internal static int RunUserId = 1;

            //---------------------static attributes--------------------
        }
        
        public static void Initialize()
        {

            users.Add(new User { UserName = "ButterFly", FirstName = "David", LastName = "Revivo", Password = "1", UserId = Config.RunUserId++ });
            users.Add(new User { UserName = "1", FirstName = "Manager", LastName = "app", Password = "1", UserId = Config.RunUserId++ });
            Random random = new Random(DateTime.Now.Millisecond);
            string[] DronesModels = new string[]
            {
                    "Alpha1", "Alpha2", "Alpha3", "Beta1", "Beta2", "Beta3", "Beta4", "Beta5", "Beta6", "Beta7"
            };
            #region initStation
            Stations.Add(new BaseStation { Name = "BaseA", StationID = Config.RunIdStation++, AvailableChargeSlots = 50,
                Latitude = 33.3,
                Longtitude = 34.5
            });

            Stations.Add(new BaseStation { Name = "BaseB", StationID = Config.RunIdStation++,
            AvailableChargeSlots = 80, Latitude = 35, Longtitude = 34.2});
            #endregion
            for (int i = 0; i < 10; i++)
            {
                Drones.Add(new Drone
                {
                    DroneID = ++Config.RunIdDrone,
                    DroneModel = DronesModels[i],
                    DroneWeight = (WeightCategories)random.Next(0, 2)
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
            for (int i = 0; i < 7; i++)
            {
                Packages.Add(new Parcel
                {
                    ParcelId = Config.RunParcelId++,
                    SenderId = Clients[i].CustomerId,
                    TargetId = Clients[i + 1].CustomerId,
                    ParcelPriority = (Priorities)rand.Next(0, 2),
                    ParcelWeight = (WeightCategories)rand.Next(0, 2),
                    DroneId = Drones[i].DroneID,
                    Assignment = currentTime,
                    Delivered = i % 2 == 0 ? null : currentTime.AddDays(1),
                    PickedUp = currentTime.AddDays(2),
                    Created = currentTime.AddDays(3),
                }
                );
            }

            
        }
    }
}


