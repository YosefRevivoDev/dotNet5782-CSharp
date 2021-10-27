using System;
using IDAL.DO;
namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {

            int choice;
            int.TryParse(Console.ReadLine(), out choice);
            Console.WriteLine(
            "Enter option number:\n" +
            "1. Add options\n" +
            "2. Update options\n" +
            "3. View options\n" +
            "4. View lists options\n" +
            "5. Exit");

            do
            {
                switch (choice)
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
                    default:
                        break;
                }
            }while (choice < 0 || choice > 5);


























            //DalObject.DalObject obj = new DalObject.DalObject();
            //int id;
            //Console.WriteLine("please enter droneid: ");
            //int.TryParse(Console.ReadLine(), out id);
            //Console.WriteLine(obj.GetDrone(id));
            //obj.Display(Display.BaseStation);

            //Console.WriteLine("please enter your BaseStation: ");
            //int idBase;
            //string nameBase;
            //BaseStation baseStation = new BaseStation();
            //int.TryParse(Console.ReadLine(),out idBase);
            //nameBase = Console.ReadLine();
            //baseStation.StationID = idBase;
            //baseStation.Name = nameBase;
            //obj.Add_BaseStation(baseStation);
            //obj.Display(Display.BaseStation);









        }
    }
}
