using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DalObject
    {
        static class DataSource
        {
            static IDAL.DO.Drone[] Drones = new IDAL.DO.Drone[10];
            static IDAL.DO.Station[] Stations = new IDAL.DO.Station[5];
            static IDAL.DO.customer[] Clients = new IDAL.DO.customer[100];
            static IDAL.DO.Parcel[] Packages = new IDAL.DO.Parcel[1000];
            static Random rand = new Random();
            internal class config
            {
                internal static int Index_Drone = 0;
                internal static int Index_Station = 0;
                internal static int Index_customer = 0;
                internal static int Index_Parcel = 0;
                internal static int Parcel_RunNum = 0;
                internal static int RunCustomerId = 0;
            }
            public static void v()
            {
                string[] NameCustomers = new string[10] { "Tomer", "Yosef", "Yehuda","Avi", "David", "Adi", "Moria","Omer","Ravit","Eliyahu" };
                for (int i = 0; i < 10; i++)
                {
                    Clients[config.Index_customer++] = new IDAL.DO.customer
                    {
                        Id = config.RunCustomerId++,
                        Name = NameCustomers[i],
                        Phone = "05" + rand.Next(0, 9) + '-' + rand.Next(1000000, 10000000),
                        Latitude = rand.Next(0, 180),
                        Longtitude = rand.Next(0,180)
                    };
                }
                //(name enum) randon.next(0 , 4)
              //  DateTime dateTime = new DateTime(2021, rand.Next(1, 13), rand.Next(0, 31));

            }
        }

    }
    
}
