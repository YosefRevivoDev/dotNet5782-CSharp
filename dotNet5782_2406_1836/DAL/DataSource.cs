using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DalObject
    {
        class DataSource
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
            }
        }

    }
    
}
