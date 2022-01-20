using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using DalApi;
using DO;
using System.Globalization;


namespace DAL
{
    sealed class DalXML : IDal
    {
        static readonly DalXML instance = new DalXML();

        public static IDal Instance { get => instance; }

        DalXML() { }

        string baseStationPath = @"BaseStationXml.xml";
        string dronePath = @"DroneXml.xml";
        string parcelPath = @"ParcelXml.xml";
        string customerPath = @"CustomerXml.xml";
        string droneChargePath = @"droneChargetXml.xml";

        #region Drone
        public void UpdateDrone(Drone drone)
        {
            List<Drone> ListDrone = XMLTools.LoadListToXMLSerializer<Drone>(dronePath);

            int index = ListDrone.FindIndex(i => i.DroneID == drone.DroneID);
            if (index == -1)
            {
                throw new Exception($"This Drone have not exist, Please try again.");
            }
            ListDrone[index] = drone;
        }
        public void AddDrone(Drone new_drone)
        {
            List<Drone> ListDrone = XMLTools.LoadListToXMLSerializer<Drone>(dronePath);
            ListDrone.Add(ListDrone.FindIndex(i => i.DroneID == new_drone.DroneID) == -1 ?
                           new_drone : throw new BaseStationException($"This id{new_drone.DroneID}already exist"));

            XMLTools.SaveListToXMLSerializer(ListDrone, dronePath);
        }
        public void RemoveDrone(int droneID)
        {
            //List<Drone> ListDrone = XMLTools.LoadListToXMLSerializer<Drone>(dronePath);

            //int index = DataSource.DroneCharges.FindIndex(i => i.DroneID == droneID);
            //DataSource.DroneCharges.RemoveAt(DataSource.DroneCharges.FindIndex(i => i.DroneID == droneID));
            //if (index == -1)
            //{
            //    throw new Exception($"This drone have not exist, Please try again.");
            //}
            //DataSource.Drones.RemoveAt(index);
            throw new NotImplementedException();

        }
        public Drone GetDrone(int Id)
        {
            List<Drone> ListDrone = XMLTools.LoadListToXMLSerializer<Drone>(dronePath);

            for (int i = 0; i < ListDrone.Count; i++)
            {
                if (Id == ListDrone[i].DroneID)
                {
                    return ListDrone[i];
                }
            }
            throw new Exception("Drone is not found");
        }
        public IEnumerable<Drone> GetDronesByPredicate(Predicate<Drone> predicate = null)
        {
            List<Drone> ListDrone = XMLTools.LoadListToXMLSerializer<Drone>(dronePath);

            return ListDrone.FindAll(i => predicate == null ? true : predicate(i));
        }
        #endregion

        #region DroneCharge
        public void UpdateDroneCharge(DroneCharge droneCharge)
        {
            throw new NotImplementedException();
        }
        public void AddDroneCharge(DroneCharge droneCharge)
        {
            throw new NotImplementedException();
        } // XElement
        public IEnumerable<DroneCharge> GetDroneChargesByPredicate(Predicate<DroneCharge> predicate = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region BaseStation

        public void AddBaseStation(BaseStation new_baseStation)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListToXMLSerializer<BaseStation>(baseStationPath);
            ListBaseStation.Add(ListBaseStation.FindIndex(i => i.StationID == new_baseStation.StationID) == -1 ?
                           new_baseStation : throw new BaseStationException($"This id{new_baseStation.StationID}already exist"));

            XMLTools.SaveListToXMLSerializer(ListBaseStation, baseStationPath);
        }

        public void RemoveBaseStation(int stationID)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListToXMLSerializer<BaseStation>(baseStationPath);
            int index = ListBaseStation.FindIndex(i => i.StationID == stationID);
            if (index == -1)
            {
                throw new Exception($"This station have not exist, Please try again.");
            }
            ListBaseStation.RemoveAt(index);
        }

        public void UpdateBaseStation(BaseStation baseStation)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListToXMLSerializer<BaseStation>(baseStationPath);
            int index = ListBaseStation.FindIndex(i => i.StationID == baseStation.StationID);
            if (index == -1)
            {
                throw new Exception($"This BaseStation have not exist, Please Try again.");
            }
            ListBaseStation[index] = baseStation;
        }

        public BaseStation GetBaseStation(int Id)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListToXMLSerializer<BaseStation>(baseStationPath);
            for (int i = 0; i < ListBaseStation.Count; i++)
            {
                if (Id == ListBaseStation[i].StationID)
                {
                    return ListBaseStation[i];
                }
            }
            throw new Exception("BaseStation is not found");
            //BaseStation baseStation = ListBaseStation.Find(b => b.StationID == Id);

            //if (baseStation != null)
            //{
            //    return baseStation;
            //}
            //else
            //{
            //    throw new Exception("BaseStation is not found");
            //}
        }
        public IEnumerable<BaseStation> GetBaseStationByPredicate(Predicate<BaseStation> predicate = null)
        {
            List<BaseStation> ListBaseStation = XMLTools.LoadListToXMLSerializer<BaseStation>(baseStationPath);

            return ListBaseStation.FindAll(i => predicate == null ? true : predicate(i));
        }
        #endregion

        #region Parcel
        public void AddParcel(Parcel new_parcel)
        {
            List<Parcel> ListParcel = XMLTools.LoadListToXMLSerializer<Parcel>(parcelPath);
            ListParcel.Add(ListParcel.FindIndex(i => i.ParcelId == new_parcel.ParcelId) == -1 ?
                           new_parcel : throw new BaseStationException($"This id{new_parcel.ParcelId}already exist"));

            XMLTools.SaveListToXMLSerializer(ListParcel, parcelPath);
        }
        public void RemoveParcel(int parcelId)
        {
            List<Parcel> ListParcel = XMLTools.LoadListToXMLSerializer<Parcel>(parcelPath);

            int index = ListParcel.FindIndex(i => i.ParcelId == parcelId);
            ListParcel.RemoveAt(ListParcel.FindIndex(i => i.ParcelId == parcelId));
            if (index == -1)
            {
                throw new Exception($"This parcel have not exist, Please try again.");
            }
            ListParcel.RemoveAt(index);
        }
        public Parcel GetParcel(int Id)
        {
            List<Parcel> ListParcel = XMLTools.LoadListToXMLSerializer<Parcel>(parcelPath);

            for (int i = 0; i < ListParcel.Count; i++)
            {
                if (Id == ListParcel[i].ParcelId)
                {
                    return ListParcel[i];
                }
            }
            throw new Exception("Parcel is not found");
        }
        public void UpdateParcel(Parcel parcel)
        {
            List<Parcel> ListParcel = XMLTools.LoadListToXMLSerializer<Parcel>(parcelPath);

            int index = ListParcel.FindIndex(i => i.ParcelId == parcel.ParcelId);
            if (index == -1)
            {
                throw new Exception($"This Parcel have not exist, Please try again.");
            }
            ListParcel[index] = parcel;
        }
        public IEnumerable<Parcel> GetPackagesByPredicate(Predicate<Parcel> predicate = null)
        {
            List<Parcel> ListParcel = XMLTools.LoadListToXMLSerializer<Parcel>(parcelPath);

            return ListParcel.FindAll(i => predicate == null ? true : predicate(i));
        }
        #endregion

        #region Customer
        public void RemoveCustomer(int customerId)
        {
            List<Customer> ListCustomer = XMLTools.LoadListToXMLSerializer<Customer>(customerPath);
            int index = ListCustomer.FindIndex(i => i.CustomerId == customerId);
            ListCustomer.RemoveAt(ListCustomer.FindIndex(i => i.CustomerId == customerId));
            if (index == -1)
            {
                throw new Exception($"This customer have not exist, Please try again.");
            }
            ListCustomer.RemoveAt(index);
        }
        public void UpdateCustomer(Customer customer)
        {
            List<Customer> ListCustomer = XMLTools.LoadListToXMLSerializer<Customer>(customerPath);

            int index = ListCustomer.FindIndex(i => i.CustomerId == customer.CustomerId);
            if (index == -1)
            {
                throw new Exception($"This Customer have not exist, Please try again.");
            }
            ListCustomer[index] = customer;
        }
        public void AddCustomer(Customer new_customer)
        {
            List<Customer> ListCustomer = XMLTools.LoadListToXMLSerializer<Customer>(customerPath);
            ListCustomer.Add(ListCustomer.FindIndex(i => i.CustomerId == new_customer.CustomerId) == -1 ?
                           new_customer : throw new BaseStationException($"This id{new_customer.CustomerId}already exist"));

            XMLTools.SaveListToXMLSerializer(ListCustomer, customerPath);
        }
        public Customer GetCustomer(int Id)
        {
            List<Customer> ListCustomer = XMLTools.LoadListToXMLSerializer<Customer>(customerPath);

            for (int i = 0; i < ListCustomer.Count; i++)
            {
                if (Id == ListCustomer[i].CustomerId)
                {
                    return ListCustomer[i];
                }
            }
            throw new Exception("Customer is not found");
        }
        public IEnumerable<Customer> GetCustomersByPredicate(Predicate<Customer> predicate = null)
        {
            List<Customer> ListCustomer = XMLTools.LoadListToXMLSerializer<Customer>(customerPath);
            return ListCustomer.FindAll(i => predicate == null ? true : predicate(i));

        }
        #endregion



        public void ChargeDrone(int droneId, int baseStationId)
        {
            throw new NotImplementedException();

        }


        public void DeliveredPackageToCustumer(int parcelId, int droneId)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string userName)
        {
            throw new NotImplementedException();
        }

        public void PackageCollectionByDrone(int parcelId, int droneId)
        {
            throw new NotImplementedException();
        }

        public void ReleasingChargeDrone(int droneId, int baseStationId)
        {
            throw new NotImplementedException();
        }

        public void SetDroneForParcel(int parcelId, int droneId)
        {
            throw new NotImplementedException();
        }

        public double[] RequetPowerConsumption()
        {
            throw new NotImplementedException();
        }

        public void MinusDroneCharge(int stationId)
        {
            throw new NotImplementedException();
        }

        public void PlusDroneCharge(int stationId)
        {
            throw new NotImplementedException();
        }

    }
}
