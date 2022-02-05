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
using System.Collections.ObjectModel;

namespace PLGui
{
    public struct Statuses
    {
        public BO.ParcelStatus ParcelStatus { get; set; }
        public BO.WeightCategories Weight { get; set; }
        public BO.Priorities Priority { get; set; }
    }
   
    /// <summary>
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        IBL GetBL;
        public ObservableCollection<ParcelToList> parcelToLists;
        public ParcelToList parcelToList;
        public Parcel parcel { set; get; }
        public CustomerInParcel customerInParcel { set; get; }
        public int index;
        private MainWindow mainWindow;

        public ParcelWindow(IBL getBL, MainWindow _mainWindow)
        {
            InitializeComponent();
            GetBL = getBL;
            parcel = new Parcel();
            //parcel = getBL.GetParcel(parcel.Id);
            DataContext = parcel;
            mainWindow = _mainWindow;
            UpdateVisibility();

        }
        
        public ParcelWindow(IBL bL, MainWindow _mainWindow, ParcelToList _parcel, int _index)
        {
            InitializeComponent();
            ParcelGrid.Visibility = Visibility.Visible;
            GetBL = bL;
            index = _index;
            mainWindow = _mainWindow;
            parcel = bL.GetParcel(_parcel.Id);
            DataContext = parcel;
            UpdateGridVisibility();

        }

        private void UpdateGridVisibility()
        {
            ParcelGrid.Visibility = Visibility.Visible;
            AddParcel.Visibility = Visibility.Hidden;

        }

        private void UpdateVisibility() // hidden Button - upgrade and remove
        {
            //ParcelID.IsReadOnly = false;
            //btnUpdateParcel.Visibility = Visibility.Hidden;
            //btnRemoveStation.Visibility = Visibility.Hidden;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        //private void btnUpdateParcel_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה בטוח שאתה רוצה לעדכן את החבילה?"
        //     , "הכנס חבילה", MessageBoxButton.YesNoCancel);

        //    switch (messageBoxResult)
        //    {
        //        case MessageBoxResult.Yes:
        //            try
        //            {
        //                GetBL.upd();
        //                baseStationToList.Name = baseStation.Name;
        //                mainWindow.baseStationToLists[index] = baseStationToList;
        //                mainWindow.lstBaseStationListView.Items.Refresh();
        //                MessageBox.Show(baseStation.ToString(), "החבילה עודכנה בהצלחה");
        //                Close();
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message);
        //            }
        //            break;
        //        case MessageBoxResult.Cancel:
        //            Close();
        //            break;
        //        case MessageBoxResult.No:
        //            break;
        //        default:
        //            break;
        //    }
        //}

        private void btnRemoveStation_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("? האם אתה בטוח שאתה רוצה למחוק תחנה זאת"
              , "", MessageBoxButton.YesNoCancel);

            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        GetBL.RemoveParcelBL(parcel.Id);
                        mainWindow.parcelToLists.Remove(parcelToList);
                        mainWindow.parcelToLists[index] = parcelToList;
                        mainWindow.lstParcelListView.Items.Refresh();
                        MessageBox.Show(parcel.ToString(), "התחנה נמחקה בהצלחה");
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

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("? האם אתה בטוח שאתה רוצה להוסיף חבילה זו"
               , "הכנס תחנה", MessageBoxButton.YesNoCancel);

            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        GetBL.AddNewParcel(parcel, parcel.Sender.CustomerId, parcel.Target.CustomerId);
                        mainWindow.parcelToLists.Add(GetBL.GetParcelToListsByPredicate()
                            .First(i => i.Id == parcel.Id));
                        MessageBox.Show(parcel.ToString(), "החבילה נוספה בהצלחה");
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

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void prioritiSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void weightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void droneInParcelButton_Click(object sender, RoutedEventArgs e)
        {
            //int drone = parcel.Id;
            //new DroneWindow(GetBL, mainWindow, drone, index).Show();
            //Close();
        }

        private void ParcelListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
