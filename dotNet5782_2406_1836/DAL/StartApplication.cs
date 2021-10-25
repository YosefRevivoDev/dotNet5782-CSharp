using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    public class StartApplication
    {
        public void start()
        {
 
            CHOICE choice;
            DateTime Applicationstarted = DateTime.Now;
            do
            {
                Console.WriteLine("Select your choice:\n" + "1 - ADD" +"2 - UPDATE" + "3 - DISPLAY" +
                    "4 - VIEW LISTS" + " 0 - EXIT");
                bool correct = Enum.TryParse(Console.ReadLine(), out choice);
                if (!correct)
                {
                    continue;
                }
                switch (choice)
                {
                    //choose 0 
                    case CHOICE.ADD:
                        {
                            int num4;
                            double num1, num2;
                            string name;
                            //Console.WriteLine("Please enter ID station: ");
                            //int.TryParse(Console.ReadLine(), out num3);
                            //st.StationID = num3;
                            Console.WriteLine("Please enter name station: ");
                            name = Console.ReadLine();
                            Console.WriteLine("Please enter Longtitude: ");
                            double.TryParse(Console.ReadLine(), out num1);
                           
                            Console.WriteLine("Please enter Latitude: ");
                            double.TryParse(Console.ReadLine(), out num2);
                         
                            Console.WriteLine("Please enter number of ChargeSlots: ");
                            int.TryParse(Console.ReadLine(), out num4);

                            DalObject.DalObject.Add_Station(name, num1, num2, num4);

                            //Add_Station();
                            //TimeSpan span = DateTime.Now - Applicationstarted;
                        }
                        break;
                    case CHOICE.UPDATE:
                        break;
                    case CHOICE.DISPLAY:
                        //להפעיל פונקציות הדפסת תצוגת אובייקטים 
                        int stationId;
                        Console.WriteLine("Please enter station ID: ");
                        int.TryParse(Console.ReadLine(), out stationId);
                        Station station = DalObject.DalObject.GetStation(stationId);
                        Console.WriteLine(string.Format("Station Details: {0}", station));
                        break;
                    case CHOICE.VIEW_LISTS:
                        Station[] stations = DalObject.DalObject.GetStations();

                        Console.WriteLine("All Station Data:");

                        foreach(Station s in stations)
                        {
                            Console.WriteLine(s);
                        }
                        break;
                    case CHOICE.EXIT:
                        break;
                    default:
                        break;
                }
            } while (choice != CHOICE.EXIT);
        }
    }
}
