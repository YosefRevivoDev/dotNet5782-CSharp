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
using BO;
using System.ComponentModel.DataAnnotations;


namespace PL
{
    /// <summary>
    /// Interaction logic for AddDroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private Drone Drone { set; get; }
        public BL bL;
        private DroneToList DroneToList;
        private ShowDronesWindow ShowDronesWindow;
        private ParcelInDeliver ParcelInDeliver;
        private BasetationToList BaseStation { set; get; }

        /// <summary>
        /// Constructor for add drone
        /// </summary>
        /// <param name="getBl"></param>
        /// <param name="_showDronesWindow"></param>
        public DroneWindow(BL getBl, ShowDronesWindow _showDronesWindow)
        {
            InitializeComponent();
            bL = getBl;
            Drone = new Drone();
            DataContext = Drone;
            DroneSituateGrid.Visibility = Visibility.Collapsed;
            droneGrid.VerticalAlignment = VerticalAlignment.Center;
            droneGrid.HorizontalAlignment = HorizontalAlignment.Center;
            ///droneGrid.Margin = ;
            ItemsSourceStations();

            ShowDronesWindow = _showDronesWindow;
            DroneWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }


        public int Idrone;

        /// <summary>
        /// Constructor for update drone
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="_ShowDronesWindow"></param>
        /// <param name="drone"></param>
        /// <param name="_Idrone"></param>
        public DroneWindow(BL bL, ShowDronesWindow _ShowDronesWindow, DroneToList drone, int _Idrone)
        {

            InitializeComponent();
            DroneSituateGrid.Visibility = Visibility.Visible;
            Idrone = _Idrone;
            this.bL = bL;
            Drone = bL.GetDrone(drone.DroneID);
            ShowDronesWindow = _ShowDronesWindow;
            DataContext = Drone;
            UpdateGridVisibility();
            ItemsSourceStations();
            DroneToList = drone;
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

            if (BaseStation != null)
            {
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            bL.AddNewDrone(Drone, bL.GetDroneToListsBLByPredicate().ToList().Count);
                            ShowDronesWindow.DronesToList.Add(bL.GetDroneToListsBLByPredicate()
                                .First(i => i.DroneID == Drone.DroneID));
                            MessageBox.Show(Drone.ToString(), "הרחפן נוסף בהצלחה");
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
                MessageBox.Show("חמור יקר אנא בחר תחנה", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
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
            ShowDronesWindow.IsEnabled = true;
            Close();
        }

        private void btnUpdateModel_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה רוצה לעדכן את שם הרחפן?",
                "הכנס שם רחפן", MessageBoxButton.YesNoCancel);

            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    bL.UpdateDrone(Drone.DroneID, Drone.DroneModel);
                    DroneToList.DroneModel = Drone.DroneModel;
                    ShowDronesWindow.DronesToList[Idrone] = DroneToList;
                    ShowDronesWindow.lstDroneListView.Items.Refresh();

                    MessageBox.Show(Drone.ToString(), "הרחפן עודכן בהצלחה");
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
        private void btnSendDroneToCharge(object sender, RoutedEventArgs e)
        {
            if (Drone.Status == DroneStatus.available && BaseStation != null)
            {
                bL.SendDroneToCharge(Drone.DroneID, BaseStation.ID);
            }
        }

        private void _btnRealeseDroneClick(object sender, RoutedEventArgs e)
        {
            if (Drone.Status == DroneStatus.maintenance)
            {
                bL.ReleaseDroneFromCharge(Drone.DroneID, BaseStation.ID, DateTime.Now);
            }
            else
            {
                MessageBox.Show("הרחפן לא במצב תחזוקה", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NumberOfStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BaseStation = (BasetationToList)NumberOfStations.SelectedItem;
        }

        private void btnSendDroneToDeliver_Click(object sender, RoutedEventArgs e)
        {
            if (Drone.Status == DroneStatus.available)
            {
                bL.AssignmentOfPackageToDrone(Drone.DroneID, ParcelInDeliver);
                MessageBox.Show("הרחפן שויך בהצלחה", "אישור", MessageBoxButton.OK);

            }
            else
            {
                MessageBox.Show("הרחפן לא פנוי", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void btnCollectParcel_Click(object sender, RoutedEventArgs e)
        {

            bL.CollectParcelByDrone(Drone.DroneID);

            try
            {
                MessageBoxResult result = MessageBox.Show("נכנסת בהצלחה", "מידע", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    
                    case MessageBoxResult.OK:
                        
                        bL.CollectParcelByDrone(Drone.DroneID);
                        Drone = bL.GetDrone(Drone.DroneID);
                        DataContext = Drone;
                        ShowDronesWindow.lstDroneListView.Items.Refresh();
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
    }
}