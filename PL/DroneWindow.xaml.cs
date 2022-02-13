using System;
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
        BaseStation baseStation;
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
            baseStation = new();
            lvStations.ItemsSource = bL.GetBasetationToListsByPredicate(b => b.AvailableChargingStations > 0);
            cbxDroneWeight.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
        }

        /// <summary>
        /// Constructor for update drone
        /// </summary>
        /// <param name="_bL"></param>
        /// <param name="drone"></param>
        /// <param name="_Idrone"></param>
        public DroneWindow(IBL _bL, MainWindow _mainWindow , int _Idrone = 0)
        {
            InitializeComponent();
            Idrone = _Idrone;
            bL = _bL;
            mainWindow = _mainWindow;
            droneToList = bL.GetDroneToListsBLByPredicate(x => x.DroneID == _Idrone).FirstOrDefault();
            drone = _bL.GetDrone(_Idrone);
            DataContext = drone;
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
            if (drone.Status == BO.DroneStatus.available)
            {
                NoFindParcel.Visibility = Visibility.Visible;
                droneMaintenance.Visibility = Visibility.Hidden;
                FindParcel.Visibility = Visibility.Hidden;
                ChargeAndCollect.Visibility = Visibility.Visible;
                CollectParcel.Visibility = Visibility.Hidden;

                // for inside of the grid collect parcel//
                btnCollectParcel.Visibility = Visibility.Visible;

                packageAssociated.Text = "הרחפן זמין";
            }
            else if (drone.Status == BO.DroneStatus.maintenance)
            {
                DetailsStation.Visibility = Visibility.Visible;
                ChargeAndCollect.Visibility = Visibility.Hidden;
                CollectParcel.Visibility = Visibility.Hidden;
                droneMaintenance.Visibility = Visibility.Visible;
                NoFindParcel.Visibility = Visibility.Visible;
                FindParcel.Visibility = Visibility.Hidden;
                btnSendDroneToDeliver.Visibility = Visibility.Hidden;
                packageAssociated.Visibility = Visibility.Hidden;
                drone = bL.GetDrone(droneToList.DroneID);
                baseStation = bL.GetBaseStation(bL.GetTheIdOfCloseStation(drone.DroneID));
                txbNameStation.Text = baseStation.ID.ToString();
                txbLocationStation.Text = baseStation.location.ToString();
            }

            else if (drone.Status == BO.DroneStatus.busy)
            {
                FindParcel.Visibility = Visibility.Visible;
                NoFindParcel.Visibility = Visibility.Hidden;
                DetailsStation.Visibility = Visibility.Hidden;
                ChargeAndCollect.Visibility = Visibility.Hidden;
                //CollectParcel.Visibility = Visibility.Visible;
                btnCollectParcel.Visibility = Visibility.Visible;

                if (drone.ParcelInDeliverd.StatusParcrlInDeliver == StatusParcrlInDeliver.AwaitingCollection)
                {
                    btnCollectParcel.Visibility = Visibility.Visible;
                    btnSuplyParcel.Visibility = Visibility.Hidden;
                }
                if (drone.ParcelInDeliverd.StatusParcrlInDeliver == StatusParcrlInDeliver.OnTheWayDestination)
                {
                    btnSuplyParcel.Visibility = Visibility.Hidden;
                    btnCollectParcel.Visibility = Visibility.Hidden;
                    NoFindParcel.Visibility = Visibility.Visible;
                    FindParcel.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                NoFindParcel.Visibility = Visibility.Hidden;
                FindParcel.Visibility = Visibility.Hidden;
                CollectParcel.Visibility = Visibility.Hidden;

            }
        }

        private void refreshMainWindow()
        {
            if (mainWindow != null)
            {
                new MainWindow();
                mainWindow.lstDroneListView.Items.Refresh();
                mainWindow.GroupingDroneList();
            }
        }

        private void refreshThisWindow()
        {

            new DroneWindow(bL, mainWindow, Idrone).Show();
            mainWindow.lstDroneListView.Items.Refresh();
            this.Close();

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
                    mainWindow.GroupingDroneList();
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
            catch (BO.CheckIfIdNotExceptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
        }

        private void _btnRealeseDroneClick(object sender, RoutedEventArgs e)
        {
            if (drone.Status == BO.DroneStatus.maintenance)
            {
                bL.ReleaseDroneFromCharge(drone.DroneID);
                mainWindow.GroupingDroneList();
                refreshThisWindow();
            }
            else
            {
                MessageBox.Show("הרחפן לא במצב תחזוקה", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

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
            catch (BO.CheckIfIdNotExceptions ex)
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
                if (!bL.CollectParcelByDrone(drone.DroneID))
                {
                    MessageBox.Show("איסוף חבילה נכשל", "אישור");

                }
                else
                {
                    refreshMainWindow();

                    drone = bL.GetDrone(drone.DroneID);
                    drone.ParcelInDeliverd.StatusParcrlInDeliver = StatusParcrlInDeliver.OnTheWayDestination;
                    btnSuplyParcel.Visibility = Visibility.Visible;
                    DataContext = drone;
                }
               
            }
            catch (BO.CheckIfIdNotExceptions ex)
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

        private void btnSuplyParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!bL.DeliveryParcelToCustomer(drone.DroneID))
                {
                    MessageBox.Show("איסוף חבילה נכשל", "אישור");
                }
                else
                {
                    drone = bL.GetDrone(drone.DroneID);
                    drone.Status = BO.DroneStatus.available;
                    DataContext = drone;
                }

            }
            catch (BO.CheckIfIdNotExceptions ex)
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

            if (drone.DroneID != default && drone.DroneModel != default && drone.DroneWeight != default && StationID != default)
            {
                switch (messageBoxResult)
                {
                    case MessageBoxResult.OK:
                        try
                        {
                            bL.AddNewDrone(drone, StationID);

                            mainWindow.dronesToLists.Add(bL.GetDroneToListsBLByPredicate(i => i.DroneID == drone.DroneID).First());

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
                MessageBox.Show("אנא מלא את כל הפרטים", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
                mainWindow.lstDroneListView.Items.Refresh();
                mainWindow.GroupingDroneList();
                Close();
          
        }
    }
}


