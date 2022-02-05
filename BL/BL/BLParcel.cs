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
        public Parcel GetParcel(int id)
        {
            try
            {
                DO.Parcel DalParcel = dal.GetParcel(id);

                Parcel BLParcel = new();

                BLParcel.Id = DalParcel.ParcelId;
                //BLParcel.Sender = GetCustomer(DalParcel.SenderId);
                //BLParcel.Target = GetCustomer(DalParcel.TargetId);
                BLParcel.Weight = (WeightCategories)DalParcel.ParcelWeight;
                BLParcel.Priority = (Priorities)DalParcel.ParcelPriority;
                BLParcel.assigned = DalParcel.Created;
                BLParcel.Requested = DalParcel.Assignment;
                BLParcel.PickedUp = DalParcel.PickedUp;
                BLParcel.Delivered = DalParcel.Delivered;

                BLParcel.Sender = GetCustomerInParcel(DalParcel.SenderId);
                BLParcel.Target = GetCustomerInParcel(DalParcel.TargetId);

                if (DalParcel.DroneId != 0)
                {
                    DroneToList drone = DroneToList.Find(i => i.DroneID == DalParcel.DroneId);

                    DroneInParcel droneInParcel = new DroneInParcel()
                    {
                        DroneID = drone.DroneID,
                        BattaryStatus = drone.BattaryStatus,
                        CorrentLocation = new Location()
                        {
                            Latitude = drone.CurrentLocation.Latitude,
                            Longtitude = drone.CurrentLocation.Longtitude
                        }
                    };
                }
                return BLParcel;
            }
            catch (DO.CheckIfIdNotException Ex)
            {
                throw new CheckIfIdNotException("ERORR", Ex);
            }

        }
        public void RemoveParcelBL(int id)
        {
            //IDAL.DO.BaseStation baseStationRemove = dal.GetBaseStation(id);
            dal.RemoveParcel(id);
        }

        public ParcelToList GetParcelToList(int parcelID)
        {
            try
            {
                Parcel parcel = GetParcel(parcelID);

                ParcelToList BLparcels = new()
                {
                    Id = parcel.Id,
                    SenderId = parcel.Sender.CustomerId,
                    TargetId = parcel.Target.CustomerId,
                    Weight = parcel.Weight,
                    Priority = parcel.Priority,
                };
                BLparcels.parcelStatus = (parcel.assigned == DateTime.MinValue) ? ParcelStatus.Defined :
                    (parcel.PickedUp == DateTime.MinValue) ? ParcelStatus.associated :
                    (parcel.Delivered == DateTime.MinValue) ? ParcelStatus.collected : ParcelStatus.provided;

                return BLparcels;
            }
            catch (DO.CheckIfIdNotException Ex)
            {
                throw new CheckIfIdNotException("ERORR", Ex);
            }
        }
        public IEnumerable<ParcelToList> GetParcelToListsByPredicate(Predicate<ParcelToList> p = null)
        {
            List<ParcelToList> parcelToLists = new();

            List<DO.Parcel> parcels = dal.GetPackagesByPredicate().ToList();
            foreach (DO.Parcel item in parcels)
            {
                parcelToLists.Add(GetParcelToList(item.ParcelId));
            }
            return parcelToLists.Where(i => p == null ? true : p(i)).ToList();


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
        public ParcelStatus GetparcelStatus(DO.Parcel parcel)
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
    }
}

