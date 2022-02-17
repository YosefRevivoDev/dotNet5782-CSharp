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
using BlApi;
using BO;
using PLGui;

namespace PL
{

    /// <summary>
    /// Interaction logic for LoginCustomerWindow.xaml
    /// </summary>
    public partial class LoginCustomerWindow : Window
    {
        IBL GetBL;
        private Customer customer;
        private MainWindow mainWindow;
        int ID;
        /// <summary>
        /// Ctor for update customer
        /// </summary>
        /// <param name = "bL" ></ param >
        /// < param name="Id"></param>

        public LoginCustomerWindow(IBL bL, MainWindow _mainWindow,int Id)
        {
            InitializeComponent();
            GetBL = bL;
            ID = Id;
            mainWindow = _mainWindow;
            customer = new();
            customer.LocationCustomer = new();
            try
            {
                customer = bL.GetCustomer(Id);
                fromCustomer.ItemsSource = customer.PackagesFromCustomer.ToList();
                toCustomer.ItemsSource = customer.PackagesToCustomer.ToList();
                DataContext = customer;

            }
            catch (BO.CheckIfIdNotExceptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.CheckIdException ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void updateCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (customer.NameCustomer != default && customer.PhoneCustomer != default)
                {
                    MessageBoxResult Result = MessageBox.Show("האם אתה בטוח שאתה רוצה לעדכן את הלקוח?"
                                     , "הכנס לקוח", MessageBoxButton.YesNoCancel);

                    switch (Result)
                    {
                        case MessageBoxResult.Yes:
                            GetBL.UpdateCustomr(customer.CustomerId, customer.NameCustomer, customer.PhoneCustomer);

                            MessageBox.Show("הלקוח עודכן בהצלחה");
                            break;
                        case MessageBoxResult.No:
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                        default:
                            break;
                    }
                }
                else
                    MessageBox.Show("נא השלם את השדות החסרים", "אישור");
            }
            catch (BO.CheckIfIdNotExceptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.CheckIdException ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
        }

        private void fromCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int parcel = ((ParcelAtCustomer)fromCustomer.SelectedItem).Id;
            new CustomerInParcelWindow(GetBL, mainWindow, parcel).Show();
        }

        private void toCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int parcelID = ((ParcelAtCustomer)toCustomer.SelectedItem).Id;
            new CustomerInParcelWindow(GetBL, mainWindow, parcelID).Show();
        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            var x = new ParcelWindow(GetBL, mainWindow);

            //var currentId = GetBL.GetCustomerToList(x => x.CustomerId == txtUserIdCustomer.Text);
            // var currentId = selectIdSender(GetBL.GetCustomerToList().ToList(),int.Parse( mainWindow.txtUserIdCustomer.Text));
            x.comboBoxOfsander.SelectedItem = (GetBL.GetCustomerToList(x => x.CustomerId==ID).ToList(), int.Parse(mainWindow.txtUserIdCustomer.Text));
            //x.comboBoxOfsander.IsEnabled = false;
            x.BorderAddParcel.Visibility = Visibility.Visible;
            x.GridUpdateParcel.Visibility = Visibility.Hidden;
            x.Height = 500;
            x.Width = 400;
            x.ShowDialog();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
