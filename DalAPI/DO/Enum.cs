using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public enum Display { BaseStation = 1, Drons = 2, Customers = 3, Parcels = 4, EXIT = 0 }
    public enum WeightCategories { 
        light = 1,
        medium,
        heavy
    }

    public enum Priorities { 
        regular =1, 
        fast, 
        emergency 
    }

    public enum Options { 
        ADD = 0,
        UPDATE, 
        DISPLAY, 
        LIST_VIEW, 
        EXIT = 0 
    }

}
