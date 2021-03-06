//Yosef Revivo
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using DO;
using DalApi;

namespace Console_Dal
{
    class Program
    {
        static IDal dal = DalFactory.GetDal();

        static void Main(string[] args)
        {
            IDal dal = DalFactory.GetDal();
            MenuStartAplication(dal);
        }

        public static void MenuStartAplication(IDal dal)
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
                        AddOptions(SubOptions, dal);
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
                        UpdateOptions(SubOptions, dal);
                        break;
                    case 3:
                        Console.WriteLine("Enter your Option please:\n");
                        Console.WriteLine(@"
                                            1 = Base Station Display
                                            2 = Drone Display
                                            3 = Customer Display
                                            4 = Parcel Display");
                        int.TryParse(Console.ReadLine(), out SubOptions);
                        ViewOptions(SubOptions, dal);
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
                        viewOptionsList(SubOptions, dal);
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
        public static void AddOptions(int SubOptions, IDal dal)
        {
            switch (SubOptions)
            {
                case 1:
                    AddBaseStation(dal);
                    break;
                case 2:
                    AddNewParcel(dal);
                    break;
                case 3:
                    AddNewCustomer(dal);
                    break;
                case 4:
                    AddDrone(dal);
                    break;
                default:
                    break;
            }
        }
        public static void AddBaseStation(IDal dal)
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
            station.AvailableChargeSlots = ChargeSlots;

            Console.WriteLine("Please enter the longitude:");
            double.TryParse(Console.ReadLine(), out Longtitude);
            station.Longtitude = Longtitude;

            Console.WriteLine("Please enter the latitude:");
            double.TryParse(Console.ReadLine(), out Latitude);
            station.Latitude = Latitude;

            dal.AddBaseStation(station);

            Console.WriteLine("A new base station has been added");
        }
        public static void AddNewParcel(IDal dal)
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
            parcel.ParcelWeight = (WeightCategories)temp;

            Console.WriteLine("Please Enter your Choice Parcel: 1 = regular,2 =  fast, 3= emergency");
            int.TryParse(Console.ReadLine(), out temp);
            parcel.ParcelPriority = (Priorities)temp;

            Console.WriteLine("The Time Requested is: ");
            DateTime dateTime = new DateTime();
            DateTime.TryParse(Console.ReadLine(), out dateTime);
            parcel.Assignment = dateTime;

            Console.WriteLine("The Time Scheduled is: ");
            DateTime.TryParse(Console.ReadLine(), out dateTime);
            parcel.Created = dateTime;

            Console.WriteLine("The Time PickedUp is: ");
            DateTime.TryParse(Console.ReadLine(), out dateTime);
            parcel.PickedUp = dateTime;

            Console.WriteLine("The Time Delivered is: ");
            DateTime.TryParse(Console.ReadLine(), out dateTime);
            parcel.Delivered = dateTime;

            dal.AddParcel(parcel);

            Console.WriteLine("A new parcel has been added");
        }
        public static void AddNewCustomer(IDal dal)
        {
            int CustomerId;
            double Longtitude, Latitude;
            Customer customer = new();

            Console.WriteLine("Please enter id:");
            int.TryParse(Console.ReadLine(), out CustomerId);
            customer.CustomerId = CustomerId;

            Console.WriteLine("Please enter phone number:");
            customer.Phone = Console.ReadLine();

            Console.WriteLine("Please enter name:");
            customer.Name = Console.ReadLine();

            Console.WriteLine("Please enter longitude:");
            double.TryParse(Console.ReadLine(), out Longtitude);
            customer.Longtitude = Longtitude;

            Console.WriteLine("Please enter latitude:");
            double.TryParse(Console.ReadLine(), out Latitude);
            customer.Latitude = Latitude;

            dal.AddCustomer(customer);

            Console.WriteLine("A new base customer has been added");
        }
        public static void AddDrone(IDal dal)
        {
            Drone drone = new();
            Console.WriteLine();

            int.TryParse(Console.ReadLine(), out int DroneID);
            drone.DroneID = DroneID;

            Console.WriteLine("Please enter phone number:");
            drone.DroneModel = Console.ReadLine();


            int.TryParse(Console.ReadLine(), out int temp);
            drone.DroneWeight = (WeightCategories)temp;

            //double.TryParse(Console.ReadLine(), out double Battary);
            //drone.Battary = Battary;

            dal.AddDrone(drone);
        }

        //--------------------------------------------Display OBJ FUNCTIONS---------------------------------------------//

        public static void ViewOptions(int SubOptions, IDal dal)
        {
            switch (SubOptions)
            {
                case 1:
                    DisplayBaseStation(dal);
                    break;
                case 2:
                    DisplayDrone(dal);
                    break;
                case 3:
                    DisplayCustomer(dal);
                    break;
                case 4:
                    DisplayParcel(dal);
                    break;
            }
        }
        //Print BaseStation by string.Format
        public static void DisplayBaseStation(IDal dal)
        {
            int stationID;
            Console.WriteLine("Please enter station ID");
            int.TryParse(Console.ReadLine(), out stationID);
            BaseStation? station = dal.GetBaseStation(stationID);
            Console.WriteLine(string.Format("Station Details: {0}", station));
        }
        //Print Drone by string.Format
        public static void DisplayDrone(IDal dal)
        {
            int DroneID;
            Console.WriteLine("Please enter Drone ID");
            int.TryParse(Console.ReadLine(), out DroneID);
            Drone new_drone = dal.GetDrone(DroneID);

            Console.WriteLine(string.Format("Drone Details: {0}", new_drone));
        }
        //Print Customer by string.Format
        public static void DisplayCustomer(IDal dal)
        {
            int CustomerID;
            Console.WriteLine("Please enter Customer ID");
            int.TryParse(Console.ReadLine(), out CustomerID);
            Customer? new_customer = dal.GetCustomer(CustomerID);
            Console.WriteLine(string.Format("Customer Details: {0}", new_customer));
        }
        //Print Parcel by string.Format
        public static void DisplayParcel(IDal dal)
        {
            int ParcelID;
            Console.WriteLine("Please enter Parcel ID");
            int.TryParse(Console.ReadLine(), out ParcelID);
            Parcel? new_Parcel = dal.GetParcel(ParcelID);
            Console.WriteLine(string.Format("Parcel Details: {0}", new_Parcel));
        }

        //--------------------------------------------UPDATE OBJ FUNCTIONS---------------------------------------------//
        static void UpdateOptions(int SubOptions, IDal dal)
        {
            switch (SubOptions)
            {
                case 1:
                    AssociateParcel(dal);
                    break;
                case 2:
                    packageCollectByDrone(dal);
                    break;
                case 3:
                    DeliveredParcel(dal);
                    break;
                case 4:
                    SendingDroneToCharge(dal);
                    break;
                case 5:
                    ReleaseDroneFromCharged(dal);
                    break;
            }
        }

        private static void UpdateDrone()
        {

            Console.WriteLine("Please enter Drone ID: ");
            int.TryParse(Console.ReadLine(), out int id);
            //לקרוא לפונקציה שמזירה את הרחפן עם ה ID הנל
            //droneToUpadate.DroneID = id;

            Console.WriteLine("Please enter New Drone Model: ");
            //droneToUpadate.DroneModel = Console.ReadLine();
            //לקרוא לפונקציה שמזירה את הרחפן עם ה ID הנל
        }

        // Call PackageCollectionByDrone function by parcel ID & drone ID paramters
        private static void ReleaseDroneFromCharged(IDal dal)
        {
            int pacelIdAssociate, droneIdAssociate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out pacelIdAssociate);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out droneIdAssociate);
            dal.PackageCollectionByDrone(pacelIdAssociate);
        }

        //Call DliveredPackageToCustumer function by parcel ID & drone ID paramters
        private static void SendingDroneToCharge(IDal dal)
        {
            Console.WriteLine("Please enter BaseStation Id that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out int baseStationId);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out int droneIdAssociate);
            // dal.DeliveredPackageToCustumer(pacelIdAssociate, droneIdAssociate);
            dal.GetDroneChargeByStation(baseStationId);

        }

        //Call ChargeDrone function by parcel ID & drone ID paramters
        private static void DeliveredParcel(IDal dal)
        {
            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out int pacelIdAssociate);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out int droneIdAssociate);
            //dal.ChargeDrone(droneIdAssociate, baseStationId);
            dal.DeliveredPackageToCustumer(pacelIdAssociate);

        }

        //Call ChargeDrone function by baseStationId & drone ID paramters
        private static void packageCollectByDrone(IDal dal)
        {
            int baseStationId, droneIdAssociate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out baseStationId);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out droneIdAssociate);
            dal.GetDroneChargeByStation(baseStationId);
        }

        //Call ReleasingChargeDrone function by drone_id & baseStationId paramters
        public static void AssociateParcel(IDal dal)
        {
            int baseStationId, droneIdAssociate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out baseStationId);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out droneIdAssociate);
            dal.ReleasingChargeDrone(droneIdAssociate);
        }


        //--------------------------------------------DISPLAY  LIST OBJ FUNCTIONS---------------------------------------------//
        public static void viewOptionsList(int SubOptions, IDal dal)
        {
            //Print list by choice (switch)
            switch (SubOptions)
            {
                case 1:
                    Console.WriteLine("Free Charge Slots: ");
                    DisplayList(dal, dal.GetBaseStationByPredicate());
                    break;
                case 2:
                    Console.WriteLine("All Customers Data: ");
                    DisplayList(dal, dal.GetCustomersByPredicate());
                    break;
                case 3:
                    Console.WriteLine("All Drones Data: ");
                    DisplayList(dal, dal.GetDronesByPredicate());
                    break;
                case 4:
                    Console.WriteLine("Free Packages: ");
                    DisplayList(dal, dal.GetPackagesByPredicate());
                    break;
                case 5:

                case 6:
                    Console.WriteLine("Free Charge Slots: ");
                    DisplayList(dal, dal.GetBaseStationByPredicate());
                    break;
            }
        }
        //Print a generic list by object 
        public static void DisplayList<T>(IDal dal, IEnumerable<T> ts) where T : struct
        {
            foreach (T s in ts)
            {
                Console.WriteLine(s);
            }
        }
    }
}
