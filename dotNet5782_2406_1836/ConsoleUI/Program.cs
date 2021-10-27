using System;
using DalObject;
using System.Linq;
using System.Text;
using IDAL.DO;
using System.Collections.Generic;



namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            DalObject.DalObject dal = new DalObject.DalObject();
            MenuStartAplication(dal);
        }
        public static void MenuStartAplication(DalObject.DalObject dal)
        {
            int Options;
            int SubOptions;

            Console.WriteLine(@"Please Enter your choice: 
                                1 = ADD
                                2= UPDATE
                                3= DISPLAY
                                4= WIEW_LIST
                                5 = EXIT ");

            do
            {
                int.TryParse(Console.ReadLine(), out Options);
                switch (Options)
                {
                    case 1:
                        Console.WriteLine("Enter your Option please:\n");
                        Console.WriteLine(@"1 = add BaseStation
                                            2 = add Drone
                                            3 = add Customer
                                            4 = add Parcel ");
                        int.TryParse(Console.ReadLine(), out SubOptions);
                        AddOptions(SubOptions, dal);
                        break;
                    case 2:
                        Console.WriteLine("Enter your Option please:\n");
                        Console.WriteLine(@"1 = Associate parcel to drone
                                            2 = Collect package by drone
                                            3 = Package delivery to customer
                                            4 = Sending a Drone for charging at the base station
                                            5 = Release Drone from charging at base station ");
                        UpdateOptions(8, dal);
                        break;
                    case 3:
                        Console.WriteLine("Enter your Option please:\n");
                        Console.WriteLine(@"1 = Base Station Display
                                             2 = Drone Display
                                             3 = Customer Display
                                             4 = Parcel Display");
                        ViewOptions(0, dal);
                        break;
                    case 4:
                        Console.WriteLine("Enter your Option please:\n");
                        Console.WriteLine(@"1 = Displays a base station list
                                            2 = Displays a list of Drone
                                            3 = Displays a list Customers
                                            4 = Displays a list Parcels
                                            5 = Displays a list of packages that have not yet been assigned to the Drone
                                            6 = Display of base stations with available charging stations");
                        viewOptionsList(0, dal);
                        break;
                    case 5:
                        break;
                    default:
                        Console.WriteLine("WRONG!,Please enter another choice again");
                        break;
                }
            } while (Options < 0 || Options > 5);        
        }

        //--------------------------------------------ADD FUNCTIONS---------------------------------------------//
        public static void AddOptions(int SubOptions, DalObject.DalObject dal)
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
        public static void AddBaseStation(DalObject.DalObject dal)
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

            dal.Add_BaseStation(station);

            Console.WriteLine("A new base station has been added");
        }
        public static void AddNewParcel(DalObject.DalObject dal)
        {
            int ParcelId, SenderId, TargetId, DroneId;

            Parcel parcel = new Parcel();
            DateTime currentDate = DateTime.Now;
            Console.WriteLine(" ");
            int.TryParse(Console.ReadLine(), out ParcelId);
            parcel.ParcelId = ParcelId;

            Console.WriteLine(" ");
            int.TryParse(Console.ReadLine(), out SenderId);
            parcel.SenderId = SenderId;

            Console.WriteLine(" ");
            int.TryParse(Console.ReadLine(), out TargetId);
            parcel.TargetId = TargetId;

            Console.WriteLine(" ");
            int.TryParse(Console.ReadLine(), out DroneId);
            parcel.DroneId = DroneId;

            Console.WriteLine(" ");
            
            int.TryParse(Console.ReadLine(), out int temp);
            parcel.Parcel_weight = (WeightCategories)temp;

            Console.WriteLine(" ");
            int.TryParse(Console.ReadLine(), out temp);
            parcel.Parcel_priority = (Priorities)temp;

            Console.WriteLine(" ");
            DateTime dateTime = new DateTime();
            DateTime.TryParse(Console.ReadLine(), out dateTime);
            parcel.Requested = dateTime;

            Console.WriteLine(" ");
            DateTime.TryParse(Console.ReadLine(), out dateTime);
            parcel.Scheduled = dateTime;

            Console.WriteLine(" ");
            DateTime.TryParse(Console.ReadLine(), out dateTime);
            parcel.PickedUp = dateTime;

            Console.WriteLine(" ");
            DateTime.TryParse(Console.ReadLine(), out dateTime);
            parcel.Delivered = dateTime;

            dal.Add_Parcel(parcel);

            Console.WriteLine("A new parcel has been added");
        }
        public static void AddNewCustomer(DalObject.DalObject dal)
        {
            int CustomerId;
            double Longtitude, Latitude;
            Customer customer = new();

            Console.WriteLine("please enter id:");
            int.TryParse(Console.ReadLine(), out CustomerId);
            customer.CustomerId = CustomerId;

            Console.WriteLine("please enter phone number:");
            customer.Phone = Console.ReadLine();

            Console.WriteLine("please enter name:");
            customer.Name = Console.ReadLine();

            Console.WriteLine("please enter longitude:");
            double.TryParse(Console.ReadLine(), out Longtitude);
            customer.Longtitude = Longtitude;

            Console.WriteLine("please enter latitude:");
            double.TryParse(Console.ReadLine(), out Latitude);
            customer.Latitude = Latitude;

            dal.Add_Customer(customer);

            Console.WriteLine("A new base customer has been added");
        }
        public static void AddDrone(DalObject.DalObject dal)
        {
            Drone drone = new();
            Console.WriteLine();

            int.TryParse(Console.ReadLine(), out int DroneID);
            drone.DroneID = DroneID;

            Console.WriteLine("please enter phone number:");
            drone.Drone_Model = Console.ReadLine();


            int.TryParse(Console.ReadLine(), out int temp);
            drone.Drone_weight = (WeightCategories)temp;

            double.TryParse(Console.ReadLine(), out double Battary);
            drone.Battary = Battary;

            dal.Add_Drone(drone);
        }

        //--------------------------------------------Display OBJ FUNCTIONS---------------------------------------------//

        public static void ViewOptions(int SubOptions, DalObject.DalObject dal)
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
        public static void DisplayBaseStation(DalObject.DalObject dal)
        {
            int stationID;
            Console.WriteLine("Please enter station ID");
            int.TryParse(Console.ReadLine(), out stationID);
            BaseStation? station = dal.GetBaseStation(stationID);
            Console.WriteLine(string.Format("Station Details: {0}", station));
        }
        public static void DisplayDrone(DalObject.DalObject dal)
        {
            int DroneID;
            Console.WriteLine("Please enter Drone ID");
            int.TryParse(Console.ReadLine(), out DroneID);
            Drone new_drone = dal.GetDrone(DroneID);
          
            Console.WriteLine(string.Format("Drone Details: {0}", new_drone));
        }
        public static void DisplayCustomer(DalObject.DalObject dal)
        {
            int CustomerID;
            Console.WriteLine("Please enter Customer ID");
            int.TryParse(Console.ReadLine(), out CustomerID);
            Customer? new_customer = dal.GetCustomer(CustomerID);
            Console.WriteLine(string.Format("Customer Details: {0}", new_customer));
        }
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

        private static void ReleaseDroneFromCharged(DalObject.DalObject dal)
        {
            throw new NotImplementedException();
        }

        private static void SendingDroneToCharge(DalObject.DalObject dal)
        {
            throw new NotImplementedException();
        }

        private static void DeliveredParcel(DalObject.DalObject dal)
        {
            throw new NotImplementedException();
        }

        private static void packageCollectByDrone(DalObject.DalObject dal)
        {
            throw new NotImplementedException();
        }

        public static void AssociateParcel(DalObject.DalObject dal)
        {
            int pacel_id_associate, drone_id_associate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out pacel_id_associate);

            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out drone_id_associate);

            //Associat func. between pacel_id_associate to drone_id_associate
           dal.SetDroneForParcel(pacel_id_associate, drone_id_associate);
        }


        //--------------------------------------------DISPLAY  LIST OBJ FUNCTIONS---------------------------------------------//
        public static void viewOptionsList(int SubOptions, DalObject.DalObject dal)
        {
            switch (SubOptions)
            {
                case 1:
                    Console.WriteLine("All Station Data: ");
                    DisplayList(dal, dal.GetBaseStation());
                    break;
                case 2:
                    Console.WriteLine("All Customers Data: ");
                    DisplayList(dal, dal.GetCustomers());
                    break;
                case 3:
                    Console.WriteLine("All Drones Data: ");
                    DisplayList(dal, dal.GetDrones());
                    break;
                case 4:
                    Console.WriteLine("All Packages Data: ");
                    DisplayList(dal, dal.GetPackages());
                    break;
            }
        }
        public static void DisplayList<T>(DalObject.DalObject dal, T[] t) where T : struct
        {
            foreach (T s in t)
            {
                Console.WriteLine(s);
            }
        }    
    }
}
