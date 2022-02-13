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
        DroneToList droneToList { set; get; }
        public CustomerInParcel customerInParcel { set; get; }
        public DroneInParcel droneInParcel { set; get; }
        public int index;
        private MainWindow mainWindow;

        /// <summary>
        /// Add constructor.
        /// </summary>
        /// <param name="getBL"></param>
        /// <param name="_mainWindow"></param>
        public ParcelWindow(IBL getBL, MainWindow _mainWindow)
        {
            InitializeComponent();
            GetBL = getBL;
            parcel = new Parcel();
            DataContext = parcel;
            mainWindow = _mainWindow;
            UpdateVisibility();
            List<CustomerToList> temp = getBL.GetCustomerToList().ToList();

            comboBoxOfsander.ItemsSource = temp;
            comboBoxOfsander.DisplayMemberPath = "CustomerId";

            comboBoxOftarget.ItemsSource = temp;
            comboBoxOftarget.DisplayMemberPath = "CustomerId";

            prioritiCombo.ItemsSource = Enum.GetValues(typeof(Priorities));
            weightCombo.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }
        
        /// <summary>
        /// Update cunstructor.
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="_mainWindow"></param>
        /// <param name="_parcel"></param>
        /// <param name="_index"></param>
        public ParcelWindow(IBL bL, MainWindow _mainWindow, int _index)
        {
            InitializeComponent();
            GetBL = bL;
            index = _index;
            mainWindow = _mainWindow;
            parcelToList = bL.GetParcelToListsByPredicate(x => x.Id == _index).FirstOrDefault();
            parcel = bL.GetParcel(_index);
            DataContext = parcel;
            ParcelGrid.Visibility = Visibility.Visible;
            UpdateGridVisibility();
            
        }

        private void UpdateGridVisibility()
        {
            ParcelGrid.Visibility = Visibility.Visible;
            btnAddParcel.Visibility = Visibility.Hidden;
            droneInParcelButton.Content = parcel.droneInParcel == null ? "אין עדיין רחפן משוייך" : parcel.droneInParcel.DroneID;

        }

        private void UpdateVisibility() // hidden Button - upgrade and remove
        {
            //btnAddParcel.Visibility = Visibility.Hidden;
            //btnRemoveStation.Visibility = Visibility.Hidden;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]$");
            e.Handled = regex.IsMatch(e.Text);
        }

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
                        int idParcel = GetBL.AddNewParcel(parcel, ((CustomerToList)comboBoxOfsander.SelectedItem).CustomerId, ((CustomerToList)comboBoxOftarget.SelectedItem).CustomerId);

                        mainWindow.parcelToLists.Add(GetBL.GetParcelToListsByPredicate(i => i.Id == idParcel).First());

                        MessageBox.Show("החבילה נוספה בהצלחה");
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

        private void droneInParcelButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (parcel.droneInParcel != null)
            {
                int idDrone = parcel.droneInParcel.DroneID;
                new DroneWindow(GetBL, mainWindow, idDrone).Show();
                Close();
            }  
        }
    }
}
