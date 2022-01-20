using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public enum DroneStatus { available, maintenance, busy }
    public enum WeightCategories { light, medium, heavy }
    public enum Priorities { regular, fast, emergency }
    public enum ParcelStatus { Defined, associated, collected, provided }
    public enum StatusParcrlInDeliver { AwaitingCollection, OnTheWayDestination } // ממתין לאיסוף, בדרך ליעד
    public enum SenderOrTarget { Sender, Target }

}
