﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel.DataAnnotations;
using System.Collections.Specialized;

using BO;
using BlApi;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace PLGui
{
    /// <summary>
    /// Interaction logic for AddDroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        public IBL bL;
        private Drone drone;
        private DroneToList droneToList;
        BaseStation BaseStation;
        private int StationID;
        public int Idrone;
        private MainWindow mainWindow;

        /// <summary>
        /// Constructor for add drone
        /// </summary>
        /// <param name="getBL"></param>
        /// <param name="_mainWindow"></param>
        public DroneWindow(IBL getBL, MainWindow _mainWindow)
        {
            InitializeComponent();
            bL = getBL;
            drone = new Drone();
            DataContext = drone;
            DroneSituateGrid.Visibility = Visibility.Hidden;
            ItemsSourceStations();
            mainWindow = _mainWindow;
            BaseStation = new();
            lvStations.ItemsSource = bL.GetBasetationToListsByPredicate(b => b.AvailableChargingStations > 0);
            cbxDroneWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            //StatusDrone.SelectedItem = Enum.GetValues(typeof(DroneStatus));
        }

        /// <summary>
        /// Constructor for update drone
        /// </summary>
        /// <param name="_bL"></param>
        /// <param name="_ShowDronesWindow"></param>
        /// <param name="drone"></param>
        /// <param name="_Idrone"></param>
        public DroneWindow(IBL _bL, MainWindow _mainWindow, DroneToList _drone, int _Idrone)
        {
            InitializeComponent();
            Idrone = _Idrone;
            bL = _bL;
            droneToList = _drone;
            drone = _bL.GetDrone(_drone.DroneID);
            DataContext = drone;
            mainWindow = _mainWindow;
            DroneSituateGrid.Visibility = Visibility.Visible;
            ItemsSourceStations();
            DroneVisibility();
        }

        private void ItemsSourceStations()
        {
            lvStations.ItemsSource = bL.GetBasetationToListsByPredicate();
        }
        public void DroneVisibility()
        {
            if (drone.Status == DroneStatus.available)
            {
                NoFindParcel.Visibility = Visibility.Visible;
                droneMaintenance.Visibility = Visibility.Hidden;
                FindParcel.Visibility = Visibility.Hidden;
                ChargeAndCollect.Visibility = Visibility.Visible;
                CollectParcel.Visibility = Visibility.Visible;

                // for inside of the grid collect parcel//
                btnCollectParcel.Visibility = Visibility.Visible;

                packageAssociated.Text = "הרחפן זמין";
            }
            else if (drone.Status == DroneStatus.maintenance)
            {
                DetailsStation.Visibility = Visibility.Visible;
                ChargeAndCollect.Visibility = Visibility.Hidden;
                CollectParcel.Visibility = Visibility.Hidden;
                droneMaintenance.Visibility = Visibility.Visible;
            }

            else if (drone.Status == DroneStatus.busy)
            {
                FindParcel.Visibility = Visibility.Visible;
                NoFindParcel.Visibility = Visibility.Hidden;
                DetailsStation.Visibility = Visibility.Hidden;
                ChargeAndCollect.Visibility = Visibility.Hidden;
                CollectParcel.Visibility = Visibility.Visible;

                if (drone.ParcelInDeliverd.StatusParcrlInDeliver == StatusParcrlInDeliver.AwaitingCollection)
                {

                    //btnCollectParcel.Visibility = Visibility.Visible;
                    btnCollectParcel.Content = "איסוף חבילה מלקוח";
                }
                else if (drone.ParcelInDeliverd.StatusParcrlInDeliver == StatusParcrlInDeliver.OnTheWayDestination)
                {
                    //btnCollectParcel.Visibility = Visibility.Visible;
                    btnCollectParcel.Content = "אספקת חבילה ללקוח";
                    NoFindParcel.Visibility = Visibility.Hidden;
                    FindParcel.Visibility = Visibility.Visible;
                }
            }
            else
            {
                NoFindParcel.Visibility = Visibility.Hidden;
                FindParcel.Visibility = Visibility.Hidden;
                CollectParcel.Visibility = Visibility.Hidden;

            }
        }
        private void refreshThisWindow()
        {

            mainWindow.lstDroneListView.Items.Refresh();
            new DroneWindow(bL, mainWindow, droneToList, Idrone).Show();
            this.Close();

        }
        private void DroneId_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.IsEnabled = true;
            Close();
        }

        private void btnUpdateModel_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה רוצה לעדכן את שם הרחפן?",
                "הכנס שם רחפן", MessageBoxButton.YesNoCancel);

            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    bL.UpdateDrone(drone.DroneID, drone.DroneModel);
                    droneToList.DroneModel = drone.DroneModel;
                    mainWindow.dronesToLists[Idrone] = droneToList;
                    mainWindow.lstDroneListView.Items.Refresh();

                    MessageBox.Show("הרחפן עודכן בהצלחה");
                    Close();
                    break;
                case MessageBoxResult.Cancel:
                    Close();
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }
        private void SendDroneToCharge(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!bL.SendDroneToCharge(drone.DroneID))
                {
                    MessageBox.Show("שליחה לטעינה נכשלה", "אישור");
                }
                else
                {
                    refreshThisWindow();
                    DataContext = drone;
                }
            }

            catch (BO.ChargExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                                  MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.CheckIdException ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.CheckIfIdNotException ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
        }


        private void _btnRealeseDroneClick(object sender, RoutedEventArgs e)
        {
            if (drone.Status == DroneStatus.maintenance)
            {
                bL.ReleaseDroneFromCharge(drone.DroneID);
                refreshThisWindow();
            }
            else
            {
                MessageBox.Show("הרחפן לא במצב תחזוקה", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private void NumberOfStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    BaseStationToList stations = (BaseStationToList)NumberOfStations.SelectedItem;
        //    StationID = stations.ID;
        //}

        private void lvStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BaseStationToList stations = (BaseStationToList)lvStations.SelectedItem;
            StationID = stations.ID;
        }

        private void btnSendDroneToDeliver_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לשייך חבילה ", "אישור", MessageBoxButton.OKCancel);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    bool test = bL.AssignmentOfPackageToDrone(drone.DroneID);
                    if (test)
                    {
                        FindParcel.Visibility = Visibility.Hidden;
                        NoFindParcel.Visibility = Visibility.Visible;
                        drone = bL.GetDrone(drone.DroneID);
                        drone.ParcelInDeliverd.StatusParcrlInDeliver = StatusParcrlInDeliver.AwaitingCollection;
                        refreshThisWindow();
                    }
                    else
                    {
                        MessageBox.Show("לא נמצאה חבילה מתאימה", "אישור");
                    }
                }
            }
            catch (BO.CheckIfIdNotException ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.CheckIdException ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.ParcelAssociationExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
        }

        private void btnCollectParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnCollectParcel.Content == "איסוף חבילה מלקוח")
                {
                    if (!bL.CollectParcelByDrone(drone.DroneID))
                    {
                        MessageBox.Show("הפעולה נכשלה", "אישור");
                        
                    }
                    else
                    {
                        drone = bL.GetDrone(drone.DroneID);
                        drone.ParcelInDeliverd.StatusParcrlInDeliver = StatusParcrlInDeliver.OnTheWayDestination;
                        DataContext = drone;
                        refreshThisWindow();
                        mainWindow.lstDroneListView.Items.Refresh();
                    }
                }
                
                else if (btnCollectParcel.Content == "אספקת חבילה ללקוח")
                {
                    if (!bL.DeliveryParcelToCustomer(drone.DroneID))
                    {
                        MessageBox.Show("הפעולה נכשלה", "אישור");
                    }
                    else
                    {
                        NoFindParcel.Visibility = Visibility.Visible;
                        drone = bL.GetDrone(drone.DroneID);
                        MessageBox.Show("החבילה סופקה ללקוח","אישור");
                        mainWindow.lstDroneListView.Items.Refresh();
                    }
                }
            }
            catch (BO.CheckIfIdNotException ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.CheckIdException ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.ParcelAssociationExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה בטוח שאתה רוצה להוסיף את הרחפן?"
               , "הכנס רחפן", MessageBoxButton.OKCancel);

            if (StationID != default)
            {
                switch (messageBoxResult)
                {
                    case MessageBoxResult.OK:
                        try
                        {
                            bL.AddNewDrone(drone, StationID);

                            mainWindow.dronesToLists.Add(bL.GetDroneToListsBLByPredicate()
                                .First(i => i.DroneID == drone.DroneID));
                            mainWindow.cmbStatusSelectorAndcmbWeightSelector();

                            MessageBox.Show("הרחפן נוסף בהצלחה");
                            Close();

                        }
                        catch (BO.CheckIdException ex)
                        {
                            MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                                MessageBoxResult.None, MessageBoxOptions.RightAlign);
                            Close();
                        }
                        break;

                    case MessageBoxResult.Cancel:
                        Close();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("משתמש יקר אנא בחר תחנה", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private void DroneToListsView_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    StatusSelectorAndWeightSelector();
        //}

        //private void StatusSelectorAndWeightSelector()
        //{
        //    if (WeightCategories.SelectedIndex == -1)
        //    {
        //        WeightSelector.SelectedIndex = 0;
        //    }
        //    WeightCategories weightCategories = (WeightCategories)WeightSelector.SelectedItem;
        //    DroneStatuses droneStatuses = (DroneStatuses)StatusSelector.SelectedItem;

        //    if (weightCategories == WeightCategories.All && droneStatuses == DroneStatuses.All)
        //        DroneListView.ItemsSource = droneToListsView;

        //    else if (weightCategories != WeightCategories.All && droneStatuses == DroneStatuses.All)
        //        DroneListView.ItemsSource = droneToListsView.ToList().FindAll(i => i.MaxWeight == (BO.WeightCategories)weightCategories);

        //    else if (weightCategories == WeightCategories.All && droneStatuses != DroneStatuses.All)
        //        DroneListView.ItemsSource = droneToListsView.ToList().FindAll(i => i.DroneStatuses == (BO.DroneStatuses)droneStatuses);

        //    else
        //        DroneListView.ItemsSource = droneToListsView.ToList().FindAll(i => i.MaxWeight == (BO.WeightCategories)weightCategories && i.DroneStatuses == (BO.DroneStatuses)droneStatuses);
        //    AddGrouping();
        //}

    }
}


