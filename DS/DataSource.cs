﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DS
{
    public static class DataSource
    {
        public static List<Drone> Drones = new List<Drone>();
        public static List<BaseStation> Stations = new List<BaseStation>();
        public static List<Customer> Clients = new List<Customer>();
        public static List<Parcel> Packages = new List<Parcel>();
        public static List<DroneCharge> DroneCharges = new List<DroneCharge>();
        public static List<User> users;
        public static Random rand = new Random();
        public static DateTime currentTime = DateTime.Now;


        // Empty Constractor for Default
        static DataSource()
        {
            Initialize();

            
        }
        public static class Config
        {
            // ------------- PowerConsumption by Drone -----------------//
            public static double PowerConsumptionAvailable = 0.01;
            public static double PowerConsumptionLightWeight = 0.04;
            public static double PowerConsumptionMediumWeight = 0.07;
            public static double PowerConsumptionHeavyWeight = 0.1;
            public static double LoadingDrone = 2;

            //------------------RunID---------------------------//

            public static int RunIdStation = 1;
            public static int RunIdDrone = 1;
            public static int RunCustomerId = 1;
            public static int RunParcelId = 1;

        }
        
        public static void Initialize()
        {
            users = new List<User>
            {
                new User{FirstName = "Yosef", LastName = "Revivo", Password = "1"},
                new User{FirstName = "Tomer", LastName = "Zecharia", Password = "1"},
                new User{FirstName = "user", LastName = "user", Password = "1"},
                new User{FirstName = "admin", LastName = "admin", Password = "1"},
            };
            
            Random random = new Random(DateTime.Now.Millisecond);

            string[] DronesModels = new string[]
            {
                    "Alpha1", "Alpha2", "Alpha3", "Beta1", "Beta2", "Beta3", "Beta4", "Beta5", "Beta6", "Beta7"
            };
            
            Stations.Add(new BaseStation { Name = "BaseA", StationID = Config.RunIdStation++, AvailableChargeSlots = 50,
                Latitude = 33.3,
                Longtitude = 34.5
            });

            Stations.Add(new BaseStation { Name = "BaseB", StationID = Config.RunIdStation++,
            AvailableChargeSlots = 80, Latitude = 35, Longtitude = 34.2});

            for (int i = 0; i < 10; i++)
            {
                Drones.Add(new Drone
                {
                    DroneID = ++Config.RunIdDrone,
                    DroneModel = DronesModels[i],
                    DroneWeight = (WeightCategories)random.Next(0, 2)
                });
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
                });
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


