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
            mainWindow = _mainWindow;
            parcel = new Parcel();
            DataContext = parcel;
            UpdateVisibility();
            List<CustomerToList> temp = getBL.GetCustomerToList().ToList();

            comboBoxOfsander.ItemsSource = temp;
            comboBoxOfsander.DisplayMemberPath = "CustomerId";

            comboBoxOftarget.ItemsSource = temp;
            comboBoxOftarget.DisplayMemberPath = "CustomerId";

            prioritiCombo.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            weightCombo.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
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

            try
            {
                parcelToList = bL.GetParcelToListsByPredicate(x => x.Id == _index).FirstOrDefault();
                parcel = bL.GetParcel(_index);
                DataContext = parcel;
                switch (parcelToList.parcelStatus)
                {
                    case BO.ParcelStatus.Defined:
                        txtScheduled.Text = "חבילה ממתינה לשיוך";
                        txtPickedUp.Text = "בהמתנה לאיסוף";
                        txtDelivered.Text = "בהמתנה לאספקה";
                        break;
                    case BO.ParcelStatus.associated:
                        txtPickedUp.Text = "בהמתנה לאיסוף";
                        txtDelivered.Text = "בהמתנה לאספקה";
                        break;
                    case BO.ParcelStatus.collected:
                        txtDelivered.Text = "בהמתנה לאספקה";
                        break;
                    default:
                        break;
                }
            }
            catch (BO.CheckIfIdNotExceptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                MessageBoxResult.None, MessageBoxOptions.RightAlign);
            }

            ParcelGrid.Visibility = Visibility.Visible;
            UpdateGridVisibility();
        }

        private void refreshMainWindow()
        {
            if (mainWindow != null)
            {
                mainWindow.GroupingDroneList();
                mainWindow.lstParcelListView.Items.Refresh();
            }
        }

        private void refreshThisWindow()
        {
            parcel = GetBL.GetParcel(parcel.Id);
            DataContext = parcel;
        }

        private void updateAllData()
        {
            mainWindow.InitCustomer();
            mainWindow.InitParcels();
        }

        private void UpdateGridVisibility()
        {
            ParcelGrid.Visibility = Visibility.Visible;
            btnAddParcel.Visibility = Visibility.Hidden;
            btnRemoveParcel.Visibility = Visibility.Visible;
            if (parcel.assigned == DateTime.MinValue)
            {
                lblParcelNotAssign.Visibility = Visibility.Visible;
                lblActionParcelDeliver.Visibility = Visibility.Hidden;
                droneInParcelButton.Visibility = Visibility.Hidden;
            }
            if (parcel.Delivered != DateTime.MinValue)
            {
                lblDelivered.Visibility = Visibility.Visible;
                lblParcelNotAssign.Visibility = Visibility.Hidden;
                lblActionParcelDeliver.Visibility = Visibility.Hidden;
                droneInParcelButton.Visibility = Visibility.Hidden;
            }
            //droneInParcelButton.Content = parcel.droneInParcel == null ? "אין עדיין רחפן משוייך" : parcel.droneInParcel.DroneID;

        }

        private void UpdateVisibility() // hidden Button - upgrade and remove
        {
            BorderAddParcel.Visibility = Visibility.Visible;
            btnRemoveParcel.Visibility = Visibility.Hidden;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxOftarget.SelectedIndex != -1 && comboBoxOfsander.SelectedIndex != -1 && weightCombo.SelectedIndex != -1 && prioritiCombo.SelectedIndex != -1)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("? האם אתה בטוח שאתה רוצה להוסיף חבילה זו"
            , "אישור", MessageBoxButton.YesNoCancel);

                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            int id = GetBL.AddNewParcel(parcel, ((CustomerToList)comboBoxOfsander.SelectedItem).CustomerId, ((CustomerToList)comboBoxOftarget.SelectedItem).CustomerId);

                            mainWindow.parcelToLists.Add(GetBL.GetParcelToListsByPredicate(i => i.Id == id).First());
                            MessageBox.Show("החבילה נוספה בהצלחה");

                            mainWindow.GroupingParcelList();
                        }
                        catch (BO.CheckIdException Ex)
                        {
                            throw new CheckIdException("ERORR", Ex);
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
            
            else
            {
                MessageBox.Show("נא למלא את כל השדות");
                return;
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

        private void btnRemoveParcel_Click(object sender, RoutedEventArgs e)
        {
            if (parcelToList.parcelStatus == BO.ParcelStatus.Defined)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("? האם אתה בטוח שאתה רוצה למחוק את חבילה"
           , "", MessageBoxButton.YesNoCancel);

                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            GetBL.RemoveParcelBL(parcel.Id);

                            mainWindow.parcelToLists.Remove(parcelToList);
                            mainWindow.parcelToLists[index] = parcelToList;

                            refreshMainWindow();
                            refreshThisWindow();
                            updateAllData();
                            Close();
                            MessageBox.Show("החבילה נמחקה בהצלחה");
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
            else
            {
                MessageBox.Show("החבילה כבר שוייכה", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

    }
}
