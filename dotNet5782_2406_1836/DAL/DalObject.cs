using DAL;
using DAL.DalObject;
using DalObject.DO;
using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DAL.DalObject.DataSource;

namespace DalObject
{
    public class DalObject
    {
        public DalObject() { DataSource.Initialize(); }

        /// <summary>
        /// ////////////////////////////////////////////add funcrions
        /// </summary>
        public static void Add_Station()
        {
            Station st = new Station();
            int num3,num4;
            double num1, num2;
            //Console.WriteLine("Please enter ID station: ");
            //int.TryParse(Console.ReadLine(), out num3);
            //st.StationID = num3;
            Console.WriteLine("Please enter name station: ");
            st.Name= Console.ReadLine();
            Console.WriteLine("Please enter Longtitude: ");
            double.TryParse(Console.ReadLine(), out num1);
            st.Longtitude = num1;
            Console.WriteLine("Please enter Latitude: ");
            double.TryParse(Console.ReadLine(), out num2);
            st.Latitude = num2;
            Console.WriteLine("Please enter number of ChargeSlots: ");
            int.TryParse(Console.ReadLine(), out num4);
            st.ChargeSlots = num4;
            Stations[config.Index_Station++] = new Station { Name= st.Name , StationID = config.RunIdStation++ };
        }
        public static void Add_Drone()
        {
            Drone dr = new Drone();
            int num_DroneID, num_weight, num_DroneStatus;
            double num_battary;

            Console.WriteLine("Please enter Drone ID : ");
            int.TryParse(Console.ReadLine(), out num_DroneID);
            dr.DroneID = num_DroneID;
            Console.WriteLine("Please enter drone model: ");
            dr.Drone_Model = Console.ReadLine();
            Console.WriteLine("Please enter Drone weight: ");
            int.TryParse(Console.ReadLine(), out num_weight);
            dr.Drone_weight = (WeightCategories)num_weight;
            Console.WriteLine("Please enter Drone status: ");
            int.TryParse(Console.ReadLine(), out num_DroneStatus);
            dr.status = (DroneStatus)num_DroneStatus;
            Console.WriteLine("Please enter Drone battary: ");
            double.TryParse(Console.ReadLine(), out num_battary);
            dr.Battary = num_battary;
            Drones[config.Index_Drone++] = new Drone { DroneID = dr.DroneID , StationID = config.RunIdStation++ };
        }

        //להוסיף תצוגת רחפן
        public static void Add_Customer()
        {
            Customer cs = new Customer();
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
        /// ////////////////////////////////////////////////////////////objects display
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        /// 
        
        

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
        public static Customer display_Customerlist(Customer[] cs_arr)
        { }
        public static Parcel display_Customerlist(Parcel[] pc_arr)
        { }
        public static Station display_Customerlist(Station[] st_arr)
        { }
    }
}
