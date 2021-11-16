using System;
using IBL;
using IDAL.DO;
using System.Collections.Generic;
using DalObject;
using System.Linq;
using System.Text;
using BO;
namespace ConsoleUI_BL
{
    class Program
    {
        readonly IDAL.IDal DeliversByDroneCompany = new DalObject.DalObject();

        static void Main(string[] args)
        {
            IBL.IBL ObjBl = new BO.BL();
            MenuStartAplication(ObjBl);
        }

        public static void MenuStartAplication(IBL.IBL ObjBl)
        {
            int Options;
            int SubOptions;

            Console.WriteLine(@"Please Enter your choice: 
                                    1 = ADD
                                    2 = UPDATE
                                    3 = DISPLAY
                                    4 = WIEW_LIST
                                    5 = EXIT");

            do
            {
                int.TryParse(Console.ReadLine(), out Options);
                switch (Options)
                {
                    case 1:
                        Console.WriteLine("Enter your Option please:\n");
                        Console.WriteLine(@"
                                                1 = add BaseStation
                                                2 = add Drone
                                                3 = add Customer
                                                4 = add Parcel");
                        int.TryParse(Console.ReadLine(), out SubOptions);
                        AddOptions(SubOptions, ObjBl);
                        break;
                    case 2:
                        Console.WriteLine("Enter your Option please:\n");
                        Console.WriteLine(@"
                                                1 = Associate parcel to drone
                                                2 = Collect package by drone
                                                3 = Package delivery to customer
                                                4 = Sending a Drone for charging at the base station
                                                5 = Release Drone from charging at base station");
                        int.TryParse(Console.ReadLine(), out SubOptions);
                        UpdateOptions(SubOptions, ObjBl);
                        break;
                    case 3:
                        Console.WriteLine("Enter your Option please:\n");
                        Console.WriteLine(@"
                                                1 = Base Station Display
                                                2 = Drone Display
                                                3 = Customer Display
                                                4 = Parcel Display");
                        int.TryParse(Console.ReadLine(), out SubOptions);
                        ViewOptions(SubOptions, ObjBl);
                        break;
                    case 4:
                        Console.WriteLine("Enter your Option please:\n");
                        Console.WriteLine(@"
                                                1 = Displays a base station list
                                                2 = Displays a list of Drone
                                                3 = Displays a list Customers
                                                4 = Displays a list Parcels
                                                5 = Displays a list of packages that have not yet been assigned to the Drone
                                                6 = Display of base stations with available charging stations");
                        int.TryParse(Console.ReadLine(), out SubOptions);
                        viewOptionsList(SubOptions, ObjBl);
                        break;
                    case 5:
                        break;
                    default:
                        Console.WriteLine("WRONG!,Please enter another choice again");
                        break;
                }

            } while (Options != 5);
        }

        //--------------------------------------------ADD FUNCTIONS---------------------------------------------//
        public static void AddOptions(int SubOptions, IBL.IBL ObjBl)
        {
            switch (SubOptions)
            {
                case 1:
                    AddBaseStation(ObjBl);
                    break;
                case 2:
                    AddNewParcel(ObjBl);
                    break;
                case 3:
                    AddNewCustomer(ObjBl);
                    break;
                case 4:
                    AddDrone(ObjBl);
                    break;
                default:
                    break;
            }
        }
        static void AddBaseStation(IBL.IBL ObjBl)
        {
            int StationID, ChargeSlots;
            double Longtitude, Latitude;
            BaseStation station = new();

            Console.WriteLine("Please enter baseStation Id:");
            int.TryParse(Console.ReadLine(), out StationID);
            station.StationID = StationID;

            Console.WriteLine("Please enter baseStation name:");
            station.Name = Console.ReadLine();

            Console.WriteLine("Please enter number of charging station:");
            int.TryParse(Console.ReadLine(), out ChargeSlots);
            station.ChargeSlots = ChargeSlots;

            Console.WriteLine("Please enter the longitude:");
            double.TryParse(Console.ReadLine(), out Longtitude);
            station.Longtitude = Longtitude;

            Console.WriteLine("Please enter the latitude:");
            double.TryParse(Console.ReadLine(), out Latitude);
            station.Latitude = Latitude;

            List<IDAL.DO.DroneCharge> droneCharges = new();
            droneCharges.Clear();

            ObjBl.AddBaseStation(station);

            Console.WriteLine("A new base station has been added");
        }
        static void AddNewParcel(IBL.IBL ObjBl)
        {
            int ParcelId, SenderId, TargetId, DroneId;

            Parcel parcel = new Parcel();
            DateTime currentDate = DateTime.Now;
            Console.WriteLine("Please Add a new Parcel");
            int.TryParse(Console.ReadLine(), out ParcelId);
            parcel.ParcelId = ParcelId;

            Console.WriteLine("Please Enter SenderId ");
            int.TryParse(Console.ReadLine(), out SenderId);
            parcel.SenderId = SenderId;

            Console.WriteLine("Please Enter Target");
            int.TryParse(Console.ReadLine(), out TargetId);
            parcel.TargetId = TargetId;

            Console.WriteLine("Please Enter DroneId");
            int.TryParse(Console.ReadLine(), out DroneId);
            parcel.DroneId = DroneId;

            Console.WriteLine("Please Enter your Choice Weight: 1 = light, 2 = medium, 3 = heavy");
            int.TryParse(Console.ReadLine(), out int temp);
            parcel.Parcel_weight = (WeightCategories)temp;

            Console.WriteLine("Please Enter your Choice Parcel: 1 = regular,2 =  fast, 3= emergency");
            int.TryParse(Console.ReadLine(), out temp);
            parcel.Parcel_priority = (Priorities)temp;

            Console.WriteLine("The Time Requested is: ");
            DateTime dateTime = new DateTime();
            DateTime.TryParse(Console.ReadLine(), out dateTime);
            parcel.Requested = dateTime;

            Console.WriteLine("The Time Scheduled is: ");
            DateTime.TryParse(Console.ReadLine(), out dateTime);
            parcel.Scheduled = dateTime;

            Console.WriteLine("The Time PickedUp is: ");
            DateTime.TryParse(Console.ReadLine(), out dateTime);
            parcel.PickedUp = dateTime;

            Console.WriteLine("The Time Delivered is: ");
            DateTime.TryParse(Console.ReadLine(), out dateTime);
            parcel.Delivered = dateTime;

            ObjBl.Add_Parcel(parcel);

            Console.WriteLine("A new parcel has been added");
        }
        public static void AddNewCustomer(IBL.IBL ObjBl)
        {
            int CustomerId;
            double Longtitude, Latitude;
            BO.CustomerInParcel customer = new();

            Console.WriteLine("Please enter id:");
            int.TryParse(Console.ReadLine(), out CustomerId);
            customer.CustomerId = CustomerId;

            Console.WriteLine("Please enter phone number:");
            customer.phone = Console.ReadLine();

            Console.WriteLine("Please enter name:");
            customer.Name = Console.ReadLine();

            Console.WriteLine("Please enter longitude:");
            double.TryParse(Console.ReadLine(), out Longtitude);
            customer.Longtitude = Longtitude;

            Console.WriteLine("Please enter latitude:");
            double.TryParse(Console.ReadLine(), out Latitude);
            customer.Latitude = Latitude;

            ObjBl.AddCustomer(customer);

            Console.WriteLine("A new base customer has been added");
        }
        static void AddDrone(IBL.IBL ObjBl)
        {
            Drone drone = new();
            Console.WriteLine();

            int.TryParse(Console.ReadLine(), out int DroneID);
            drone.DroneID = DroneID;

            Console.WriteLine("Please enter phone number:");
            drone.Drone_Model = Console.ReadLine();


            int.TryParse(Console.ReadLine(), out int temp);
            drone.Drone_weight = (WeightCategories)temp;

            //double.TryParse(Console.ReadLine(), out double Battary);
            //drone.Battary = Battary;

            ObjBl.AddDrone(drone);
        }

        //--------------------------------------------Display OBJ FUNCTIONS---------------------------------------------//

        public static void ViewOptions(int SubOptions,IBL.IBL ObjBl)
        {
            switch (SubOptions)
            {
                case 1:
                    DisplayBaseStation(ObjBl);
                    break;
                case 2:
                    DisplayDrone(ObjBl);
                    break;
                case 3:
                    DisplayCustomer(ObjBl);
                    break;
                case 4:
                    DisplayParcel(ObjBl);
                    break;
            }
        }
        //Print BaseStation by string.Format
        public static void DisplayBaseStation(IBL.IBL ObjBl)
        {
            int stationID;
            Console.WriteLine("Please enter station ID");
            int.TryParse(Console.ReadLine(), out stationID);
            BaseStation? station = ObjBl.GetBaseStation(stationID);
            Console.WriteLine(string.Format("Station Details: {0}", station));
        }
        //Print Drone by string.Format
        public static void DisplayDrone(IBL.IBL ObjBl)
        {
            int DroneID;
            Console.WriteLine("Please enter Drone ID");
            int.TryParse(Console.ReadLine(), out DroneID);
            Drone new_drone = ObjBl.GetDrone(DroneID);

            Console.WriteLine(string.Format("Drone Details: {0}", new_drone));
        }
        //Print Customer by string.Format
        public static void DisplayCustomer(IBL.IBL ObjBl)
        {
            int CustomerID;
            Console.WriteLine("Please enter Customer ID");
            int.TryParse(Console.ReadLine(), out CustomerID);
            BO.Customer? new_customer = ObjBl.GetCustomer(CustomerID);
            Console.WriteLine(string.Format("Customer Details: {0}", new_customer));
        }
        //Print Parcel by string.Format
        public static void DisplayParcel(IBL.IBL ObjBl)
        {
            int ParcelID;
            Console.WriteLine("Please enter Parcel ID");
            int.TryParse(Console.ReadLine(), out ParcelID);
            BO.Parcel? new_Parcel = ObjBl.GetParcel(ParcelID);
            Console.WriteLine(string.Format("Parcel Details: {0}", new_Parcel));
        }

        //--------------------------------------------UPDATE OBJ FUNCTIONS---------------------------------------------//
        static void UpdateOptions(int SubOptions, IBL.IBL ObjBl)
        {
            switch (SubOptions)
            {
                case 1:
                    AssociateParcel(ObjBl);
                    break;
                case 2:
                    packageCollectByDrone(ObjBl);
                    break;
                case 3:
                    DeliveredParcel(ObjBl);
                    break;
                case 4:
                    SendingDroneToCharge(ObjBl);
                    break;
                case 5:
                    ReleaseDroneFromCharged(ObjBl);
                    break;
            }
        }

        // Call PackageCollectionByDrone function by parcel ID & drone ID paramters
        private static void ReleaseDroneFromCharged(IBL.IBL ObjBl)
        {
            int pacel_id_associate, drone_id_associate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out pacel_id_associate);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out drone_id_associate);
            ObjBl.PackageCollectionByDrone(pacel_id_associate, drone_id_associate);
        }

        //Call DliveredPackageToCustumer function by parcel ID & drone ID paramters
        private static void SendingDroneToCharge(IBL.IBL ObjBl)
        {
            int pacel_id_associate, drone_id_associate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out pacel_id_associate);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out drone_id_associate);
            ObjBl.DeliveredPackageToCustumer(pacel_id_associate, drone_id_associate);
        }

        //Call ChargeDrone function by parcel ID & drone ID paramters
        private static void DeliveredParcel(IBL.IBL ObjBl)
        {
            int baseStationId, drone_id_associate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out baseStationId);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out drone_id_associate);
            ObjBl.ChargeDrone(drone_id_associate, baseStationId);
        }

        //Call ChargeDrone function by baseStationId & drone ID paramters
        private static void packageCollectByDrone(IBL.IBL ObjBl)
        {
            int baseStationId, drone_id_associate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out baseStationId);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out drone_id_associate);
            ObjBl.ChargeDrone(drone_id_associate, baseStationId);
        }

        //Call ReleasingChargeDrone function by drone_id & baseStationId paramters
        public static void AssociateParcel(IBL.IBL ObjBl)
        {
            int baseStationId, drone_id_associate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out baseStationId);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out drone_id_associate);
            ObjBl.ReleasingChargeDrone(drone_id_associate, baseStationId);
        }


        //--------------------------------------------DISPLAY  LIST OBJ FUNCTIONS---------------------------------------------//
        public static void ViewOptionsList(int SubOptions, IBL.IBL ObjBl)
        {
            //Print list by choice (switch)
            switch (SubOptions)
            {
                case 1:
                    Console.WriteLine("All Station Data: ");
                    DisplayList(ObjBl, ObjBl.GetBaseStation());
                    break;
                case 2:
                    Console.WriteLine("All Customers Data: ");
                    DisplayList(ObjBl, ObjBl.GetCustomer());
                    break;
                case 3:
                    Console.WriteLine("All Drones Data: ");
                    DisplayList(ObjBl, ObjBl.GetDrones());
                    break;
                case 4:
                    Console.WriteLine("All Packages Data: ");
                    DisplayList(ObjBl, ObjBl.GetPackages());
                    break;
                case 5:
                    Console.WriteLine("Free Packages: ");
                    DisplayList(ObjBl, ObjBl.GetPackagesByPredicate());
                    break;
                case 6:
                    Console.WriteLine("Free Charge Slots: ");
                    DisplayList(ObjBl, ObjBl.GetBaseStationByPredicate());
                    break;
            }
        }
        //Print a generic list by object 
        public static void DisplayList<T>(IBL.IBL ObjBl, T[] t) where T : struct
        {
            foreach (T s in t)
            {
                Console.WriteLine(s);
            }
        }
    }
}
