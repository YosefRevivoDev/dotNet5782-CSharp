using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public enum DroneStatus { available = 1, maintenance, busy }
    public enum WeightCategories { light = 1, medium, heavy }
    public enum Priorities { regular = 1, fast, emergency }
    public enum ParcelStatus { Defined = 1, associated, collected, provided }
    public enum StatusParcrlInDeliver { AwaitingCollection, OnTheWayDestination } // ממתין לאיסוף, בדרך ליעד
    public enum SenderOrTarget { Sender, Target }

}
