using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public enum Display { BaseStation = 1, Drons = 2, Customers = 3, Parcels = 4, EXIT = 0 }
        public enum WeightCategories { light, medium, heavy }
        public enum Priorities { regular, fast, emergency }
        //public enum DroneStatus { available, maintenance, busy}
        public enum Options {ADD = 1, UPDATE = 2 , DISPLAY = 3, LIST_VIEW = 4, EXIT = 0 }
        
    }
}
