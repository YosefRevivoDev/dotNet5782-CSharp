using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;

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
                            //Add_Station();
                            //TimeSpan span = DateTime.Now - Applicationstarted;
                        }
                        break;
                    case CHOICE.UPDATE:
                        break;
                    case CHOICE.DISPLAY:
                        //להפעיל פונקציות הדפסת תצוגת אובייקטים 
                        break;
                    case CHOICE.VIEW_LISTS:
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
