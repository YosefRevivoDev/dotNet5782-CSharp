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

namespace PLGui
{
    /// <summary>
    /// Interaction logic for AddDroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        public IBL bL;
        BackgroundWorker backgroundWorker;
        private Drone drone { set; get; }
        private DroneToList droneToList;
        private ParcelInDeliver parcelInDeliverd;
        private BaseStationToList baseStationToList { set; get; }
        public int Idrone;
        private MainWindow mainWindow;
        private int StationID;



        /// <summary>
        /// Constructor for add drone
        /// </summary>
        /// <param name="getBl"></param>
        /// <param name="_showDronesWindow"></param>
        public DroneWindow(IBL getBL, MainWindow _mainWindow)
        {
            InitializeComponent();
            bL = getBL;
            drone = new Drone();
            DataContext = drone;
            DroneSituateGrid.Visibility = Visibility.Collapsed;
            droneGrid.VerticalAlignment = VerticalAlignment.Center;
            droneGrid.HorizontalAlignment = HorizontalAlignment.Center;
            ItemsSourceStations();
            mainWindow = _mainWindow;
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
        public DroneWindow(IBL _bL, MainWindow _mainWindow, DroneToList drone, int _Idrone)
        {
            InitializeComponent();
            DroneSituateGrid.Visibility = Visibility.Visible;
            Idrone = _Idrone;
            this.bL = _bL;
            this.drone = _bL.GetDrone(drone.DroneID);
            mainWindow = _mainWindow;
            DataContext = this.drone;
            ItemsSourceStations();
            droneToList = drone;
        }

        private void ItemsSourceStations()
        {
            lvStations.ItemsSource = bL.GetBasetationToLists();
        }

        private void refreshThisWindow()
        {

            if (mainWindow != null)
            {
                mainWindow.lstDroneListView.Items.Refresh();
                // mainWindow.AddGrouping();
            }
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

                    MessageBox.Show(drone.ToString(), "הרחפן עודכן בהצלחה");
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
                    // UpdatingWindow(drone.Id);
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
                bL.ReleaseDroneFromCharge(drone.DroneID, baseStationToList.ID, DateTime.Now);
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

        private void btnSendDroneToDeliver_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לשייך חבילה ", "אישור", MessageBoxButton.OKCancel);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.OK:
                        bool test = bL.AssignmentOfPackageToDrone(drone.DroneID);
                        if (test)
                        {
                            //NoParcel.Visibility = Visibility.Hidden;
                            //YesParcel.Visibility = Visibility.Visible;
                            drone = bL.GetDrone(drone.DroneID);
                            refreshThisWindow();
                            //UpdatingWindow(drone.Id);
                        }
                        else
                        {
                            MessageBox.Show("לא נמצאה חבילה מתאימה", "אישור");
                        }
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                    default:
                        break;
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

            bL.CollectParcelByDrone(drone.DroneID);

            try
            {
                MessageBoxResult result = MessageBox.Show("נכנסת בהצלחה", "מידע", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {

                    case MessageBoxResult.OK:

                        bL.CollectParcelByDrone(drone.DroneID);
                        drone = bL.GetDrone(drone.DroneID);
                        DataContext = drone;
                        mainWindow.lstDroneListView.Items.Refresh();
                        btnCollectParcel.Visibility = Visibility.Hidden;

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "לא נמצא רחפן", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה בטוח שאתה רוצה להוסיף את הרחפן?"
               , "הכנס רחפן", MessageBoxButton.YesNoCancel);

            if (baseStationToList != null)
            {
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            bL.AddNewDrone(drone, bL.GetDroneToListsBLByPredicate().ToList().Count);
                            mainWindow.dronesToLists.Add(bL.GetDroneToListsBLByPredicate()
                                .First(i => i.DroneID == drone.DroneID));
                            //mainWindow.dronesToList.CollectionChanged += mainWindow.OnCollectionChanged;
                            lvStations.ItemsSource = bL.GetDroneToListsBLByPredicate(i => i. > 0);

                            MessageBox.Show(drone.ToString(), "הרחפן נוסף בהצלחה");
                            Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
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

            else
            {
                MessageBox.Show("משתמש יקר אנא בחר תחנה", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lvStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            baseStationToList = (BO.BaseStationToList)lvStations.SelectedItem;
            StationID = baseStationToList.ID;

        }
    }
}