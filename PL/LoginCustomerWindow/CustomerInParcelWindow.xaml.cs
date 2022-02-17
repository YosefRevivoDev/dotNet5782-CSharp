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
using BlApi;
using BO;
using PLGui;

namespace PL
{

    /// <summary>
    /// Interaction logic for CustomerInParcek.xaml
    /// </summary>
    public partial class CustomerInParcelWindow : Window
    {
        private IBL GetBL;

        private Parcel parcel;
        private ParcelToList parcelToList;
        private MainWindow mainWindow;
        private DroneToList droneToList;
        int Id;
        private int index;

        public CustomerInParcelWindow(IBL bL, MainWindow _mainWindow, int id)
        {
            InitializeComponent();
            GetBL = bL;
            mainWindow = _mainWindow;
            Id = id;
            try
            {
                parcelToList = GetBL.GetParcelToListsByPredicate(i => i.Id == id).First();
                parcel = GetBL.GetParcel(id); DataContext = parcel;
                DataContext = parcel;
                switch (parcelToList.parcelStatus)
                {
                    case BO.ParcelStatus.Defined:
                        txtScheduled.Text = "לא שוייך";
                        txtPickedUp.Text = "לא נאסף";
                        txtDelivered.Text = "לא סופק";
                        btnDroneInParcelButton.Content = "החבילה לא שויכה לרחפן";
                        break;
                    case BO.ParcelStatus.associated:
                        txtPickedUp.Text = "לא נאסף";
                        txtDelivered.Text = "לא סופק";
                        break;
                    case BO.ParcelStatus.collected:
                        txtDelivered.Text = "לא סופק";
                        break;
                    default:
                        break;
                }
            }
            catch (BO.CheckIfIdNotExceptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                MessageBoxResult.None, MessageBoxOptions.RightAlign);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void droneInParcelButton_Click_1(object sender, RoutedEventArgs e)
        {
            //if (parcel.droneInParcel != null)
            //{
            //    int idDrone = parcel.droneInParcel.DroneID;
            //    new DroneWindow(GetBL, mainWindow, idDrone).Show();
            //    Close();
            //}

        }

        private void btnAssignParcel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRemoveParcelCustomer_Click(object sender, RoutedEventArgs e)
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
                            mainWindow.lstParcelListView.Items.Refresh();
                            
                            MessageBox.Show("החבילה נמחקה בהצלחה");
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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
