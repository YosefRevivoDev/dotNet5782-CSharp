using System;
using IBL;
using IDAL.DO;
using DalObject;
using System.Collections.Generic;
using BO;


namespace ConsoleUI_BL
{
    class Program
    {
        static IBL.IBL bl = new BL();

        static void Main(string[] args)
        {
            bl.DroneToLists.ForEach(i => Console.WriteLine(i));
        }


        //   Random random = new(DateTime.Now.Millisecond);

        static void MenuStartAplication()
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
                        AddOptions(SubOptions);
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
                        UpdateOptions(SubOptions);
                        break;
                    case 3:
                        Console.WriteLine("Enter your Option please:\n");
                        Console.WriteLine(@"
                                    1 = Base Station Display
                                    2 = Drone Display
                                    3 = Customer Display
                                    4 = Parcel Display");
                        int.TryParse(Console.ReadLine(), out SubOptions);
                        ViewOptions(SubOptions);
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
                        viewOptionsList(SubOptions);
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
        public static void AddOptions(int SubOptions)
        {
            switch (SubOptions)
            {
                case 1:
                    AddBaseStation(bl);
                    break;
                case 2:
                    AddNewParcel(bl);
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

        public static void AddBaseStation()
        {
            BO.BaseStation station = new();

            Console.WriteLine("Please enter baseStation Id:");
            int.TryParse(Console.ReadLine(), out int stationID);
            station.ID = stationID;

            Console.WriteLine("Please enter baseStation name:");
            station.Name = Console.ReadLine();

            Console.WriteLine("Please enter number of available charge slots station:");
            int.TryParse(Console.ReadLine(), out int chargeSlots);
            station.AvailableChargingStations = chargeSlots;

            Console.WriteLine("Please enter the longitude:");
            double.TryParse(Console.ReadLine(), out double longtitude);
            station.Location.Longtitude = longtitude;

            Console.WriteLine("Please enter the latitude:");
            double.TryParse(Console.ReadLine(), out double latitude);
            station.Location.Latitude = latitude;

            // list of DroneCharge & initiolize the list to empty list 
            station.droneCharge = new List<BO.DroneCharge>();

            BL.AddBaseStation(station);

            Console.WriteLine("A new basestation has been added");
        }

        public static void AddDrone()
        {

            // IDAL.DO.Drone drone = new();
            BO.Drone drone = new();

            Console.WriteLine("Please enter number DroneID");
            int.TryParse(Console.ReadLine(), out int DroneID);
            drone.DroneID = DroneID;

            Console.WriteLine("Please enter phone number:");
            drone.Drone_Model = Console.ReadLine();

            Console.WriteLine("Please enter drone weight: 1 to light ,2 to medium,3 to heavy");//לבדוק חריגה אם הוזן מספר גדול מ 3 
            int.TryParse(Console.ReadLine(), out int temp);
            drone.Drone_weight = (BO.WeightCategories)temp;// MaxWeight

            Console.WriteLine("Please enter number DroneID");
            int.TryParse(Console.ReadLine(), out int firstChargeStation);

            BL.AddNewDrone(drone, firstChargeStation);
        }

        public static void AddNewCustomer()
        {
            int CustomerId;
            double Longtitude, Latitude;
            BO.Customer customer = new();

            Console.WriteLine("Please enter id:");
            int.TryParse(Console.ReadLine(), out CustomerId);
            customer.CustomerId = CustomerId;

            Console.WriteLine("Please enter name:");
            customer.Name = Console.ReadLine();

            Console.WriteLine("Please enter phone number:");
            customer.Phone = Console.ReadLine();

            Console.WriteLine("Please enter longitude:");
            double.TryParse(Console.ReadLine(), out Longtitude);
            customer.LocationCustomer.Longtitude = Longtitude;

            Console.WriteLine("Please enter latitude:");
            double.TryParse(Console.ReadLine(), out Latitude);
            customer.LocationCustomer.Latitude = Latitude;

            BL.AddNewCustomer(customer);

            Console.WriteLine("A new base customer has been added");
        }

        static void AddNewParcel(DalObject.DalObject dal)
        {

            BO.Parcel parcel = new();

            DateTime currentDate = DateTime.Now;

            Console.WriteLine("Please Enter SenderId ");
            int.TryParse(Console.ReadLine(), out int senderId);
            parcel.SenderId = senderId;

            Console.WriteLine("Please Enter Target");
            int.TryParse(Console.ReadLine(), out int targetId);
            parcel.TargetId = targetId;

            Console.WriteLine("Please Enter your Choice Weight: 1 = light, 2 = medium, 3 = heavy");
            int.TryParse(Console.ReadLine(), out int WeightParcel);
            parcel.weight = (BO.WeightCategories) WeightParcel;

            Console.WriteLine("Please Enter your Choice Parcel: 1 = regular,2 =  fast, 3= emergency");
            int.TryParse(Console.ReadLine(), out int priority);
            parcel.Priority = (BO.Priorities) priority;

            Console.WriteLine("The Time Requested is: ");
            DateTime.TryParse(Console.ReadLine(), out DateTime requested);
            

            Console.WriteLine("The Time Scheduled is: ");
            DateTime.TryParse(Console.ReadLine(), out DateTime createTime);
            

            Console.WriteLine("The Time PickedUp is: ");
            DateTime.TryParse(Console.ReadLine(), out DateTime pickedUp);
            

            Console.WriteLine("The Time Delivered is: ");
            DateTime.TryParse(Console.ReadLine(), out DateTime delivered);
            

            BL.AddNewParcel(parcel , requested, createTime, pickedUp, delivered);

            Console.WriteLine("A new parcel has been added");
        }
        
        
        //--------------------------------------------Display OBJ FUNCTIONS---------------------------------------------//

        public static void ViewOptions(int SubOptions, BO.BL bl)
        {
            switch (SubOptions)
            {
                case 1:
                    DisplayBaseStation(bl);
                    break;
                case 2:
                    DisplayDrone(bl);
                    break;
                case 3:
                    DisplayCustomer(bl);
                    break;
                case 4:
                    DisplayParcel(bl);
                    break;
            }
        }
        //Print BaseStation by string.Format
        public static void DisplayBaseStation(DalObject.DalObject dal)
        {
            int stationID;
            Console.WriteLine("Please enter station ID");
            int.TryParse(Console.ReadLine(), out stationID);
            BaseStation? station = dal.GetBaseStation(stationID);
            Console.WriteLine(string.Format("Station Details: {0}", station));
        }
        //Print Drone by string.Format
        public static void DisplayDrone(DalObject.DalObject dal)
        {
            int DroneID;
            Console.WriteLine("Please enter Drone ID");
            int.TryParse(Console.ReadLine(), out DroneID);
            Drone new_drone = dal.GetDrone(DroneID);

            Console.WriteLine(string.Format("Drone Details: {0}", new_drone));
        }
        //Print Customer by string.Format
        public static void DisplayCustomer(DalObject.DalObject dal)
        {
            int CustomerID;
            Console.WriteLine("Please enter Customer ID");
            int.TryParse(Console.ReadLine(), out CustomerID);
            Customer? new_customer = dal.GetCustomer(CustomerID);
            Console.WriteLine(string.Format("Customer Details: {0}", new_customer));
        }
        //Print Parcel by string.Format
        public static void DisplayParcel(DalObject.DalObject dal)
        {
            int ParcelID;
            Console.WriteLine("Please enter Parcel ID");
            int.TryParse(Console.ReadLine(), out ParcelID);
            Parcel? new_Parcel = dal.GetParcel(ParcelID);
            Console.WriteLine(string.Format("Parcel Details: {0}", new_Parcel));
        }

        //--------------------------------------------UPDATE OBJ FUNCTIONS---------------------------------------------//
        static void UpdateOptions(int SubOptions, DalObject.DalObject dal)
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

        // Call PackageCollectionByDrone function by parcel ID & drone ID paramters
        private static void ReleaseDroneFromCharged(DalObject.DalObject dal)
        {
            int pacel_id_associate, drone_id_associate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out pacel_id_associate);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out drone_id_associate);
            dal.PackageCollectionByDrone(pacel_id_associate, drone_id_associate);
        }

        //Call DliveredPackageToCustumer function by parcel ID & drone ID paramters
        private static void SendingDroneToCharge(DalObject.DalObject dal)
        {
            int pacel_id_associate, drone_id_associate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out pacel_id_associate);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out drone_id_associate);
            dal.DeliveredPackageToCustumer(pacel_id_associate, drone_id_associate);
        }

        //Call ChargeDrone function by parcel ID & drone ID paramters
        private static void DeliveredParcel(DalObject.DalObject dal)
        {
            int baseStationId, drone_id_associate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out baseStationId);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out drone_id_associate);
            dal.ChargeDrone(drone_id_associate, baseStationId);
        }

        //Call ChargeDrone function by baseStationId & drone ID paramters
        private static void packageCollectByDrone(DalObject.DalObject dal)
        {
            int baseStationId, drone_id_associate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out baseStationId);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out drone_id_associate);
            dal.ChargeDrone(drone_id_associate, baseStationId);
        }

        //Call ReleasingChargeDrone function by drone_id & baseStationId paramters
        public static void AssociateParcel(DalObject.DalObject dal)
        {
            int baseStationId, drone_id_associate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out baseStationId);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out drone_id_associate);
            dal.ReleasingChargeDrone(drone_id_associate, baseStationId);
        }


        //--------------------------------------------DISPLAY  LIST OBJ FUNCTIONS---------------------------------------------//
        public static void viewOptionsList(int SubOptions, DalObject.DalObject dal)
        {
            //Print list by choice (switch)
            switch (SubOptions)
            {
                case 1:
                    Console.WriteLine("All Station Data: ");
                    DisplayList(dal, dal.GetBaseStation());
                    break;
                case 2:
                    Console.WriteLine("All Customers Data: ");
                    DisplayList(dal, dal.GetCustomer());
                    break;
                case 3:
                    Console.WriteLine("All Drones Data: ");
                    DisplayList(dal, dal.GetDrones());
                    break;
                case 4:
                    Console.WriteLine("All Packages Data: ");
                    DisplayList(dal, dal.GetPackages());
                    break;
                case 5:
                    Console.WriteLine("Free Packages: ");
                    DisplayList(dal, dal.GetPackagesByPredicate());
                    break;
                case 6:
                    Console.WriteLine("Free Charge Slots: ");
                    DisplayList(dal, dal.GetBaseStationByPredicate());
                    break;
            }
        }
        //Print a generic list by object 
        public static void DisplayList<T>(DalObject.DalObject dal, T[] t) where T : struct
        {
            foreach (T s in t)
            {
                Console.WriteLine(s);
            }
        }
    }
}

