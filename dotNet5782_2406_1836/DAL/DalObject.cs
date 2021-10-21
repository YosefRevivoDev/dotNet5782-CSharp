using DAL;
using DAL.DalObject;
using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DalObject
    {
        //  DataSource.Initialize();//אתחול
        public DalObject() { DataSource.Initialize(); }

        /// <summary>
        /// ////////////////////////////////////////////add funcrions
        /// </summary>
        public static void Add_Station()
        {
            Station st = new Station();
            int num; 
            Console.WriteLine("Please enter ID station: ");
            int.TryParse(Console.ReadLine(), out num);
            st.StationID = num;
            Console.WriteLine("Please enter model station: ");
            st.Model= Console.ReadLine();
        }
        public static void Add_Drone()
        {
            Drone dr = new Drone();
            int num;
            Console.WriteLine("Please enter Drone ID : ");
            int.TryParse(Console.ReadLine(), out num);
            dr.DroneID = num;
            Console.WriteLine("Please enter drone model: ");
            dr.Drone_Model = Console.ReadLine();
            Console.WriteLine("Please enter model Drone weight: ");
          //  dr.Drone_weight = SetWeight(Weight);
          //להוסיף את התכונות שנותרו
        }
        //להוסיף תצוגת רחפן
        public static void Add_Customer()
        {
            customer cs = new customer();
            int num;
            double d1, d2;
            Console.WriteLine("Please enter ID: ");
            int.TryParse(Console.ReadLine(), out num);
            cs.Id = num;
            Console.WriteLine("Please enter name: ");
            cs.Name = Console.ReadLine();
            Console.WriteLine("Please enter phone number: ");
            cs.Phone = Console.ReadLine();
            Console.WriteLine("Please enter Longtitude: ");
            double.TryParse(Console.ReadLine(), out d1);
            cs.Longtitude = d1;
            Console.WriteLine("Please enter Latitude: ");
            double.TryParse(Console.ReadLine(), out d2);
            cs.Latitude = d2;
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////object display
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        /// 
        
        public static customer Display_Customer(customer cs)//לבדוק למה זה אדום
        {
            Console.WriteLine("Please enter ID: "+ cs.Id);
            Console.WriteLine("Please enter name: " + cs.Name);
            Console.WriteLine("Please enter phone number: " + cs.Phone);
            Console.WriteLine("Please enter Longtitude: " + cs.Longtitude);
            Console.WriteLine("Please enter Latitude: " + cs.Latitude);
        }
        public static Drone display_Drone(Drone dr)//לבדוק למה זה אדום
        {

        }

        public static Parcel parcel_display(Parcel pc)//לבדוק למה זה אדום
        {

        }
        public static Station station_display(Station st)//לבדוק למה זה אדום
        {

        }


        public static void SetWeight(WeightCategories Weight)//לבדוק איך עושים השמה למשקל - enum 
        {
            switch (Weight)
            {
                case WeightCategories.Easy:
                    return;
                 case WeightCategories.Medium:
                    return;
                case WeightCategories.Heavy:
                    return;
            }
        }
        ////////////////////////////////////////////////////////lists display
        ///
        public static Drone display_Dronelist(Drone[] dr_arr)
        {
            for (int i = 0; i < Index_Drone; i++)
            {
                display_Drone(Drone[i]);
            }
        }
        public static customer display_Customerlist(customer[] cs_arr)
        { }
        public static Parcel display_Customerlist(Parcel[] pc_arr)
        { }
        public static Station display_Customerlist(Station[] st_arr)
        { }
    }
}
