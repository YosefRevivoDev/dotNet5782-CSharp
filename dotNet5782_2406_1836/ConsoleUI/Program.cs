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
                        AddNewDrone();
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
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                }
            }


            static void Main(string[] args)
            {


                MenuStartAplication();
                DalObject.DalObject obj = new DalObject.DalObject();
                int id;
                Console.WriteLine("please enter droneid: ");
                int.TryParse(Console.ReadLine(), out id);
                Console.WriteLine(obj.GetDrone(id));
                obj.Display(Display.BaseStation);

                Console.WriteLine("please enter your BaseStation: ");
                int idBase;
                string nameBase;
                BaseStation baseStation = new BaseStation();
                int.TryParse(Console.ReadLine(), out idBase);
                nameBase = Console.ReadLine();
                baseStation.StationID = idBase;
                baseStation.Name = nameBase;
                obj.Add_BaseStation(baseStation);
                obj.Display(Display.BaseStation);


            }
        }
    }
}
