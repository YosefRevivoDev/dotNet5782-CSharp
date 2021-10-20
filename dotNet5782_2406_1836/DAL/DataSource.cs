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
            }

            public static void Initialize()
            {
                Random random = new Random(DateTime.Now.Millisecond);
                #region initStation
                Stations[config.Index_Station++] = new IDAL.DO.Station { Model = "fff", StationID = config.RunIdStation++ };
                Stations[config.Index_Station++] = new IDAL.DO.Station { Model = "aaa", StationID = config.RunIdStation++ };
                #endregion 


            }
        }

    }
    
}
