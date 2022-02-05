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

namespace PLGui
{

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
        private BaseStation baseStation;
        private Customer customer;
        private Parcel parcel;

        public MainWindow()
        {
            getBL = BlFactory.GetBl();
            
            InitializeComponent();
            InitDrones();
            InitParcels();
            InitCustomer();
            InitBaseStation();
        }


        #region Init Object
        private void InitDrones()
        {
            dronesToLists = new();
            IEnumerable<DroneToList> temp = getBL.GetDroneToListsBLByPredicate();
            foreach (var item in temp)
            {
                dronesToLists.Add(item);
            }
            cmbStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            cmbWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            lstDroneListView.ItemsSource = dronesToLists;
        }
        private void InitParcels()
        {
            parcelToLists = new();
            IEnumerable<ParcelToList> parcelTemp = getBL.GetParcelToListsByPredicate();
            foreach (var item in parcelTemp)
            {
                parcelToLists.Add(item);
            }
            lstParcelListView.ItemsSource = parcelToLists;
            cmbParcelStatus.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
        }
        private void InitCustomer()
        {
            customerToLists = new();
            IEnumerable<CustomerToList> customerTemp = getBL.GetCustomerToList();
            foreach (var item in customerTemp)
            {
                customerToLists.Add(item);
            }
            lstCustomerListView.ItemsSource = customerToLists;
        }
        private void InitBaseStation()
        {
            baseStationToLists = new();
            IEnumerable<BaseStationToList> basetationTemp = getBL.GetBasetationToListsByPredicate();
            foreach (var item in basetationTemp)
            {
                baseStationToLists.Add(item);
            }
            lstBaseStationListView.ItemsSource = baseStationToLists;
        }
        #endregion
        private void lstDroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList drones = (DroneToList)lstDroneListView.SelectedItem;
            if (drones != null)
            {
                int Idrone = lstDroneListView.SelectedIndex;
                new DroneWindow(getBL, this, drones, Idrone).Show();
                //x = x.DroneVisibility();
            }
        }

        private void lstBaseStationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BaseStationToList basetation = (BaseStationToList)lstBaseStationListView.SelectedItem;
            if (basetation != null)
            {
                int IndexBaseStation = lstBaseStationListView.SelectedIndex;
                new BaseStationWindow(getBL, this, basetation, IndexBaseStation).Show();
            }
        }

        private void lstCustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerToList customer = (CustomerToList)lstCustomerListView.SelectedItem;
            if (customer != null)
            {
                int insexCustomer = lstCustomerListView.SelectedIndex;
                new CustomerWindow(getBL, this, customer, insexCustomer).Show();
            }
        }

        private void lstParcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToList parcelToList = (ParcelToList)lstParcelListView.SelectedItem;
            if (parcelToList != null)
            {
                int indexParcel = lstParcelListView.SelectedIndex;
                new ParcelWindow(getBL, this, parcelToList, indexParcel).Show();
            }
        }


        private void cmbStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneStatus status = (DroneStatus)cmbStatusSelector.SelectedItem;
            txtLable.Text = status.ToString();
            lstDroneListView.ItemsSource = dronesToLists.Where(x => x.Status == status).ToList();
        }

        private void txtLable_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
      
        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cmbWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories droneWeight = (WeightCategories)cmbWeightSelector.SelectedItem;
            txtLable.Text = droneWeight.ToString();
            lstDroneListView.ItemsSource = dronesToLists.Where(x => x.DroneWeight == droneWeight).ToList();
        }

        private void cmbParcelStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParcelStatus comboBoxParcelStatus = (ParcelStatus)cmbParcelStatus.SelectedItem;
            lstParcelListView.ItemsSource = parcelToLists.Where(x => x.parcelStatus == comboBoxParcelStatus).ToList();
        }

        private void LoginApp_Click(object sender, RoutedEventArgs e)
        {
            new LoginApp(this, getBL).Show();
        }

        #region ADD Object Click
        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            var x = new DroneWindow(getBL, this);
            x.GridAddDrone.Visibility = Visibility.Visible;
            x.GridUpdateDrone.Visibility = Visibility.Hidden;
            x.Height = 450;
            x.Width = 500;
            x.ShowDialog();
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
            var x = new CustomerWindow (getBL, this);
            x.GridAddCustomer.Visibility = Visibility.Visible;
            x.GridUpdateCustomer.Visibility = Visibility.Hidden;
            x.Height = 550;
            x.Width = 400;
            x.ShowDialog();
        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            var x = new ParcelWindow(getBL, this);
            x.GridAddDrone.Visibility = Visibility.Visible;
            x.GridUpdateDrone.Visibility = Visibility.Hidden;
            x.Height = 500;
            x.Width = 400;
            x.ShowDialog();
        }
        #endregion
    }
}


