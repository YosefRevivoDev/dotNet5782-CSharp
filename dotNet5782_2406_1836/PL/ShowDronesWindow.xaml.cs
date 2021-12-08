using System;
using System.Collections.Generic;
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
using IBL;


namespace PL
{
    /// <summary>
    /// Interaction logic for ShowDronesWindow.xaml
    /// </summary>
    public partial class ShowDronesWindow : Window
    {
        IBL.IBL GetBL;

        public ShowDronesWindow(IBL.IBL getBL)
        {
            InitializeComponent();
            this.cmbStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneWeightCategories));
            this.GetBL = getBL;
        }

        private void cmbStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneStatus status = (DroneStatus)cmbStatusSelector.SelectedItem;
            this.txtLable.Text = status.ToString();
            this.lstDroneListView.ItemsSource = GetBL.DroneToList()
        }
    }
}
