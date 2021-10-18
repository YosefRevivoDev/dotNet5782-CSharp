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
                Console.WriteLine("Select your choice: ");
                bool correct = Enum.TryParse(Console.ReadLine(), out choice);
                if (!correct)
                {
                    continue;
                }
                switch (choice)
                {
                    //choose 0 
                    case CHOICE.ADD:
                        //TimeSpan span = DateTime.Now - Applicationstarted;

                        break;
                    case CHOICE.UPDATE:
                        break;
                    case CHOICE.DISPLAY:
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
