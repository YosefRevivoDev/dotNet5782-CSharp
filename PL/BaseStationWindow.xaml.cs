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

namespace PLGui
{
    /// <summary>
    /// Interaction logic for BaseStationWindow.xaml
    /// </summary>
    public partial class BaseStationWindow : Window
    {
        BL.BL GetBL;
        public BaseStationToList baseStationToList;
        public BaseStation baseStation { set; get; }
        public int index;
        private MainWindow mainWindow;

        /// <summary>
        /// Constructor for add baseStation
        /// </summary>
        /// <param name = "getBl" ></ param >
        /// < param name="BaseStationWindow"></param>
        public BaseStationWindow(BL.BL getBL, MainWindow _mainWindow)
        {
            InitializeComponent();
            GetBL = getBL;
            baseStation = new BaseStation();
            baseStation = getBL.GetBaseStation(baseStation.ID);
            DataContext = baseStation;
            BaseStationGrid.VerticalAlignment = VerticalAlignment.Center;
            BaseStationGrid.HorizontalAlignment = HorizontalAlignment.Center;
            mainWindow = _mainWindow;
            UpdateVisibility();

        }
        /// <summary>
        /// Constructor for update drone
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="_baseStation"></param>
        /// <param name="baseStation"></param>
        /// <param name="_index"></param>
        public BaseStationWindow(BL.BL bL, MainWindow _mainWindow, BaseStation _baseStation, int _index)
        {
            InitializeComponent();
            BaseStationGrid.Visibility = Visibility.Visible;
            GetBL = bL;
            index = _index;
            mainWindow = _mainWindow;
            baseStation = bL.GetBaseStation(_baseStation.ID);
            DataContext = baseStation;
            UpdateGridVisibility();

        }

        private void UpdateGridVisibility()
        {
            
            addStation.Visibility = Visibility.Hidden;
        }

        private void UpdateVisibility() // hidden Button - upgrade and remove
        {
            StationID.IsReadOnly = false;
            RemoveStation.Visibility = Visibility.Hidden;
            UpdateBaseStation.Visibility = Visibility.Hidden;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void addStation_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה בטוח שאתה רוצה להוסיף את תחנה?"
                , "הכנס תחנה", MessageBoxButton.YesNoCancel);

            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        GetBL.AddBaseStation(baseStation);
                        mainWindow.baseStationToLists.Add(GetBL.GetBasetationToLists()
                            .First(i => i.ID == baseStation.ID));
                        MessageBox.Show(baseStation.ToString(), "הרחפן נוסף בהצלחה");
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

        private void UpdateBaseStation_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה בטוח שאתה רוצה לעדכן את תחנה?"
              , "הכנס תחנה", MessageBoxButton.YesNoCancel);

            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        GetBL.UpdateBaseStation(baseStation.ID, baseStation.Name, baseStation.AvailableChargingStations);
                        baseStationToList.Name = baseStation.Name;
                        mainWindow.baseStationToLists[index] = baseStationToList;
                        mainWindow.lstBaseStationListView.Items.Refresh();
                        MessageBox.Show(baseStation.ToString(), "התחנה עודכנה בהצלחה");
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

        private void RemoveStation_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("? האם אתה בטוח שאתה רוצה למחוק תחנה זאת"
               , "", MessageBoxButton.YesNoCancel);

            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        GetBL.RemoveBaseStationBL(baseStation.ID);
                        mainWindow.baseStationToLists.Remove(baseStationToList);
                        mainWindow.baseStationToLists[index] = baseStationToList;
                        mainWindow.lstBaseStationListView.Items.Refresh();
                        MessageBox.Show(baseStation.ToString(), "התחנה נמחקה בהצלחה");
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
    }
}
