using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal struct DroneCharge
    {
        public int DroneID { get; internal set; }
        public int StationID { get; internal set; }

    }
    //string[] NameCustomers = new string[10] { "Tomer", "Yosef", "Yehuda", "Avi", "David", "Adi", "Moria", "Omer", "Ravit", "Eliyahu" };
    //            for (int i = 0; i< 10; i++)
    //            {
    //                Clients[config.Index_customer++] = new IDAL.DO.customer
    //                {
    //                    Id = config.RunCustomerId++,
    //                    Name = NameCustomers[i],
    //                    Phone = "05" + rand.Next(0, 9) + '-' + rand.Next(1000000, 10000000),
    //                    Latitude = rand.Next(0, 180),
    //                    Longtitude = rand.Next(0,180)
    //                };
    //            }
    //            //(name enum) randon.next(0 , 4)
    //  internal static int RunCustomerId = 0;
}
