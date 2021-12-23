using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BO;


namespace ConsoleUI_BL
{
    internal class ProgramMain
    {
        private static readonly IBL bl = new BL();

        private static void Main(string[] args)
        {
            //Console.WriteLine(String.Join("    ", bl.DroneToList));
            MenuStartAplication();
            //DalObject.DalObject dal = new DalObject.DalObject();
            //MenuStartAplication(dal).
        }

        #region
        readonly Random random = new(DateTime.Now.Millisecond);

        public static void MenuStartAplication()
        {
            int Options;
            int SubOptions;
   
            do
            {
                Console.WriteLine(@"Please Enter your choice: 
                        1 = ADD
                        2 = UPDATE
                        3 = DISPLAY
                        4 = WIEW_LIST
                        5 = EXIT");

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
                        UpdateOptions(SubOptions, bl);
                        break;
                    case 3:
                        Console.WriteLine("Enter your Option please:\n");
                        Console.WriteLine(@"
                                    1 = Base Station Display
                                    2 = Drone Display
                                    3 = Customer Display
                                    4 = Parcel Display");
                        int.TryParse(Console.ReadLine(), out SubOptions);
                        ViewOptions(SubOptions, bl);
                        break;
                    case 4:
                        Console.WriteLine("Enter your Option please:\n");
                        Console.WriteLine(@"
                                    1 = Displays a base station list
                                    2 = Displays a list Customers
                                    3 = Displays a list of Drone
                                    4 = Displays a list Parcels
                                    5 = Displays a list of packages that have not yet been assigned to the Drone
                                    6 = Display of base stations with available charging stations");
                        int.TryParse(Console.ReadLine(), out SubOptions);
                        ViewOptionsList(SubOptions, bl);
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
                    AddNewDrone();
                    break;
                case 3:
                    AddNewCustomer();
                    break;
                case 4:
                    AddNewParcel();
                    break;
                default:
                    break;
            }
        }

        public static void AddBaseStation()
        {
            BaseStation station = new();
            station.Location = new Location();
            Console.WriteLine("Please enter baseStation Id: ");
            int.TryParse(Console.ReadLine(), out int stationID);
            station.ID = stationID;

            Console.WriteLine("Please enter baseStation name: ");
            station.Name = Console.ReadLine();

            Console.WriteLine("Please enter number of available charge slots station: ");
            int.TryParse(Console.ReadLine(), out int chargeSlots);
            station.AvailableChargingStations = chargeSlots;

            Console.WriteLine("Please enter the longitude: ");
            double.TryParse(Console.ReadLine(), out double longtitude);
            station.Location.Longtitude = longtitude;

            Console.WriteLine("Please enter the latitude: ");
            double.TryParse(Console.ReadLine(), out double latitude);
            station.Location.Latitude = latitude;

            // list of DroneCharge & initiolize the list to empty list 
            station.droneCharges = new List<DroneCharge>();
            Console.WriteLine("A new basestation has been added ");
            bl.AddBaseStation(station);

     
        }

        public static void AddNewDrone()
        {

            //IDAL.DO.Drone drone = new();
            DroneToList drone = new();

            Console.WriteLine("Please enter number DroneID: ");
            int.TryParse(Console.ReadLine(), out int DroneID);
            drone.DroneID = DroneID;

            Console.WriteLine("Please enter phone number: ");
            drone.DroneModel = Console.ReadLine();

            Console.WriteLine("Please enter drone weight: 1 to light ,2 to medium ,3 to heavy ");//לבדוק חריגה אם הוזן מספר גדול מ 3 
            int.TryParse(Console.ReadLine(), out int temp);
            drone.DroneWeight = (WeightCategories)temp;// MaxWeight

            Console.WriteLine("Please enter number of station to loading the drone: ");
            int.TryParse(Console.ReadLine(), out int stationId);
           
          //  bl.AddNewDrone(drone, stationId);
            Console.WriteLine("A new drone has been added ");
        }

        public static void AddNewCustomer()
        {
            Customer customer = new();
            customer.LocationCustomer = new();

            Console.WriteLine("Please enter id: ");
            int.TryParse(Console.ReadLine(), out int CustomerId);
            customer.CustomerId = CustomerId;

            Console.WriteLine("Please enter name: ");
            customer.Name = Console.ReadLine();

            Console.WriteLine("Please enter phone number: ");
            customer.PhoneCustomer = Console.ReadLine();

            Console.WriteLine("Please enter longitude: ");
            double.TryParse(Console.ReadLine(), out double Longtitude);
            customer.LocationCustomer.Longtitude = Longtitude;

            Console.WriteLine("Please enter latitude: ");
            double.TryParse(Console.ReadLine(), out double Latitude);
            customer.LocationCustomer.Latitude = Latitude;

            bl.AddNewCustomer(customer);

            Console.WriteLine("A new base customer has been added  ");
        }

        public static void AddNewParcel()
        {

            Parcel parcel = new();

            DateTime currentDate = DateTime.Now;

            Console.WriteLine("Please Enter SenderId ");
            int.TryParse(Console.ReadLine(), out int senderId);

            Console.WriteLine("Please Enter Target ");
            int.TryParse(Console.ReadLine(), out int targetId);
           
            Console.WriteLine("Please Enter your Choice Weight: 1 = light , 2 = medium , 3 = heavy ");
            int.TryParse(Console.ReadLine(), out int WeightParcel);
            parcel.Weight = (BO.WeightCategories)WeightParcel;

            Console.WriteLine("Please Enter your Choice Parcel: 1 = regular ,2 =  fast , 3= emergency ");
            int.TryParse(Console.ReadLine(), out int priority);
            parcel.Priority = (BO.Priorities)priority;
            Console.WriteLine("A new parcel has been added");
            bl.AddNewParcel(parcel, senderId, targetId);
        }


        //--------------------------------------------Display OBJ FUNCTIONS---------------------------------------------//

        public static void ViewOptions(int SubOptions, IBL bl)
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
        public static void DisplayBaseStation(IBL bL)
        {
            Console.WriteLine("Please enter station ID: ");
            int.TryParse(Console.ReadLine(), out int stationID);
            BaseStation station = bl.GetBaseStation(stationID);
            Console.WriteLine(string.Format("Station Details: {0} ", station));
        }
        //Print Drone by string.Format
        public static void DisplayDrone(IBL bl)
        {
            Console.WriteLine("Please enter Drone ID: ");
            int.TryParse(Console.ReadLine(), out int DroneID);
            Drone newDrone = bl.GetDrone(DroneID);

            Console.WriteLine(string.Format("Drone Details: {0} " , newDrone));
        }
        
        public static void DisplayCustomer(IBL bL)
        {
            Console.WriteLine("Please enter Customer ID: ");
            int.TryParse(Console.ReadLine(), out int CustomerID);
            Customer newCustomer = bl.GetCustomer(CustomerID);
            Console.WriteLine(string.Format("Customer Details: {0} " , newCustomer));
        }

        public static void DisplayParcel(IBL bL)
        {
            Console.WriteLine("Please enter Parcel ID: ");
            int.TryParse(Console.ReadLine(), out int ParcelID);
            Parcel new_Parcel = bl.GetParcel(ParcelID);
            Console.WriteLine(string.Format("Parcel Details: {0} " , new_Parcel));
        }

        //--------------------------------------------UPDATE OBJ FUNCTIONS---------------------------------------------//
        public static void UpdateOptions(int SubOptions, IBL bL)
        {
            switch (SubOptions)
            {
                case 1:
                  //AssociateParcel(bL);
                    break;
                case 2:
                //packageCollectByDrone(bL);
                    break;
                case 3:
                 //   DeliveredParcel(bl);
                    break;
                case 4:
                // SendingDroneToCharge(bl);
                    break;
                case 5:
               //  ReleaseDroneFromCharged(bl);
                    break;
            }
        }

        

       

        // Call PackageCollectionByDrone function by parcel ID & drone ID paramters
        private static void ReleaseDroneFromCharge(IBL bL)
        {
            Console.WriteLine("Please enter parcel ID that you what to associate with your drone: ");
            int.TryParse(Console.ReadLine(), out int pacelIdAssociate);
            Console.WriteLine("Please enter drone ID that you what to associate with your parcel: ");
            int.TryParse(Console.ReadLine(), out int droneIdAssociate);
            bl.ReleaseDroneFromCharge(droneIdAssociate, pacelIdAssociate, DateTime.Now);
        }

        //Call DliveredPackageToCustumer function by parcel ID & drone ID paramters
        public static void SendingDroneToCharge(IBL bL)
        {

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone: ");
            int.TryParse(Console.ReadLine(), out int parcelIdAssociate);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel: ");
            int.TryParse(Console.ReadLine(), out int droneIdAssociate);
            bl.SendDroneToCharge(parcelIdAssociate, droneIdAssociate);
        }

        //Call ChargeDrone function by parcel ID & drone ID paramters
        //private static void DeliveredParcel(IBL bL)
        //{

        //    Console.WriteLine("Please enter parcel ID that you what to associate with your drone: ");
        //    int.TryParse(Console.ReadLine(), out int baseStationId);

        //    Console.WriteLine("Please enter drone ID that you what to associate with your parcel: ");
        //    int.TryParse(Console.ReadLine(), out int droneIdAssociate);

        //    bL.ChargeDrone(droneIdAssociate, baseStationId);
        //}

        ////Call ChargeDrone function by baseStationId & drone ID paramters
        //private static void packageCollectByDrone(IBL bL)
        //{

        //    Console.WriteLine("Please enter parcel ID that you what to associate with your drone: ");
        //    int.TryParse(Console.ReadLine(), out int baseStationId);

        //    Console.WriteLine("Please enter drone ID that you what to associate with your parcel: ");
        //    int.TryParse(Console.ReadLine(), out int droneIdAssociate);
        //    bL.(droneIdAssociate, baseStationId);
        //}

        //Call ReleasingChargeDrone function by drone_id & baseStationId paramters
        public static void AssociateParcel(IBL bL)
        {

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone: ");
            int.TryParse(Console.ReadLine(), out int baseStationId);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel; ");
            int.TryParse(Console.ReadLine(), out int droneIdAssociate);
            bl.ReleaseDroneFromCharge(droneIdAssociate, baseStationId, DateTime.Now);// לבדוק עם יהודה 
        }


        //--------------------------------------------DISPLAY  LIST OBJ FUNCTIONS---------------------------------------------//
        public static void ViewOptionsList(int SubOptions, IBL bl)
        {
            //Print list by choice (switch)
            switch (SubOptions)
            {
                case 1:
                    Console.WriteLine("All Available Station Data: ");
                    DisplayList(bl, bl.GetBasetationToLists());
                    break;
                // bl.GetBaseStation.findAll(i => i.Availablestation > 0); להוסיף תנאי להצגת תחנות בסיס פנויות
                case 2:
                    Console.WriteLine("All Customers Data: ");
                    DisplayList(bl, bl.GetCustomerToList());
                    break;
                case 3:
                    Console.WriteLine("All Drones Data: ");
                    DisplayList(bl, bl.DroneToList);
                    break;
                case 4:
                    Console.WriteLine("All Packages Data: ");
                    DisplayList(bl, bl.GetParcelToLists());
                    break;
            }
        }
        //Print a generic list by object 
        public static void DisplayList<T>(IBL bL, IEnumerable<T> t)
        {
            foreach (T s in t)
            {
                Console.WriteLine(s);
            }
        }
        #endregion
    }
}

