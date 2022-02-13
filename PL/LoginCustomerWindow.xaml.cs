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

namespace PL
{

    /// <summary>
    /// Interaction logic for LoginCustomerWindow.xaml
    /// </summary>
    public partial class LoginCustomerWindow : Window
    {
        IBL GetBL;
        private Customer customer;

        /// <summary>
        /// Ctor for update customer
        /// </summary>
        /// <param name = "bL" ></ param >
        /// < param name="_mainWindow"></param>
        /// <param name = "_customer" ></ param >
        /// < param name="_index"></param>
        public LoginCustomerWindow(IBL bL, int Id)
        {
            InitializeComponent();
            GetBL = bL;
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
                    MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה בטוח שאתה רוצה לעדכן את הלקוח?"
                                     , "הכנס לקוח", MessageBoxButton.OKCancel);

                    switch (messageBoxResult)
                    {
                        case MessageBoxResult.Yes:

                            GetBL.UpdateCustomr(customer.CustomerId, customer.NameCustomer, customer.PhoneCustomer);

                            MessageBox.Show("הלקוח עודכן בהצלחה");
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
            new CustomerInParcelWindow(GetBL, parcel).Show();
        }

        private void toCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int parcelID = ((ParcelAtCustomer)toCustomer.SelectedItem).Id;
            new CustomerInParcelWindow(GetBL, parcelID).Show();
        }
    }
}
