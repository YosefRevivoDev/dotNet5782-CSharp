using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


namespace PL
{
    /// <summary>
    /// Interaction logic for ShowDronesWindow.xaml
    /// </summary>
    public partial class ShowDronesWindow : Window
    {
        BL getBL;
        public ObservableCollection<DroneToList> DronesToList;

        public ShowDronesWindow(BL getBL)
        {
            InitializeComponent();
            DronesToList = new();
            foreach (var item in getBL.GetDroneToListsBLByPredicate())
            {
                DronesToList.Add(item);
            }

            cmbStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            cmbWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            this.getBL = getBL;
            lstDroneListView.ItemsSource = DronesToList;
        }

        private void cmbStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneStatus status = (DroneStatus)cmbStatusSelector.SelectedItem;
            txtLable.Text = status.ToString();
            lstDroneListView.ItemsSource = DronesToList.Where(x => x.Status == status).ToList();
        }

        private void txtLable_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(getBL, this).Show();
        }

        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cmbWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories droneWeight = (WeightCategories)cmbWeightSelector.SelectedItem;
            txtLable.Text = droneWeight.ToString();
            lstDroneListView.ItemsSource = DronesToList.Where(x => x.DroneWeight == droneWeight).ToList();

        }

        private void lstDroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList drones = (DroneToList)lstDroneListView.SelectedItem;
            if (drones != null)
            {
                int Idrone = lstDroneListView.SelectedIndex;
                new DroneWindow(getBL, this, drones, Idrone).Show();
            }
           
        }
    }
}
