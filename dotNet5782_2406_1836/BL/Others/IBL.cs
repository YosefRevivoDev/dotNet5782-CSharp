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
        void AddBaseStation(BaseStation station);
        object[] GetBaseStationByPredicate();
        object[] GetBaseStation(int stationID);
        object[] GetCustomer(int customerID);
        void ReleasingChargeDrone(int drone_id_associate, int baseStationId);
        void ChargeDrone(int drone_id_associate, int baseStationId);
        void DeliveredPackageToCustumer(int pacel_id_associate, int drone_id_associate);
        void PackageCollectionByDrone(int pacel_id_associate, int drone_id_associate);
        BO.Parcel GetParcel(int parcelID);
        Drone GetDrone(int droneID);
        void AddDrone(Drone drone);
        object[] GetPackagesByPredicate();
        object[] GetPackages();
        object[] GetDrones();
        object[] GetCustomer();
        object[] GetBaseStation();
    }
}
