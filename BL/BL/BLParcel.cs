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
        public int AddNewParcel(Parcel newParcel, int SenderId, int TargetId)
        {
            newParcel.droneInParcel = null;
            DO.Parcel parcel = new()
            {
                SenderId = SenderId,
                TargetId = TargetId,
                ParcelWeight = (DO.WeightCategories)newParcel.Weight,
                ParcelPriority = (DO.Priorities)newParcel.Priority,
                Created = DateTime.Now,
                Assignment = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue
            };

            try
            {
                return dal.AddParcel(parcel);
            }
            catch { }
            return -1;
        }

        public Parcel GetParcel(int id)
        {
            try
            {
                DO.Parcel DalParcel = dal.GetParcel(id);

                Parcel BLParcel = new();

                BLParcel.Id = DalParcel.ParcelId;
                BLParcel.Weight = (WeightCategories)DalParcel.ParcelWeight;
                BLParcel.Priority = (Priorities)DalParcel.ParcelPriority;
                BLParcel.Requested = DalParcel.Created;
                BLParcel.assigned = DalParcel.Assignment;
                BLParcel.PickedUp = DalParcel.PickedUp;
                BLParcel.Delivered = DalParcel.Delivered;

                BLParcel.Sender = GetCustomerInParcel(DalParcel.SenderId);
                BLParcel.Target = GetCustomerInParcel(DalParcel.TargetId);
                if (DalParcel.DroneId != 0)
                {
                    DroneToList drone = DroneToList.Find(i => i.DroneID == DalParcel.DroneId);

                    BLParcel.droneInParcel = new DroneInParcel()
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
                throw new CheckIfIdNotExceptions("ERORR", Ex);
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
                    SenderName = parcel.Sender.NameCustomer,
                    TargetName = parcel.Target.NameCustomer,
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
                throw new CheckIfIdNotExceptions("ERORR", Ex);
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


        public ParcelInDeliver GetParcelInDeliverd(Location firstLocation, int parcelID)
        {
            try
            {
                DO.Parcel parcel = dal.GetParcel(parcelID);


                DO.Customer SenderCustomer = dal.GetCustomer(parcel.SenderId);
                DO.Customer TargetCustomer = dal.GetCustomer(parcel.TargetId);

                Location senderLocation = new() { Latitude = SenderCustomer.Latitude, Longtitude = SenderCustomer.Longtitude };
                Location targetLocation = new() { Latitude = TargetCustomer.Latitude, Longtitude = TargetCustomer.Longtitude };

                ParcelInDeliver ParcelInDeliverd = new()
                {
                    ID = parcel.ParcelId,

                    Priorities = (Priorities)parcel.ParcelPriority,
                    WeightCategories = (WeightCategories)parcel.ParcelWeight,

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

                ParcelInDeliverd.Sender = GetCustomerInParcel(parcel.SenderId);
                ParcelInDeliverd.Target = GetCustomerInParcel(parcel.TargetId);

                ParcelInDeliverd.CollectionLocation = targetLocation;
                ParcelInDeliverd.DeliveryDestination = senderLocation;

                return ParcelInDeliverd;
            }
            catch (DO.CheckIdException ex)
            {
                throw new CheckIdException("ERORR", ex);
            }
            catch (DO.CheckIfIdNotException ex)
            {
                throw new CheckIfIdNotExceptions("ERORR", ex);
            }
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
                throw new CheckIfIdNotExceptions("ERORR", ex);
            }
        }
    }
}

