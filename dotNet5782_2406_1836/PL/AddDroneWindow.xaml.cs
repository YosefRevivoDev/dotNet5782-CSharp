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

namespace PL
{
    /// <summary>
    /// Interaction logic for AddDroneWindow.xaml
    /// </summary>
    public partial class AddDroneWindow : Window
    {
        Drone addNewDrone;
        BL bL;
        ShowDronesWindow _showDronesWindow;
        public AddDroneWindow(BL getBl, ShowDronesWindow showDronesWindow)
        {
            bL = getBl;
            addNewDrone = new Drone();
            DataContext = addNewDrone;
            InitializeComponent();
            DroneSituateGrid.Visibility = Visibility.Collapsed;
            droneGrid.VerticalAlignment = VerticalAlignment.Center;
            droneGrid.HorizontalAlignment = HorizontalAlignment.Center;
            ///droneGrid.Margin = ;
            _showDronesWindow = showDronesWindow;
            DroneWeight.ItemsSource = Enum.GetValues(typeof(DroneWeightCategories));
        }

        private void addDrone_Click(object sender, RoutedEventArgs e)
        {
           MessageBoxResult messageBoxResult =  MessageBox.Show("האם אתה בטוח שאתה רוצה להוסיף את הרחפן?"
               , "Insert Drone",  MessageBoxButton.YesNoCancel);

           
            switch (messageBoxResult)
            {
                 case MessageBoxResult.Yes:
                    try
                    {
                        bL.AddNewDrone(addNewDrone, bL.GetDroneToListsBLByPredicate().ToList().Count);
                        _showDronesWindow.DronesToList.Add(bL.GetDroneToListsBLByPredicate()
                            .First(i => i.DroneID == addNewDrone.DroneID));
                        MessageBox.Show(addNewDrone.ToString(), "Good Inser Drone");
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

        private void DroneId_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]$");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}