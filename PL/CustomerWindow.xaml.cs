using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BlApi;
using BO;

namespace PLGui
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary> 
    public partial class CustomerWindow : Window
    {
        IBL GetBL;
        public ObservableCollection<CustomerToList> customerToLists;
        public CustomerToList customerToList;
        ParcelToList parcelToList { set; get; }
        public Customer customer { set; get; }
        public int index;
        private MainWindow mainWindow;

        /// <summary>
        /// Ctor for add customr
        /// </summary>
        /// <param name="getBl"></param>
        /// <param name="_mainWindow"></param>
        public CustomerWindow(IBL getBl, MainWindow _mainWindow)
        {
            InitializeComponent();
            GetBL = getBl;
            customer = new Customer() { LocationCustomer = new Location() };
            DataContext = customer;
            mainWindow = _mainWindow;
            UpdateVisibility();
        }


        public int indexCustomer;

        /// <summary>
        /// Ctor for update customer
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="_mainWindow"></param>
        /// <param name="_customer"></param>
        /// <param name="_index"></param>
        public CustomerWindow(IBL bL, MainWindow _mainWindow, CustomerToList customerToList, int index)
        {
            InitializeComponent();
            indexCustomer = index;
            GetBL = bL;
            mainWindow = _mainWindow;
            this.customerToList = customerToList;
            customer = bL.GetCustomer(customerToList.CustomerId);
            DataContext = customer;
            Updating.Visibility = Visibility.Visible;
            ItemsSourceParcels();
            UpdateGridVisibility();
        }

        private void ItemsSourceParcels()
        {
            fromCustomer.ItemsSource = customer.PackagesFromCustomer.ToList();
            toCustomer.ItemsSource = customer.PackagesToCustomer.ToList();
        }
        private void UpdatingWindow(int id)
        {
            GridUpdateCustomer.Visibility = Visibility.Visible;
            GridAddCustomer.Visibility = Visibility.Hidden;
            try
            {
                customer = GetBL.GetCustomer(id);
            }
            catch (BO.CheckIfIdNotExceptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                return;
            }
            DataContext = customer;
        }

        private void UpdateGridVisibility()
        {
            btnAddCustomer.Visibility = Visibility.Hidden;
        }

        private void UpdateVisibility() // hidden Button - upgrade and remove
        {
            btnUpdateCustomer.Visibility = Visibility.Hidden;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void updateCustomer_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה בטוח שאתה רוצה לעדכן את הלקוח?"
             , "הכנס לקוח", MessageBoxButton.YesNoCancel);

            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        GetBL.UpdateCustomr(customer.CustomerId, customer.NameCustomer, customer.PhoneCustomer);
                        customerToList = GetBL.GetCustomerToList(i => i.CustomerId == customer.CustomerId).First();
                        mainWindow.customerToLists[indexCustomer] = customerToList;
                        mainWindow.lstBaseStationListView.Items.Refresh();
                        MessageBox.Show("הלקוח עודכן בהצלחה");
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

        private void RemoveCustomer_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("? האם אתה בטוח שאתה רוצה למחוק תחנה זאת"
               , "", MessageBoxButton.YesNoCancel);

            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        GetBL.RemoveCustomerBL(customer.CustomerId);
                        mainWindow.customerToLists.Remove(customerToList);
                        mainWindow.customerToLists[index] = customerToList;
                        mainWindow.lstCustomerListView.Items.Refresh();
                        MessageBox.Show(customer.ToString(), "התחנה נמחקה בהצלחה");
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

        private void CustomerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            customerToList = (CustomerToList)mainWindow.lstCustomerListView.SelectedItem;
            if (customerToList != null)
            {
                UpdatingWindow(customerToList.CustomerId);
            }
        }

        private void fromCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int parcel = ((ParcelAtCustomer)fromCustomer.SelectedItem).Id;
            new ParcelWindow(GetBL, mainWindow, parcel).Show();
            Close();
        }

        private void toCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int parcelID = ((ParcelAtCustomer)toCustomer.SelectedItem).Id;
            new ParcelWindow(GetBL, mainWindow, parcelID).Show();
            Close();
        }     

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("? האם אתה בטוח שאתה רוצה להוסיף לקוח זה"
             , "הכנס תחנה", MessageBoxButton.YesNoCancel);

            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        GetBL.AddNewCustomer(customer);

                        CustomerToList customerToList = GetBL.GetCustomerToList()
                            .First(i => i.CustomerId == customer.CustomerId);

                        mainWindow.customerToLists[index] = customerToList;

                        mainWindow.customerToLists.Add(customerToList);
                        MessageBox.Show("הלקוח נוסף בהצלחה");
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
    }
}
