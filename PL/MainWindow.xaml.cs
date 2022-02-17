using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
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
using BlApi;
using PL;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace PLGui
{

    public enum FreeChargeStatus { הכל, פנוי, מלא };
    public enum WeightCategories { All, light, medium, heavy };
    public enum DroneStatus { All, available, maintenance, busy };
    public enum Priorities { הכל, רגיל, מהיר, דחוף };
    public enum ParcelStatus { הכל, נוצר, שוייך, נאסף, סופק };

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL getBL;
        public ObservableCollection<DroneToList> dronesToLists;
        public ObservableCollection<ParcelToList> parcelToLists;
        public ObservableCollection<CustomerToList> customerToLists;
        public ObservableCollection<BaseStationToList> baseStationToLists;
        public CustomerToList customerToList;
        public Customer customer;
        public DroneToList droneToList;

        public MainWindow()
        {
            getBL = BlFactory.GetBl();

            InitializeComponent();
            InitDrones();
            InitParcels();
            InitCustomer();
            InitBaseStation();


            customer = new Customer() { LocationCustomer = new Location() };
            DataContext = customer;
        }

        #region Init Object
        public void InitDrones()
        {
            dronesToLists = new();
            IEnumerable<DroneToList> temp = getBL.GetDroneToListsBLByPredicate();
            foreach (var item in temp)
            {
                dronesToLists.Add(item);
            }
            cmbStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            cmbWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            cmbStatusSelector.SelectedIndex = 0;
            dronesToLists.CollectionChanged += SortedDrones_CollectionChanged;

            lstDroneListView.ItemsSource = dronesToLists;

            controlComboBoxesSortedDrone();
        }

        public void InitParcels()
        {
            parcelToLists = new();
            IEnumerable<ParcelToList> parcelTemp = getBL.GetParcelToListsByPredicate();
            foreach (var item in parcelTemp)
            {
                parcelToLists.Add(item);
            }
            cmbStatusSelectorParcel.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            cmbPrioritiSelectorParcel.ItemsSource = Enum.GetValues(typeof(Priorities));
            cmbWeightSelectorParcel.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            cmbWeightSelectorParcel.SelectedIndex = 0;
            parcelToLists.CollectionChanged += SortedParcels_CollectionChanged;

            lstParcelListView.ItemsSource = parcelToLists;

            controlComboBoxesSortedParcel();
        }

        public void InitCustomer()
        {
            customerToLists = new();
            IEnumerable<CustomerToList> customerTemp = getBL.GetCustomerToList();
            foreach (var item in customerTemp)
            {
                customerToLists.Add(item);
            }
            lstCustomerListView.ItemsSource = customerToLists;
        }

        public void InitBaseStation()
        {
            baseStationToLists = new();
            IEnumerable<BaseStationToList> basetationTemp = getBL.GetBasetationToListsByPredicate();
            foreach (var item in basetationTemp)
            {
                baseStationToLists.Add(item);
            }
            cmbFreeChrgeSelector.ItemsSource = Enum.GetValues(typeof(FreeChargeStatus));

            cmbFreeChrgeSelector.SelectedIndex = 0;
            baseStationToLists.CollectionChanged += SortedCharges_CollectionChanged;

            lstBaseStationListView.ItemsSource = baseStationToLists;

            cmbSortedCharges();
        }



        #endregion

        #region Double Click

        private void lstDroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList drones = (DroneToList)lstDroneListView.SelectedItem;
            if (drones != null)
            {
                new DroneWindow(getBL, this, drones.DroneID).Show();
            }
        }

        private void lstBaseStationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BaseStationToList basetation = (BaseStationToList)lstBaseStationListView.SelectedItem;
            if (basetation != null)
            {
                new BaseStationWindow(getBL, this, basetation.ID, lstBaseStationListView.SelectedIndex).Show();
            }
        }

        private void lstCustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerToList customer = (CustomerToList)lstCustomerListView.SelectedItem;
            if (customer != null)
            {
                new CustomerWindow(getBL, this, customer, lstCustomerListView.SelectedIndex).Show();
            }
        }

        private void lstParcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToList parcelToList = (ParcelToList)lstParcelListView.SelectedItem;
            if (parcelToList != null)
            {
                new ParcelWindow(getBL, this, parcelToList.Id).Show();
            }
        }
        #endregion

        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        #region ComboBox List Drone
        private void cmbStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            controlComboBoxesSortedDrone();
        }
        private void cmbWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            controlComboBoxesSortedDrone();
        }

        private void controlComboBoxesSortedDrone()
        {
            if (cmbWeightSelector.SelectedIndex == -1)
            {
                cmbWeightSelector.SelectedIndex = 0;
            }
            if (cmbStatusSelector.SelectedIndex == -1)
            {
                cmbStatusSelector.SelectedIndex = 0;
            }
            WeightCategories weightCategories = (WeightCategories)cmbWeightSelector.SelectedItem;
            DroneStatus droneStatus = (DroneStatus)cmbStatusSelector.SelectedItem;

            if (weightCategories == WeightCategories.All && droneStatus == DroneStatus.All)
                lstDroneListView.ItemsSource = dronesToLists;

            else if (weightCategories != WeightCategories.All && droneStatus == DroneStatus.All)
                lstDroneListView.ItemsSource = dronesToLists.ToList().FindAll(i => i.DroneWeight == (BO.WeightCategories)weightCategories);

            else if (weightCategories == WeightCategories.All && droneStatus != DroneStatus.All)
                lstDroneListView.ItemsSource = dronesToLists.ToList().FindAll(i => i.Status == (BO.DroneStatus)droneStatus);

            else
                lstDroneListView.ItemsSource = dronesToLists.ToList().FindAll(i => i.DroneWeight == (BO.WeightCategories)weightCategories
                && i.Status == (BO.DroneStatus)droneStatus);
            GroupingDroneList();
        }

        private void SortedDrones_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            controlComboBoxesSortedDrone();
        }

        #endregion

        #region ComboBox List Parcel
        private void cmbStatusSelectorParcel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            controlComboBoxesSortedParcel();
        }
        private void cmbPrioritiSelectorParcel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            controlComboBoxesSortedParcel();
        }
        private void cmbWeightSelectorParcel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            controlComboBoxesSortedParcel();
        }
        private void controlComboBoxesSortedParcel()
        {
            if (cmbPrioritiSelectorParcel.SelectedIndex == -1)
            {
                cmbPrioritiSelectorParcel.SelectedIndex = 0;
            }
            if (cmbStatusSelectorParcel.SelectedIndex == -1)
            {
                cmbStatusSelectorParcel.SelectedIndex = 0;
            }
            if (cmbWeightSelectorParcel.SelectedIndex == -1)
            {
                cmbWeightSelectorParcel.SelectedIndex = 0;
            }

            WeightCategories weightCategories = (WeightCategories)cmbWeightSelectorParcel.SelectedItem;
            ParcelStatus parcelStatus = (ParcelStatus)cmbStatusSelectorParcel.SelectedItem;
            Priorities priorities = (Priorities)cmbPrioritiSelectorParcel.SelectedItem;

            if ((weightCategories == WeightCategories.All) && (parcelStatus == ParcelStatus.הכל) && (priorities == Priorities.הכל))
                lstParcelListView.ItemsSource = parcelToLists;

            else if ((weightCategories != WeightCategories.All) && (parcelStatus == ParcelStatus.הכל) && (priorities == Priorities.הכל))
                lstParcelListView.ItemsSource = parcelToLists.ToList().FindAll(i => i.Weight == (BO.WeightCategories)weightCategories);

            else if ((weightCategories == WeightCategories.All) && (parcelStatus != ParcelStatus.הכל) && (priorities == Priorities.הכל))
                lstParcelListView.ItemsSource = parcelToLists.ToList().FindAll(i => i.parcelStatus == (BO.ParcelStatus)parcelStatus);

            else if ((weightCategories == WeightCategories.All) && (parcelStatus == ParcelStatus.הכל) && (priorities != Priorities.הכל))
                lstParcelListView.ItemsSource = parcelToLists.ToList().FindAll(i => i.Priority == (BO.Priorities)priorities);


            else if ((weightCategories != WeightCategories.All) && (parcelStatus != ParcelStatus.הכל) && (priorities == Priorities.הכל))
                lstParcelListView.ItemsSource = parcelToLists.ToList().FindAll(i => i.Weight == (BO.WeightCategories)weightCategories &&
                i.parcelStatus == (BO.ParcelStatus)parcelStatus);

            else if ((weightCategories != WeightCategories.All) && (parcelStatus == ParcelStatus.הכל) && (priorities != Priorities.הכל))
                lstParcelListView.ItemsSource = parcelToLists.ToList().FindAll(i => i.Weight == (BO.WeightCategories)weightCategories &&
                i.Priority == (BO.Priorities)priorities);

            else if ((weightCategories == WeightCategories.All) && (parcelStatus != ParcelStatus.הכל) && (priorities != Priorities.הכל))
                lstParcelListView.ItemsSource = parcelToLists.ToList().FindAll(i => i.Priority == (BO.Priorities)priorities &&
                i.parcelStatus == (BO.ParcelStatus)parcelStatus);

            else
                lstParcelListView.ItemsSource = parcelToLists.ToList().FindAll(i => i.Priority == (BO.Priorities)priorities &&
                i.parcelStatus == (BO.ParcelStatus)parcelStatus && i.Weight == (BO.WeightCategories)weightCategories);

            GroupingParcelList();
        }
        private void SortedParcels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            controlComboBoxesSortedParcel();
        }

        #endregion

        #region ComboBox List BaseStation

        private void cmbFreeChrgeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbSortedCharges();
        }

        private void SortedCharges_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            cmbSortedCharges();
        }

        private void cmbSortedCharges()
        {
            FreeChargeStatus freeChargeSort = (FreeChargeStatus)cmbFreeChrgeSelector.SelectedItem;

            if (freeChargeSort == FreeChargeStatus.הכל)
                lstBaseStationListView.ItemsSource = baseStationToLists;

            else if (freeChargeSort == FreeChargeStatus.פנוי)
                lstBaseStationListView.ItemsSource = baseStationToLists.ToList().FindAll(i => i.AvailableChargingStations > 0);

            else if (freeChargeSort == FreeChargeStatus.מלא)
                lstBaseStationListView.ItemsSource = baseStationToLists.ToList().FindAll(i => i.AvailableChargingStations <= 0);
        }
        #endregion

        #region LOGIN
        private void btnEnterApp_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserId.Text == "" || txtPasswordBox1.Password == "")
            {
                MessageBox.Show("נא הכנס שם משתמש וסיסמא");
                return;
            }
            try
            {
                User user = getBL.GetUser(txtUserId.Text);
                if (user.UserId == txtUserId.Text && user.Password == txtPasswordBox1.Password)
                {
                    CompanyManagement.Visibility = Visibility.Visible;
                    LoginManagement.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("הקוד שגוי");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("המשתמש לא נמצא");
                return;
            }
        }

        private void btnCustomerEnter_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserIdCustomer.Text == "")
            {
                MessageBox.Show("נא הכנס שם משתמש ");
                return;
            }

            List<CustomerToList> customers = getBL.GetCustomerToList().ToList();
            var customerCombo = from item in customers
                                select item.CustomerId;
            int find = customerCombo.FirstOrDefault(i => i == int.Parse(txtUserIdCustomer.Text.ToString()));
            if (find != default)
            {
                int IdCustomer = int.Parse(find.ToString());
                new LoginCustomerWindow(getBL, this, IdCustomer).Show();
            }
            else
                MessageBox.Show("מספר המשתמש אינו קיים\n אנא נסה שוב", "אישור");
        }

        private void txtRegisterApp_Click(object sender, RoutedEventArgs e)
        {
            if (customer.CustomerId != default && customer.NameCustomer != default && customer.PhoneCustomer != default && customer.LocationCustomer.Longtitude != default && customer.LocationCustomer.Latitude != default)
            {
                try
                {

                    MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לאשר הוספה זו", "אישור", MessageBoxButton.OKCancel);
                    switch (messageBoxResult)
                    {

                        case MessageBoxResult.OK:
                            getBL.AddNewCustomer(customer);
                            MessageBox.Show("הלקוח נוצר  בהצלחה\n מיד יוצגו פרטי לקוח", "אישור");
                            new LoginCustomerWindow(getBL,this ,customer.CustomerId).Show();
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                        default:
                            break;

                    }
                }
                catch (BO.CustumerException)
                {
                    MessageBox.Show("לקוח זה כבר קיים", "אישור");
                }
            }
            else
                MessageBox.Show("נא השלם את השדות החסרים", "אישור");
        }

        #endregion

        #region ADD Object Click
        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            var x = new DroneWindow(getBL, this);
            x.GridAddDrone.Visibility = Visibility.Visible;
            x.GridUpdateDrone.Visibility = Visibility.Hidden;
            x.Height = 500;
            x.Width = 500;
            x.Show();
        }

        private void addBaseStation_Click(object sender, RoutedEventArgs e)
        {
            var x = new BaseStationWindow(getBL, this);
            x.GridAddStation.Visibility = Visibility.Visible;
            x.GridUpdateStation.Visibility = Visibility.Hidden;
            x.Height = 450;
            x.Width = 500;
            x.ShowDialog();
        }

        private void addingCustomer_Click(object sender, RoutedEventArgs e)
        {
            var x = new CustomerWindow(getBL, this);
            x.GridAddCustomer.Visibility = Visibility.Visible;
            x.GridUpdateCustomer.Visibility = Visibility.Hidden;
            x.Height = 600;
            x.Width = 400;
            x.ShowDialog();
        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            var x = new ParcelWindow(getBL, this);
            x.AddParcel.Visibility = Visibility.Visible;
            x.GridUpdateParcel.Visibility = Visibility.Hidden;
            x.Height = 500;
            x.Width = 400;
            x.ShowDialog();
        }

        #endregion

        #region GROUPING

        CollectionView droneView;
        /// <summary>
        /// grouping functaion by DroneStatus
        /// </summary>
        public void GroupingDroneList()
        {
            string choise = "Status";
            droneView = (CollectionView)CollectionViewSource.GetDefaultView(lstDroneListView.ItemsSource);
            if (droneView.CanGroup == true)
            {
                PropertyGroupDescription groupDescription = new(choise);
                droneView.GroupDescriptions.Clear();
                droneView.GroupDescriptions.Add(groupDescription);
            }
            else
            {
                return;
            }
        }

        CollectionView parcelView;
        /// <summary>
        /// grouping functaion by SenderName.
        /// </summary>
        public void GroupingParcelList()
        {
            string choise = "SenderName";
            parcelView = (CollectionView)CollectionViewSource.GetDefaultView(lstParcelListView.ItemsSource);

            if (parcelView.CanGroup == true)
            {
                PropertyGroupDescription groupDescription = new(choise);
                parcelView.GroupDescriptions.Clear();
                parcelView.GroupDescriptions.Add(groupDescription);
                parcelView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
            }

            else
                return;
        }

        #endregion

        private void TabItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CompanyManagement.Visibility = Visibility.Hidden;
            LoginManagement.Visibility = Visibility.Visible;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string tabItem = ((sender as TabControl).SelectedItem as TabItem).Name;
            //switch (tabItem)
            //{

            //    case "Drones":
            //        InitDrones();
            //        break;

            //    case "Parcels":
            //        InitParcels();
            //        GroupingParcelList();
            //        break;

            //    case "Customers":
            //        InitCustomer();
            //        break;

            //    case "Basetation":
            //        InitBaseStation();
            //        break;

            //    case "undo":

            //        break;
            //    default:
            //        break;
            //}
        }

        private void onlyNumbersForID(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regex = new("^[0-9]{0,9}$");
            e.Handled = !regex.IsMatch(temp);
        }

        private void phonePattren(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regex = new("^[0][0-9]{0,9}$");
            e.Handled = !regex.IsMatch(temp);
        }

        private void onlyAlphaBeta(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new("[^א-ת]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void lungetudePattren(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regexA = new("^[2-3]{1,2}[.]{0,1}$");
            Regex regexB = new("^[2-3]{1,2}[.][0-9]{0,9}$");
            e.Handled = !(regexA.IsMatch(temp) || regexB.IsMatch(temp));
        }

        private void lattitudePattren(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regexA = new("^[3-4]{1,2}[.]{0,1}$");
            Regex regexB = new("^[3-4]{1,2}[.][0-9]{0,9}$");
            e.Handled = !(regexA.IsMatch(temp) || regexB.IsMatch(temp));
        }
    }
}


