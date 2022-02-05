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
using BlApi;

namespace PLGui
{

    public enum SlotsSTatus { הכל, פנוי, מלא };

    /// <summary>
    /// Interaction logic for BaseStationWindow.xaml
    /// </summary>
    public partial class BaseStationWindow : Window
    {
        IBL GetBL;
        public BaseStationToList baseStationToList;
        public BaseStation baseStation { set; get; }
        public int index;
        private MainWindow mainWindow;
        private BaseStationToList basetation;
        private int indexBaseStation;

        /// <summary>
        /// Constructor for add baseStation
        /// </summary>
        /// <param name = "getBL" ></ param >
        /// < param name="_mainWindow"></param>
        public BaseStationWindow(IBL getBL, MainWindow _mainWindow)
        {
            InitializeComponent();
            GetBL = getBL;
            baseStation = new BaseStation();
            //baseStation = getBL.GetBaseStation(baseStation.ID);
            DataContext = baseStation;
            mainWindow = _mainWindow;
            UpdateVisibility();

        }
        /// <summary>
        /// Constructor for update drone
        /// </summary>
        /// <param name="getBL"></param>
        /// <param name="_baseStation"></param>
        /// <param name="_mainWindow"></param>
        /// <param name="_index"></param>
        public BaseStationWindow(IBL getBL, MainWindow _mainWindow, BaseStation _baseStation, int _index)
        {
            InitializeComponent();
            BaseStationGrid.Visibility = Visibility.Visible;
            GetBL = getBL;
            index = _index;
            mainWindow = _mainWindow;
            baseStation = getBL.GetBaseStation(_baseStation.ID);
            DataContext = baseStation;
            UpdateGridVisibility();

        }

        private void UpdateGridVisibility()
        {

            //addStation.Visibility = Visibility.Hidden;
        }

        private void UpdateVisibility() // hidden Button - upgrade and remove
        {
            //StationID.IsReadOnly = false;
            //UpdateBaseStation.Visibility = Visibility.Hidden;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]$");
            e.Handled = regex.IsMatch(e.Text);
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

        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה בטוח שאתה רוצה להוסיף את תחנה?"
              , "הכנס תחנה", MessageBoxButton.YesNoCancel);

            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        GetBL.AddBaseStation(baseStation);
                        mainWindow.baseStationToLists.Add(GetBL.GetBasetationToListsByPredicate()
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SlutsSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void StationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void onlyNumbersForID(object sender, TextCompositionEventArgs e)
        {

        }

        private void onlytwoNumbers(object sender, TextCompositionEventArgs e)
        {

        }

        private void lungetudePattren(object sender, TextCompositionEventArgs e)
        {

        }

        private void lattitudePattren(object sender, TextCompositionEventArgs e)
        {

        }

        private void Drones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}


