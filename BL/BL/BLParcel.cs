using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BL : IBL
    {
        public void AddNewParcel(Parcel newParcel, int SenderId, int TargetId)
        {
            try
            {
                newParcel.Drone = null;
                DO.Parcel parcel = new()
                {
                    SenderId = SenderId,
                    TargetId = TargetId,
                    ParcelWeight = (DO.WeightCategories)newParcel.Weight,
                    ParcelPriority = (DO.Priorities)newParcel.Priority,
                    Created = DateTime.Now,
                    Assignment = null,
                    PickedUp = null,
                    Delivered = null
                };
                dal.AddParcel(parcel);
            }
            catch { }
        }
        public IEnumerable<ParcelToList> GetParcelToLists()
        {
            List<DO.Parcel> parcels = dal.GetPackagesByPredicate().ToList();
            List<ParcelToList> BLparcels = new();

            foreach (var item in parcels)
            {
                BLparcels.Add(new ParcelToList
                {
                    Id = item.ParcelId,
                    SenderId = item.SenderId,
                    TargetId = item.TargetId,
                    Weight = (WeightCategories)item.ParcelWeight,
                    Priority = (Priorities)item.ParcelPriority,
                    ParcelAreNotAssighmentToDrone = dal.GetPackagesByPredicate(x => x.DroneId == item.DroneId && x.Assignment == null).ToList().Count
                });
            }
            return BLparcels;
        }
        public Parcel GetParcel(int id)
        {
            DO.Parcel DalParcel = dal.GetParcel(id);

            Parcel BLParcel = new();

            BLParcel.Id = DalParcel.ParcelId;
            BLParcel.Sender = GetCustomer(DalParcel.SenderId);
            BLParcel.Target = GetCustomer(DalParcel.TargetId);
            BLParcel.Weight = (WeightCategories)DalParcel.ParcelWeight;
            BLParcel.Priority = (Priorities)DalParcel.ParcelPriority;
            BLParcel.CreateTime = DalParcel.Created;
            BLParcel.Requested = DalParcel.Assignment;
            BLParcel.PickedUp = DalParcel.PickedUp;
            BLParcel.Delivered = DalParcel.Delivered;


            if (DalParcel.DroneId != 0)
            {
                DroneToList drone = DroneToList.Find(i => i.DroneID == DalParcel.DroneId);
                BLParcel.Drone = new DroneInParcel();
                BLParcel.Drone.DroneID = drone.DroneID;
                BLParcel.Drone.BattaryStatus = drone.BattaryStatus;
                BLParcel.Drone.CorrentLocation = new Location()
                {
                    Latitude = drone.CurrentLocation.Latitude,
                    Longtitude = drone.CurrentLocation.Longtitude
                };
            }

            return BLParcel;
        }
        public void RemoveParcelBL(int id)
        {
            //IDAL.DO.BaseStation baseStationRemove = dal.GetBaseStation(id);
            dal.RemoveParcel(id);
        }
        public ParcelInDeliver GetParcelInDeliverd(Location firstLocation, int NumOfPackageId)
        {
            Parcel parcel = GetParcel(NumOfPackageId);
            DO.Customer SenderCustomer = dal.GetCustomer(parcel.Sender.CustomerId);
            DO.Customer TargetCustomer = dal.GetCustomer(parcel.Target.CustomerId);

            Location senderLocation = new() { Latitude = SenderCustomer.Latitude, Longtitude = SenderCustomer.Longtitude };
            Location targetLocation = new() { Latitude = TargetCustomer.Latitude, Longtitude = TargetCustomer.Longtitude };

            ParcelInDeliver ParcelInDeliverd = new()
            {
                ID = parcel.Id,
                Sender = parcel.Sender,
                Target = parcel.Target,
                Priorities = parcel.Priority,
                WeightCategories = parcel.Weight,

            };
            if (parcel.PickedUp == DateTime.MinValue)
            {
                ParcelInDeliverd.StatusParcrlInDeliver = StatusParcrlInDeliver.AwaitingCollection;
                ParcelInDeliverd.TransportDistance = helpFunction.DistanceBetweenLocations(firstLocation, senderLocation);
            }
            else
            {
                ParcelInDeliverd.StatusParcrlInDeliver = StatusParcrlInDeliver.OnTheWayDestination;
                ParcelInDeliverd.TransportDistance = helpFunction.DistanceBetweenLocations(senderLocation, targetLocation);
            }

            //ParcelInDeliverd.Sender = CustomerInParcel
            //ParcelInDeliverd



            return ParcelInDeliverd;
        }
        private ParcelStatus GetparcelStatus(DO.Parcel parcel)
        {
            if (parcel.Delivered != null)
            {
                return ParcelStatus.provided;
            }
            if (parcel.PickedUp != null)
            {
                return ParcelStatus.collected;
            }
            if (parcel.Assignment != null)
            {
                return ParcelStatus.associated;
            }
            return ParcelStatus.Defined;
        }
        public ParcelAtCustomer GetParcelAtCustomer(int parcelID, int customrID)
        {
            try
            {
                DO.Parcel parcel = dal.GetParcel(parcelID);

                ParcelAtCustomer parcelAtCustomer = new ParcelAtCustomer()
                {
                    Id = parcel.ParcelId,
                    Priority = (Priorities)parcel.ParcelPriority,
                    Weight = (WeightCategories)parcel.ParcelWeight
                };

                parcelAtCustomer.ParcelStatus = (parcel.Assignment == DateTime.MinValue) ? ParcelStatus.Defined :
                    (parcel.PickedUp == DateTime.MinValue) ? ParcelStatus.associated :
                    (parcel.Delivered == DateTime.MinValue) ? ParcelStatus.collected : ParcelStatus.provided;
              
                parcelAtCustomer.Sender = GetCustomerInParcel((parcel.SenderId == customrID) ? parcel.TargetId : parcel.SenderId);
                parcelAtCustomer.Target = GetCustomerInParcel((parcel.TargetId == customrID) ? parcel.SenderId : parcel.TargetId);

                return parcelAtCustomer;
            }
            catch (DO.CheckIdException ex)
            {
                throw new CheckIdException("ERORR", ex);
            }
            catch (DO.CheckIfIdNotException ex)
            {
                throw new CheckIfIdNotException("ERORR", ex);
            }
        }
        //private IEnumerable<ParcelAtCustomer> GetParcelAtCustomers(IEnumerable<DO.Parcel> DalPackagesIdCustomer, int id, bool flag)
        //{
        //    return from package in DalPackagesIdCustomer
        //           where flag ? package.SenderId == id : package.TargetId == id
        //           select GetParcelAtCustomer(package, id);
        //}
    }
}
