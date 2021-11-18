using System;
using IBL;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BO;


namespace ConsoleUI_BL
{
    class Program
    {
        static readonly IBL.IBL bl = new BL();

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
                    AddBaseStation();
                    break;
                case 2:
                    AddNewParcel();
                    break;
                case 3:
                    AddNewCustomer();
                    break;
                case 4:
                    AddDrone(bl);
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

        public static void AddDrone(IBL.IBL bl)
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

        public static void AddNewParcel()
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

        
           

            BL.AddNewParcel(parcel);

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
        public static void DisplayBaseStation(BO.BL bL)
        {
            int stationID;
            Console.WriteLine("Please enter station ID");
            int.TryParse(Console.ReadLine(), out stationID);
            BaseStation? station = bl.GetBaseStation(stationID);
            Console.WriteLine(string.Format("Station Details: {0}", station));
        }
        //Print Drone by string.Format
        public static void DisplayDrone(IBL.IBL bL)
        {
            int DroneID;
            Console.WriteLine("Please enter Drone ID");
            int.TryParse(Console.ReadLine(), out DroneID);
            Drone new_drone = bl.GetDrone(DroneID);

            Console.WriteLine(string.Format("Drone Details: {0}", new_drone));
        }
        //Print Customer by string.Format
        public static void DisplayCustomer(BO.BL bL)
        {
            int CustomerID;
            Console.WriteLine("Please enter Customer ID");
            int.TryParse(Console.ReadLine(), out CustomerID);
            Customer? new_customer = bl.GetCustomer(CustomerID);
            Console.WriteLine(string.Format("Customer Details: {0}", new_customer));
        }
        //Print Parcel by string.Format
        public static void DisplayParcel(BO.BL bL)
        {
            int ParcelID;
            Console.WriteLine("Please enter Parcel ID");
            int.TryParse(Console.ReadLine(), out ParcelID);
            Parcel? new_Parcel = bl.GetParcel(ParcelID);
            Console.WriteLine(string.Format("Parcel Details: {0}", new_Parcel));
        }

        //--------------------------------------------UPDATE OBJ FUNCTIONS---------------------------------------------//
        public static void UpdateOptions(int SubOptions, IBL.IBL bl)
        {
            switch (SubOptions)
            {
                case 1:
                    AssociateParcel(bl);
                    break;
                case 2:
                    packageCollectByDrone(bl);
                    break;
                case 3:
                    DeliveredParcel(bl);
                    break;
                case 4:
                    SendingDroneToCharge(bl);
                    break;
                case 5:
                    ReleaseDroneFromCharged(bl);
                    break;
            }
        }

        // Call PackageCollectionByDrone function by parcel ID & drone ID paramters
        private static void ReleaseDroneFromCharged(BO.BL bL)
        {
            int pacel_id_associate, drone_id_associate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out pacel_id_associate);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out drone_id_associate);
            bl.PackageCollectionByDrone(pacel_id_associate, drone_id_associate);
        }

        //Call DliveredPackageToCustumer function by parcel ID & drone ID paramters
        public static void SendingDroneToCharge(BO.BL bL)
        {
            int Pacel_id_associate, Prone_id_associate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out Pacel_id_associate);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out drone_id_associate);
            bl.DeliveredPackageToCustumer(pacel_id_associate, drone_id_associate);
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
        public static void ViewOptionsList(int SubOptions, BO.BL bl)
        {
            //Print list by choice (switch)
            switch (SubOptions)
            {
                case 1:
                    Console.WriteLine("All Station Data: ");
                    DisplayList(bl, bl.GetBaseStation());
                    break;
                case 2:
                    Console.WriteLine("All Customers Data: ");
                    DisplayList(bl, bl.GetCustomer());
                    break;
                case 3:
                    Console.WriteLine("All Drones Data: ");
                    DisplayList(bl, bl.GetDrones());
                    break;
                case 4:
                    Console.WriteLine("All Packages Data: ");
                    DisplayList(bl, bl.GetPackages());
                    break;
                case 5:
                    Console.WriteLine("Free Packages: ");
                    DisplayList(bl, bl.GetPackagesByPredicate());
                    break;
                case 6:
                    Console.WriteLine("Free Charge Slots: ");
                    DisplayList(bl, bl.GetBaseStationByPredicate());
                    break;
            }
        }
        //Print a generic list by object 
        public static void DisplayList<T>(BO.BL bl, T[] t)
        {
            foreach (T s in t)
            {
                Console.WriteLine(s);
            }
        }
    }
}

