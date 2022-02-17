using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BO;
using System.Threading.Tasks;
using System.Threading;
using static BL.BL;


namespace BL
{
    internal class Simulator
    {
        enum Mintance { Starting, Going, charging };
        private const double VELOCITY = 0.1;
        private const int DELAY = 1000;
        private const double TIME_STOP = DELAY / 1000.0;
        private const double STOP = VELOCITY / TIME_STOP;

        BL BL;
        Drone drone;
        DroneToList droneToList;
        Action actionInSimulator;
        Customer sander;
        Customer target;
        double disLocationsX;
        double disLocationsY;
        int ChargeForKm;

        internal Simulator(BL bL, int droneId, Action ProgressSimulator, Func<bool> RunSimulator)
        {
            BL = bL;
            droneToList = BL.GetDroneToListsBLByPredicate(i => i.DroneID == droneId).First();
            drone = BL.GetDrone(droneId);

            actionInSimulator = ProgressSimulator;

            while (!RunSimulator())
            {
                switch (droneToList.Status)
                {
                    case DroneStatus.available:
                        try
                        {
                            lock (BL)
                            {
                                BL.AssignmentOfPackageToDrone(droneId);
                            }
                            actionInSimulator();
                            Thread.Sleep(DELAY * 2);
                        }
                        catch (Exception)
                        {
                            if (droneToList.BattaryStatus != 100)
                            {
                                try
                                {
                                    lock (BL)
                                    {
                                        BL.SendDroneToCharge(droneId);
                                    }
                                    actionInSimulator();
                                    Thread.Sleep(DELAY * 2);
                                }
                                catch (Exception)
                                {
                                    //If the drone can not return to the station because there is no place
                                    //release a random drone from a charger to bring it home.
                                    lock (BL)
                                    {
                                        BaseStationToList stationToList = BL.GetBasetationToListsByPredicate(i => i.ID == BL.GetTheIdOfCloseStation(droneId)).First();
                                        BaseStation station = BL.GetBaseStation(stationToList.ID);
                                        BL.ReleaseDroneFromCharge(station.droneCharges.First().DroneID);
                                        BL.SendDroneToCharge(droneId);
                                        actionInSimulator();
                                        Thread.Sleep(DELAY * 2);
                                    }
                                }
                            }
                            else
                            {
                                Thread.Sleep(DELAY * 2);
                            }
                        }
                        break;
                    case DroneStatus.maintenance:
                        {

                            while (droneToList.BattaryStatus < 100)
                            {
                                if (droneToList.BattaryStatus + 1.0 < 100)
                                    droneToList.BattaryStatus += 1.0;
                                else
                                    droneToList.BattaryStatus = 100.0;

                                Thread.Sleep(DELAY / 3);
                                actionInSimulator();
                            }
                            lock (BL)
                            {
                                BL.ReleaseDroneFromCharge(droneId);
                                actionInSimulator();
                            }
                            Thread.Sleep(DELAY * 2);
                        }
                        break;
                    case DroneStatus.busy:
                        {
                            drone = BL.GetDrone(droneId);
                            sander = BL.GetCustomer(drone.ParcelInDeliverd.Sender.CustomerId);
                            target = BL.GetCustomer(drone.ParcelInDeliverd.Target.CustomerId);

                            if (drone.ParcelInDeliverd.StatusParcrlInDeliver == StatusParcrlInDeliver.AwaitingCollection)
                            {
                                locationDinamic(sander, PowerConsumptionAvailable);
                                lock (BL)
                                {
                                    drone.BattaryStatus += drone.ParcelInDeliverd.TransportDistance * PowerConsumptionAvailable;
                                    BL.CollectParcelByDrone(droneId);
                                    actionInSimulator();
                                }
                                Thread.Sleep(DELAY * 2);
                            }
                            else
                            {
                                double weight = PowerConsumptionLightWeight;
                                switch (drone.ParcelInDeliverd.WeightCategories)
                                {
                                    case WeightCategories.light:
                                        weight = PowerConsumptionLightWeight;
                                        break;
                                    case WeightCategories.medium:
                                        weight = PowerConsumptionMediumWeight;
                                        break;
                                    case WeightCategories.heavy:
                                        weight = PowerConsumptionHeavyWeight;
                                        break;
                                    default:
                                        break;
                                }
                                locationDinamic(target, weight);
                                lock (BL)
                                {
                                    drone.BattaryStatus += drone.ParcelInDeliverd.TransportDistance * weight;
                                    BL.DeliveryParcelToCustomer(droneId);
                                    actionInSimulator();
                                }
                                Thread.Sleep(DELAY * 2);
                            }
                        }
                        break;
                }
            }
        }

        private void locationDinamic(Customer customer, double weight)
        {
            ChargeForKm = (int)drone.ParcelInDeliverd.TransportDistance;
            disLocationsX = customer.LocationCustomer.Longtitude - drone.CurrentLocation.Longtitude;
            disLocationsY = customer.LocationCustomer.Latitude - drone.CurrentLocation.Latitude;
            for (int i = ChargeForKm; i > 0; i--)
            {
                droneToList.CurrentLocation.Longtitude += disLocationsX / ChargeForKm;
                droneToList.CurrentLocation.Latitude += disLocationsY / ChargeForKm;
                droneToList.BattaryStatus -= weight;
                actionInSimulator();
                Thread.Sleep(DELAY);
            }
        }
    }
}
