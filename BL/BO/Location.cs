﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Location
    {
        public double Longtitude { get; set; }
        public double Latitude { get; set; }

        public override string ToString()
        {
            return string.Format("{0}/{1}", Longtitude, Latitude);
        }
    }
}
