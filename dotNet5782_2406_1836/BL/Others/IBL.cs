using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IBL
{
    public interface IBL
    {
        List<DroneToList> DroneToList { get; }
        public static void AddOptions(int SubOptions){ }

        BO.BaseStation GetBaseStation(int stationID);
        BO.Drone GetDrone(int droneID);
        BO.Customer GetCustomer(int customerID);
        BO.Parcel GetParcel(int parcelID);
        public IEnumerable<BasetationToList> GetBasetationToLists(Predicate<BasetationToList> predicate = null);
    }
}
