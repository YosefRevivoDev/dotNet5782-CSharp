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


namespace PLGui
{
    /// <summary>
    /// Interaction logic for AddDroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private Drone drone { set; get; }
        public BL.BL bL;
        private DroneToList droneToList;
        private ParcelInDeliver parcelInDeliverd;
        private BaseStationToList baseStationToList { set; get; }
        public NotifyCollectionChangedEventHandler OnCollectionChanged { get; private set; }

        /// <summary>
        /// Constructor for add drone
        /// </summary>
        /// <param name="getBl"></param>
        /// <param name="_showDronesWindow"></param>
        public DroneWindow(BL.BL getBl, MainWindow _mainWindow)
        {
            InitializeComponent();
            bL = getBl;
            drone = new Drone();
            DataContext = drone;
            DroneSituateGrid.Visibility = Visibility.Collapsed;
            droneGrid.VerticalAlignment = VerticalAlignment.Center;
            droneGrid.HorizontalAlignment = HorizontalAlignment.Center;
            ItemsSourceStations();
            mainWindow = _mainWindow;
            DroneWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StatusDrone.SelectedItem = Enum.GetValues(typeof(DroneStatus));
        }


        public int Idrone;
        private MainWindow mainWindow;

        /// <summary>
        /// Constructor for update drone
        /// </summary>
        /// <param name="_bL"></param>
        /// <param name="_ShowDronesWindow"></param>
        /// <param name="drone"></param>
        /// <param name="_Idrone"></param>
        public DroneWindow(BL.BL _bL, MainWindow _mainWindow, DroneToList drone, int _Idrone)
        {
            InitializeComponent();
            DroneSituateGrid.Visibility = Visibility.Visible;
            Idrone = _Idrone;
            this.bL = _bL;
            this.drone = _bL.GetDrone(drone.DroneID);
            mainWindow = _mainWindow;
            DataContext = this.drone;
            UpdateGridVisibility();
            ItemsSourceStations();
            droneToList = drone;
        }

        private void ItemsSourceStations()
        {
            NumberOfStations.ItemsSource = bL.GetBasetationToLists();
        }

        private void UpdateGridVisibility()
        {
            DroneId.IsReadOnly = true;
            DroneModel.IsReadOnly = true;
            DroneWeight.IsReadOnly = true;
            /*  NumberOfStations.Visibility = Visibility.Hidden;
              NumberOfStation.Visibility = Visibility.Hidden;*/
            addDrone.Visibility = Visibility.Hidden;
        }

        private void addDrone_Click(object sender, RoutedEventArgs e)
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
            if (drone.CurrentLocation != baseStationToList.location && drone.Status == DroneStatus.available && baseStationToList != null)
            {
                bL.SendDroneToCharge(drone.DroneID, baseStationToList.ID);
                drone = bL.GetDrone(drone.DroneID);
                DataContext = drone;
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

        private void NumberOfStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            baseStationToList = (BaseStationToList)NumberOfStations.SelectedItem;
        }

        private void btnSendDroneToDeliver_Click(object sender, RoutedEventArgs e)
        {
            if (drone.Status == DroneStatus.available)
            {
                bL.AssignmentOfPackageToDrone(drone.DroneID, parcelInDeliverd);
                MessageBox.Show("הרחפן שויך בהצלחה", "אישור", MessageBoxButton.OK);

            }
            else
            {
                MessageBox.Show("הרחפן לא פנוי", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);

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

        private void StatusDrone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            droneToList = (DroneToList)StatusDrone.SelectedItem;

        }

    }
}