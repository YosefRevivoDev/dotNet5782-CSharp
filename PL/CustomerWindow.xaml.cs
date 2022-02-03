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
        public CustomerToList customerToList;
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
            customer = new Customer();
            customer = getBl.GetCustomer(customer.CustomerId);
            DataContext = customer;
            CustomrtGrid.VerticalAlignment = VerticalAlignment.Center;
            CustomrtGrid.HorizontalAlignment = HorizontalAlignment.Center;
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
        public CustomerWindow(IBL bL, MainWindow _mainWindow, CustomerToList _customer, int _index)
        {
            InitializeComponent();
            CustomrtGrid.Visibility = Visibility.Visible;
            indexCustomer = _index;
            GetBL = bL;
            mainWindow = _mainWindow;
            customer = bL.GetCustomer(_customer.CustomerId);
            DataContext = customer;
            UpdateGridVisibility();
        }


        private void UpdateGridVisibility()
        {
            btnaddCustomer.Visibility = Visibility.Hidden;
        }

        private void UpdateVisibility() // hidden Button - upgrade and remove
        {
            CustomerID.IsReadOnly = false;
            btnRemoveCustomer.Visibility = Visibility.Hidden;
            btnupdateCustomer.Visibility = Visibility.Hidden;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void updateCustomer_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("האם אתה בטוח שאתה רוצה לעדכן את תחנה?"
             , "הכנס תחנה", MessageBoxButton.YesNoCancel);

            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        GetBL.UpdateCustomr(customer.CustomerId, customer.NameCustomer, customer.PhoneCustomer);
                        customerToList.NameCustomer = customer.NameCustomer;
                        mainWindow.customerToLists[index] = customerToList;
                        mainWindow.lstBaseStationListView.Items.Refresh();
                        MessageBox.Show(customerToList.ToString(), "התחנה עודכנה בהצלחה");
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

        private void addCustomer_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("? האם אתה בטוח שאתה רוצה להוסיף לקוח זה"
              , "הכנס תחנה", MessageBoxButton.YesNoCancel);

            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        GetBL.AddNewCustomer(customer);
                        mainWindow.customerToLists.Add(GetBL.GetCustomerToList()
                            .First(i => i.CustomerId == customer.CustomerId));
                        MessageBox.Show(customer.ToString(), "הלקוח נוסף בהצלחה");
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
