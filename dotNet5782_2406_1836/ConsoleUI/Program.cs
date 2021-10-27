using System;
using DalObject;
using System.Linq;
using System.Text;
using IDAL.DO;
using System.Collections.Generic;
using static DalObject.DalObject;


namespace ConsoleUI
{
    class Program
    {

        static void MenuStartAplication()
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
                        AddOptions(SubOptions);
                        break;
                    case 2:
                        Console.WriteLine("Enter your Option please:\n");
                        Console.WriteLine(@"1 = Associate parcel to drone
                                            2 = Collect package by drone
                                            3 = Package delivery to customer
                                            4 = Sending a Drone for charging at the base station
                                            5 = Release Drone from charging at base station ");
                        break;
                    case 3:
                        Console.WriteLine("Enter your Option please:\n");
                        Console.WriteLine(@"1 = Base Station Display
                                             2 = Drone Display
                                             3 = Customer Display
                                             4 = Parcel Display");
                        break;
                    case4:
                        Console.WriteLine("Enter your Option please:\n");
                        Console.WriteLine(@"1 = Displays a base station list
                                            2 = Displays a list of Drone
                                            3 = Displays a list Customers
                                            4 = Displays a list Parcels
                                            5 = Displays a list of packages that have not yet been assigned to the Drone
                                            6 = Display of base stations with available charging stations");
                        break;
                    case 5:
                        break;
                    default:
                        Console.WriteLine("WRONG!,Please enter another choice again");
                        break;
                }
            } while (Options < 0 || Options > 5);

            static void AddOptions(int SubOptions)
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
                        AddParcelToShipping();
                        break;
                    default:
                        break;
                }
            }

            static void UpdateOptions(int SubOptions)
            {
                switch (SubOptions)
                {
                    case 1:
                        AssociateParcel();
                        break;
                    case 2:
                        packageCollectByDrone();
                        break;
                    case 3:
                        DeliveredParcel();
                        break;
                    case 4:
                        SendingDroneToCharge();
                        break;
                    case 5:
                        ReleaseDroneFromCharged();
                        break;
                }
            }


            static void ViewOptions(int SubOptions)
            {
                switch (SubOptions)
                {
                    case 1:
                        DisplayBaseStation();
                        break;
                    case 2:
                        DisplayDrone();
                        break;
                    case 3:
                        DisplayCustomer();
                        break;
                    case 4:
                        DisplayParcel();
                        break;
                }
            }


            static void ViewOptionsList(int SubOptions)
            {
                switch (SubOptions)
                {
                    case 1:
                        DisplayBaseStationList();
                        break;
                    case 2:
                        DisplayDronelist();
                        break;
                    case 3:
                        DisplayCustomerslist();
                        break;
                    case 4:
                        DisplayParcelslist();
                        break;
                }


            }
            //--------------------------------------------ADD FUNCTIONS---------------------------------------------//
            static void AddBaseStation()
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

                DalObject.DalObject.Add_BaseStation(station);

                Console.WriteLine("A new base station has been added");
            }
        }

        public static void AddNewParcel()
        {
            int ParcelId, SenderId, TargetId, DroneId, choice;

            Parcel parcel = new();
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

            Console.WriteLine(" "); ;
            int.TryParse(Console.ReadLine(), out Parcel_weight);
            parcel.Parcel_weight = (WeightCategories)Parcel_weight;

            Console.WriteLine(" ");
            int.TryParse(Console.ReadLine(), out Parcel_weight);
            parcel.Priority = (Priorities)intTemp;

            Console.WriteLine("");
            parcel.Scheduled = currentDate;

            DalObject.DalObject.Add_Parcel(parcel);

            Console.WriteLine("A new parcel has been added");
        }
        public static void AddNewCustomer()
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

            DalObject.DalObject.Add_Customer(customer);

            Console.WriteLine("A new base customer has been added");
        }

        public static void  AddParcelToShipping()
        {
        int id, sender_id, target_id, drone_id, parcel_weight, parcel_priorities;

        int choice;
        Parcel parcel = new();
        DateTime currentDate = DateTime.Now;

        Console.WriteLine("please enter id:");
        int.TryParse(Console.ReadLine(), out id);
        parcel.ParcelId = id;

            Console.WriteLine("please enter sender id:");
            int.TryParse(Console.ReadLine(), out sender_id);
        parcel.SenderId = sender_id;

            Console.WriteLine("please enter target id:");
            int.TryParse(Console.ReadLine(), out target_id);
        parcel.TargetId = target_id;

            Console.WriteLine("please enter drone id:");
            int.TryParse(Console.ReadLine(), out drone_id);
        parcel.DroneId = drone_id;

            Console.WriteLine("please enter parcel weight: (1 for Light, 2 for medium, 3 for Heavy)");
            int.TryParse(Console.ReadLine(), out parcel_weight);
        parcel.Parcel_weight = (WeightCategories)parcel_weight;

        Console.WriteLine("please enter parcel priority: (1 for Regular, 2 for fast, 3 for emergency)");
            int.TryParse(Console.ReadLine(), out parcel_priorities);
        parcel.Parcel_priority = (Priorities)parcel_priorities;
        }




        //--------------------------------------------UPDATE OBJ FUNCTIONS---------------------------------------------//

        public static void AssociateParcel()
        {
            int pacel_id_associate, drone_id_associate;

            Console.WriteLine("Please enter parcel ID that you what to associate with your drone");
            int.TryParse(Console.ReadLine(), out pacel_id_associate);
            Console.WriteLine("Please enter drone ID that you what to associate with your parcel");
            int.TryParse(Console.ReadLine(), out drone_id_associate);

            //Associat func. between pacel_id_associate to drone_id_associate
            SetDroneForParcel(pacel_id_associate, drone_id_associate);
        }








        //--------------------------------------------Display OBJ FUNCTIONS---------------------------------------------//
        public static void DisplayBaseStation()
        {
            int stationID;
            Console.WriteLine("Please enter station ID");
            int.TryParse(Console.ReadLine(), out stationID);
            BaseStation station = (BaseStation)GetBaseStation(stationID);
            Console.WriteLine(string.Format("Station Details: {0}", station));
        }

        public static void DisplayDrone()
        {
            int DroneID;
            Console.WriteLine("Please enter Drone ID");
            int.TryParse(Console.ReadLine(), out DroneID);
            Drone new_drone = GetDrone(DroneID);
            Console.WriteLine(string.Format("Drone Details: {0}", new_drone));
        }
        public static void DisplayCustomer()
        {
            int CustomerID;
            Console.WriteLine("Please enter Customer ID");
            int.TryParse(Console.ReadLine(), out CustomerID);
            Customer new_customer = GetCustomer(CustomerID);
            Console.WriteLine(string.Format("Customer Details: {0}", new_customer));
        }
        public static void DisplayParcel()
        {
            int ParcelID;
            Console.WriteLine("Please enter Parcel ID");
            int.TryParse(Console.ReadLine(), out ParcelID);
            Parcel new_Parcel = (Parcel)GetParcel(ParcelID);
            Console.WriteLine(string.Format("Parcel Details: {0}", new_Parcel));
        }


        //--------------------------------------------Display LIST OBJ FUNCTIONS---------------------------------------------//
        public static void DisplayBaseStationList()
        {
            BaseStation[] stations = DalObject.DalObject.copyArray();
            Console.WriteLine("All Station Data");
            foreach(BaseStation s in stations)
            {
                Console.WriteLine(s);
            }
            //----------------------------2 אופציות -----------------------------
            DalObject.DalObject.Display((Display)1);
        }
        public static void DisplayDronelist()
        {
            DalObject.DalObject.Display((Display)2);
        }
        public static void DisplayCustomerslist()
        {
            DalObject.DalObject.Display((Display)3);
        }
        public static void DisplayParcelslist()
        {
            DalObject.DalObject.Display((Display)4);
        }





        static void Main(string[] args)
        { 
            {
            }
        }
    }
}
