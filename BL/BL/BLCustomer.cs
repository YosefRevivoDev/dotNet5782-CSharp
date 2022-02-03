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
        public void AddNewCustomer(Customer newCustomer)
        {
            try
            {
                DO.Customer customer = new()
                {
                    CustomerId = newCustomer.CustomerId,
                    Name = newCustomer.NameCustomer,
                    Phone = newCustomer.PhoneCustomer,
                    Latitude = newCustomer.LocationCustomer.Latitude,
                    Longtitude = newCustomer.LocationCustomer.Longtitude
                };
                dal.AddCustomer(customer);
            }
            catch (DO.CheckIdException ex)
            {
                throw new CheckIdException("ERORR" , ex);
            }
        }

        public IEnumerable<CustomerToList> GetCustomerToList(Predicate<CustomerToList> p = null)
        {
            try
            {
                List<DO.Customer> customers = dal.GetCustomersByPredicate().ToList();
                List<CustomerToList> BLCustomer = new();

                foreach (DO.Customer item in customers)
                {
                    BLCustomer.Add(GetCustomerToList(item.CustomerId));
                }
                return BLCustomer.Where(i => p == null ? true : p(i)).ToList();
            }
            catch(DO.CheckIfIdNotException ex)
            {
                throw new CheckIfIdNotException("ERORR" , ex);
            }

        }
        public CustomerToList GetCustomerToList(int customerID)
        {
            try
            {
                Customer customer = GetCustomer(customerID);
                CustomerToList customerToList = new()
                {
                    CustomerId = customer.CustomerId,
                    NameCustomer = customer.NameCustomer,
                    Phone = customer.PhoneCustomer
                };
                List<ParcelAtCustomer> parcelSupplied = customer.PackagesToCustomer.FindAll(i => i.ParcelStatus == ParcelStatus.provided);
                customerToList.SendParcelAndSupplied = parcelSupplied.Count;

                List<ParcelAtCustomer> parcelOnWayToCustomer = customer.PackagesToCustomer.FindAll(i => i.ParcelStatus == ParcelStatus.provided);
                customerToList.ParcelOweyToCustomer = parcelOnWayToCustomer.Count;

                List<ParcelAtCustomer> parcelThatNotDelivered = customer.PackagesFromCustomer.FindAll(i => i.ParcelStatus == ParcelStatus.provided);
                customerToList.SendParcelAndNotSupplied = parcelThatNotDelivered.Count;

                List<ParcelAtCustomer> parcelcRecieved = customer.PackagesFromCustomer.FindAll(i => i.ParcelStatus == ParcelStatus.provided);
                customerToList.ParcelsReciever = parcelcRecieved.Count;

                return customerToList;
            }
            catch (DO.CheckIfIdNotException Ex)
            {
                throw new CheckIfIdNotException("ERORR", Ex);
            }

        }
        public Customer GetCustomer(int customrID)
        {
            try
            {
                DO.Customer DalCustomer = dal.GetCustomer(customrID);
                Customer BLCustomer = new()
                {
                    CustomerId = DalCustomer.CustomerId,
                    NameCustomer = DalCustomer.Name,
                    PhoneCustomer = DalCustomer.Phone,
                    LocationCustomer = new()
                    {
                        Latitude = DalCustomer.Latitude,
                        Longtitude = DalCustomer.Longtitude
                    },
                    PackagesToCustomer = new(),
                    PackagesFromCustomer = new()
                };

                List<DO.Parcel> parcelOfSender = dal.GetPackagesByPredicate(i => i.SenderId == customrID).ToList();
                foreach (DO.Parcel parcel in parcelOfSender)
                {
                    ParcelAtCustomer parcelAtCustomer = new();
                    parcelAtCustomer = GetParcelAtCustomer(parcel.ParcelId, customrID);
                    BLCustomer.PackagesFromCustomer.Add(parcelAtCustomer);
                }

                List<DO.Parcel> parcelOfTarget = dal.GetPackagesByPredicate(i => i.TargetId == customrID).ToList();
                foreach (DO.Parcel parcel in parcelOfTarget)
                {
                    ParcelAtCustomer parcelAtCustomer = new();
                    parcelAtCustomer = GetParcelAtCustomer(parcel.ParcelId, customrID);
                    BLCustomer.PackagesToCustomer.Add(parcelAtCustomer);
                }
                return BLCustomer;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public void UpdateCustomr(int customerId, string newNameCustomer, string newPhoneCustomer)
        {
            try
            {
                DO.Customer customer = dal.GetCustomer(customerId);
                customer.Name = newNameCustomer;
                customer.Phone = newPhoneCustomer;

                dal.UpdateCustomer(customer);
            }
            catch (DO.CustumerException)
            {

                throw new Exception(" ");
            }
        }
        public void RemoveCustomerBL(int id)
        {
            //IDAL.DO.BaseStation baseStationRemove = dal.GetBaseStation(id);
            dal.RemoveCustomer(id);
        }
        public CustomerInParcel GetCustomerInParcel(int customerID)
        {
            try
            {
                DO.Customer customer = dal.GetCustomer(customerID);
                CustomerInParcel customerInParcel = new()
                {
                    CustomerId = customer.CustomerId,
                    NameCustomer = customer.Name
                };
                return customerInParcel;
            }
            catch (DO.CheckIfIdNotException ex)
            {
                throw new CheckIfIdNotException("Error: " ,  ex);
            }
        }


    }
}
