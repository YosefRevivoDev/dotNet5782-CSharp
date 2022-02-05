using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DS
{
    public class DataSource
    {
        public static List<Drone> Drones = new List<Drone>();
        public static List<BaseStation> Stations = new List<BaseStation>();
        public static List<Customer> Customer = new List<Customer>();
        public static List<Parcel> Parcels = new List<Parcel>();
        public static List<DroneCharge> DroneCharges = new List<DroneCharge>();
        public static List<User> users = new List<User>();

        // Empty Constractor for Default
      
        public static class Config
        {
            // ------------- PowerConsumption by Drone -----------------//
            public static double PowerConsumptionAvailable = 0.01;
            public static double PowerConsumptionLightWeight = 0.04;
            public static double PowerConsumptionMediumWeight = 0.07;
            public static double PowerConsumptionHeavyWeight = 0.1;
            public static double LoadingDrone = 1.0;

            //------------------RunID---------------------------//

            public static int RunIdDrone = 0;
            public static int RunParcelId = 0;

        }

        public void Initialize()
        {
            Random random = new Random(DateTime.Now.Millisecond);
            #region User
            users = new List<User>
            {
                new User{FirstName = "Yosef", LastName = "Revivo", Password = "1"},
                new User{FirstName = "Tomer", LastName = "Zecharia", Password = "1"},
                new User{FirstName = "user", LastName = "user", Password = "1"},
                new User{FirstName = "admin", LastName = "admin", Password = "1"},
            };
            #endregion

            #region Drones
            string[] DronesModels = new string[]
            {
                    "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Beta1", "Beta2", "Beta3", "Beta4", "Beta5"
            };

            for (int i = 0; i < 10; i++)
            {
                Drones.Add(new Drone
                {
                    DroneID = ++Config.RunIdDrone,
                    DroneModel = DronesModels[i],
                    DroneWeight = (WeightCategories)random.Next(1, 3)
                });
            }
            #endregion

            #region BaseStation
            Stations.Add(new BaseStation
            {
                Name = "B.S.R 4",
                StationID = random.Next(1,20),
                Latitude = 32.0931651,
                Longtitude = 34.8247758,
                AvailableChargeSlots = 50,
            });

            Stations.Add(new BaseStation
            {
                Name = "מתנס קהילתי - ק.הרצוג",
                StationID = random.Next(21, 60),
                AvailableChargeSlots = 80,
                Latitude = 32.0964575,
                Longtitude = 34.8377433

            });
            #endregion
            
            #region Clients
            Customer.Add(new Customer
            {
                CustomerId = 200532406,
                Name = "יוסף רביבו",
                Phone = "05" + random.Next(0, 9) + '-' + random.Next(1000000, 10000000),
                Longtitude = 32.0932627,
                Latitude = 34.8306360

            });
            Customer.Add(new Customer()
            {
                CustomerId = 309865341,
                Name = "פיני איינהורן",
                Phone = "05" + random.Next(0, 9) + '-' + random.Next(1000000, 10000000),
                Longtitude = 32.0934950,
                Latitude = 34.8415697
            });
            Customer.Add(new Customer()
            {
                CustomerId = 203548976,
                Name = "יוני רבינוביץ",
                Phone = "05" + random.Next(0, 9) + '-' + random.Next(1000000, 10000000),
                Longtitude = 32.0937440,
                Latitude = 34.8364607
            });
            Customer.Add(new Customer()
            {
                CustomerId = 349927608,
                Name = "דניאל שרם",
                Phone = "05" + random.Next(0, 9) + '-' + random.Next(1000000, 10000000),
                Longtitude = 32.0878331,
                Latitude = 34.8901060
            });
            Customer.Add(new Customer()
            {
                CustomerId = 200786113,
                Name = "שוקי אלבירט",
                Phone = "05" + random.Next(0, 9) + '-' + random.Next(1000000, 10000000),
                Longtitude = 32.0940572,
                Latitude = 34.8362261
            });
            Customer.Add(new Customer()
            {
                CustomerId = 201344879,
                Name = "ליאור וענונו",
                Phone = "05" + random.Next(0, 9) + '-' + random.Next(1000000, 10000000),
                Longtitude = 32.0993644,
                Latitude = 34.8361656
            });
            Customer.Add(new Customer()
            {
                CustomerId = 301242853,
                Name = "אפרים שוובר",
                Phone = "05" + random.Next(0, 9) + '-' + random.Next(1000000, 10000000),
                Longtitude = 32.0943398,
                Latitude = 34.8289060
            });
            Customer.Add(new Customer()
            {
                CustomerId = 209433871,
                Name = "אבנר לבייב",
                Phone = "05" + random.Next(0, 9) + '-' + random.Next(1000000, 10000000),
                Longtitude = 32.0856264,
                Latitude = 34.8227299
            });
            Customer.Add(new Customer()
            {
                CustomerId = 311890522,
                Name = "יענקי סלוד",
                Phone = "05" + random.Next(0, 9) + '-' + random.Next(1000000, 10000000),
                Longtitude = 32.0963894,
                Latitude = 34.8322994
            });
            Customer.Add(new Customer()
            {
                CustomerId = 200996586,
                Name = "משה הורוביץ",
                Phone = "05" + random.Next(0, 9) + '-' + random.Next(1000000, 10000000),
                Longtitude = 32.0948123,
                Latitude = 34.8293454
            });
            #endregion

            #region Paecels
            for (int i = 0; i< 20; i++)
            {
                Parcels.Add(new Parcel
                {
                    ParcelId = Config.RunParcelId++,
                    SenderId = Customer[i].CustomerId,
                    TargetId = Customer[i + 1].CustomerId,
                    ParcelPriority = (Priorities) random.Next(1, 3),
                    ParcelWeight = (WeightCategories) random.Next(1, 3),
                    DroneId = Drones[i].DroneID,
                    Created = DateTime.Now,
                    Assignment = DateTime.MinValue,
                    PickedUp = DateTime.MinValue,
                    Delivered = i % 2 == 0 ? null : DateTime.MinValue,
                });
            }
            #endregion

        }
    }
}

/*static DalXml()
{
    XMLTools.SaveListToXMLSerializer(DataSource.Drones, dronePath);
    XMLTools.SaveListToXMLSerializer(DataSource.Customer, customerPath);
    XMLTools.SaveListToXMLSerializer(DataSource.Stations, baseStationPath);
    XMLTools.SaveListToXMLSerializer(DataSource.Parcels, parcelPath);
    XMLTools.SaveListToXMLSerializer(DataSource.users, userPath);
    XMLTools.SaveListToXMLSerializer(DataSource.DroneCharges, droneChargesPath);
}*/
