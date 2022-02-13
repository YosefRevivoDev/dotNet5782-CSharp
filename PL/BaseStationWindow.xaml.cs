﻿using System;
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
using System.Collections.ObjectModel;

namespace PLGui
{

    public enum SlotsSTatus { הכל, פנוי, מלא };

    /// <summary>
    /// Interaction logic for BaseStationWindow.xaml
    /// </summary>
    public partial class BaseStationWindow : Window
    {
        IBL GetBL;
        public ObservableCollection<BaseStationToList> baseStationToLists;
        public BaseStationToList baseStationToList;
        public BaseStation baseStation { set; get; }
        public int index;
        private MainWindow mainWindow;
       
        /// <summary>
        /// Constructor for add base Station
        /// </summary>
        /// <param name = "getBL" ></ param >
        /// < param name="_mainWindow"></param>
        public BaseStationWindow(IBL getBL, MainWindow _mainWindow)
        {
            InitializeComponent();
            GetBL = getBL;
            baseStation = new BaseStation() { location = new Location() };
            DataContext = baseStation;
            mainWindow = _mainWindow;
            UpdateVisibility();

        }

        /// <summary>
        /// Constructor for update Base Station
        /// </summary>
        /// <param name="getBL"></param>
        /// <param name="_baseStation"></param>
        /// <param name="_mainWindow"></param>
        /// <param name="_index"></param>
        public BaseStationWindow(IBL getBL, MainWindow _mainWindow, int idBaseStation, int index)
        {
            InitializeComponent();
            Updating.Visibility = Visibility.Visible;
            GetBL = getBL;
            this.index = index;
            mainWindow = _mainWindow;
            baseStationToList = getBL.GetBasetationToListsByPredicate(x => x.ID == idBaseStation).FirstOrDefault();
            baseStation = getBL.GetBaseStation(idBaseStation);
            DataContext = baseStation;
            lstDronesCharge.ItemsSource = baseStation.droneCharges;
            UpdateGridVisibility();

        }

        private void UpdatingWindow(int id)
        {
            Updating.Visibility = Visibility.Visible;
            AddStation.Visibility = Visibility.Hidden;
            try
            {
                baseStation = GetBL.GetBaseStation(id);
            }
            catch (BO.CheckIfIdNotExceptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                return;
            }
            lstDronesCharge.ItemsSource = baseStation.droneCharges.ToList();
            DataContext = baseStation;
        }

        private void UpdateGridVisibility()
        {
            AddStation.Visibility = Visibility.Hidden;
            btnUpdateStation.Visibility = Visibility.Visible;
            btnAddStation.Visibility = Visibility.Hidden;

        }

        private void UpdateVisibility() // hidden Button - upgrade and remove
        {
            GridUpdateStation.Visibility = Visibility.Hidden;
            btnUpdateStation.Visibility = Visibility.Hidden;
            btnAddStation.Visibility = Visibility.Visible;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void UpdateBaseStation_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה בטוח שאתה רוצה לעדכן את התחנה?", "עדכן", MessageBoxButton.YesNoCancel);

            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        GetBL.UpdateBaseStation(baseStation.ID, baseStation.Name, baseStation.AvailableChargingStations);

                        mainWindow.baseStationToLists[index] = GetBL.GetBasetationToListsByPredicate(x => x.ID == baseStation.ID).First();
                        mainWindow.lstBaseStationListView.Items.Refresh();

                        MessageBox.Show("התחנה עודכנה בהצלחה");
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
                        MessageBox.Show( "התחנה נוספה בהצלחה");
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

        private void StationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            baseStationToList = (BaseStationToList)mainWindow.lstBaseStationListView.SelectedItem;
            if (baseStationToList != null)
            {
                UpdatingWindow(baseStationToList.ID);
            }
        }

        private void Drones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            int Idrone = ((DroneInCharging)lstDronesCharge.SelectedItem).DroneID;
            new DroneWindow(GetBL, mainWindow, Idrone).Show();
            Close();

        }
    }
}


