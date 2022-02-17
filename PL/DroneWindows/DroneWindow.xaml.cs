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
        BackgroundWorker backgroundWorker;

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
        /// <param name="_mainWindow"></param>
        /// <param name="_Idrone"></param>
        public DroneWindow(IBL _bL, MainWindow _mainWindow, int _Idrone = 0)
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
                btnSendDroneToDeliver.Visibility = Visibility.Visible;
                droneMaintenance.Visibility = Visibility.Hidden;
                FindParcel.Visibility = Visibility.Hidden;
                ChargeAndCollect.Visibility = Visibility.Visible;
                CollectParcel.Visibility = Visibility.Hidden;
                DetailsStation.Visibility = Visibility.Hidden;
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
                CollectParcel.Visibility = Visibility.Visible;
                //btnCollectParcel.Visibility = Visibility.Visible;

                if (drone.ParcelInDeliverd.StatusParcrlInDeliver == StatusParcrlInDeliver.AwaitingCollection)
                {
                    btnCollectParcel.Visibility = Visibility.Visible;
                    btnSuplyParcel.Visibility = Visibility.Hidden;
                }

                if (drone.ParcelInDeliverd.StatusParcrlInDeliver == StatusParcrlInDeliver.OnTheWayDestination)
                {
                    btnSuplyParcel.Visibility = Visibility.Visible;
                    btnCollectParcel.Visibility = Visibility.Hidden;
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

        private void refreshMainWindow()
        {
            if (mainWindow != null)
            {
                mainWindow.GroupingDroneList();
                mainWindow.lstDroneListView.Items.Refresh();
                mainWindow.lstBaseStationListView.Items.Refresh();
            }
        }

        private void refreshThisWindow()
        {
            drone = bL.GetDrone(drone.DroneID);
            DataContext = drone;
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
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה רוצה לעדכן את שם הרחפן?",
            "הכנס שם רחפן", MessageBoxButton.YesNoCancel);

                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        bL.UpdateDrone(drone.DroneID, drone.DroneModel);

                        refreshMainWindow();
                        refreshThisWindow();
                        updateAllData();
                        MessageBox.Show("הרחפן עודכן בהצלחה");
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        break;
                }
            }
            catch (BO.DroneNotUpdate)
            {
                MessageBox.Show("לא בוצעו שינויים");
            }
            catch (DO.CheckIdException Ex)
            {
                throw new CheckIdException("הרחפן קיים במערכת" + Ex);
            }
            catch (DO.CheckIfIdNotException Ex)
            {
                throw new CheckIfIdNotExceptions("ERORR", Ex);
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
                    refreshMainWindow();
                    refreshThisWindow();
                    DroneVisibility();
                    updateAllData();
                }
            }

            catch (BO.ChargExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                                  MessageBoxResult.None, MessageBoxOptions.RightAlign);
            }
            catch (BO.CheckIdException ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
            }
            catch (BO.CheckIfIdNotExceptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
            }
        }

        private void _btnRealeseDroneClick(object sender, RoutedEventArgs e)
        {

            try
            {
                bL.ReleaseDroneFromCharge(drone.DroneID);

                refreshMainWindow();
                refreshThisWindow();
                DroneVisibility();
                updateAllData();
            }
            catch (DO.CheckIdException Ex)
            {
                throw new CheckIdException("ERORR", Ex);
            }
            catch (DO.CheckIfIdNotException Ex)
            {
                throw new CheckIfIdNotExceptions("ERORR", Ex);
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
                    bL.AssignmentOfPackageToDrone(drone.DroneID);
                    refreshThisWindow();
                    refreshMainWindow();
                    DroneVisibility();
                    updateAllData();
                }
            }
            catch (BO.CheckIfIdNotExceptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
            }
            catch (BO.CheckIdException ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
            }
            catch (BO.ParcelAssociationExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
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
                    refreshThisWindow();
                    refreshMainWindow();
                    drone.ParcelInDeliverd.StatusParcrlInDeliver = StatusParcrlInDeliver.OnTheWayDestination;
                    btnSuplyParcel.Visibility = Visibility.Visible;
                    updateAllData();
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
                    refreshThisWindow();
                    refreshMainWindow();
                    DroneVisibility();
                    updateAllData();
                }
            }
            catch (BO.CheckIdException ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (DO.CheckIfIdNotException Ex)
            {
                throw new CheckIfIdNotExceptions("ERORR", Ex);
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
                            updateAllData();
                            MessageBox.Show("הרחפן נוסף בהצלחה");
                            Close();
                        }
                        catch (BO.CheckIdException ex)
                        {
                            MessageBox.Show("רחפן זה קיים במערכת", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                                MessageBoxResult.None, MessageBoxOptions.RightAlign);
                            Close();
                        }
                        catch (BO.CheckIfIdNotExceptions ex)
                        {
                            MessageBox.Show("לא תקין, אנא נסה שוב", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
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
            Close();
        }

        private void updateAllData()
        {
            mainWindow.InitBaseStation();
            mainWindow.InitCustomer();
            mainWindow.InitParcels();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        #region Simulator

        private void helpProgressChanged() => backgroundWorker.ReportProgress(0);

        private bool helpRunWorkerCompleted() => backgroundWorker.CancellationPending;

        private void StartSimulator_Click(object sender, RoutedEventArgs e)
        {
            //PleaseWaitWindow pleaseWaitWindow=new PleaseWaitWindow();
            if (StartSimulator.Content.ToString() == "הפעלת סימולטור")
            {
                visibilatyButtonsFunction(false);

                StartSimulator.Content = "עצור סימולטור";
                backgroundWorker = new BackgroundWorker();
                backgroundWorker.WorkerSupportsCancellation = true;
                backgroundWorker.WorkerReportsProgress = true;
                backgroundWorker.DoWork += BackgroundWorker_DoWork;
                backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
                backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

                backgroundWorker?.RunWorkerAsync();
            }
            else
            {
                StartSimulator.Content = "הפעלת סימולטור";
                backgroundWorker?.CancelAsync();

                //the drone in simulation
                while (backgroundWorker != null && backgroundWorker.IsBusy == true)
                {
                    MessageBox.Show("אנא המתן לסיום התהליך");
                }
                visibilatyButtonsFunction(true);
            }
        }

        private void visibilatyButtonsFunction(bool bol)
        {

            if (bol == true)
            {
                DroneVisibility();
                btnUpdateDrone.Visibility = Visibility.Visible;
                CloseWindow.Visibility = Visibility.Visible;
            }
            else
            {
                btnSendDroneToDeliver.Visibility = Visibility.Hidden;
                btnDroneCharge.Visibility = Visibility.Hidden;
                btnCollectParcel.Visibility = Visibility.Hidden;
                btnUpdateDrone.Visibility = Visibility.Hidden;
                CloseWindow.Visibility = Visibility.Hidden;
                btnRealeseDrone.Visibility = Visibility.Hidden;
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            refreshThisWindow();
            if (mainWindow != null)
            {
                mainWindow.GroupingDroneList();
                mainWindow.lstDroneListView.Items.Refresh();
                //mainWindow.lstBaseStationListView.Items.Refresh();
            }
            DroneVisibility();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bL.newSimulator(drone.DroneID, helpProgressChanged, helpRunWorkerCompleted);
        }

        #endregion

        private void CloseAddWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}


