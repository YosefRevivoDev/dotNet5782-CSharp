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

namespace PL
{

    /// <summary>
    /// Interaction logic for LoginCustomerWindow.xaml
    /// </summary>
    public partial class LoginCustomerWindow : Window
    {
        IBL GetBL;

        /// <summary>
        /// Ctor for update customer
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="_mainWindow"></param>
        /// <param name="_customer"></param>
        /// <param name="_index"></param>
        //public CustomerWindow(IBL bL,int Id)
        //{
        //    InitializeComponent();
        //    GetBL = bL;
        //    GetBL = bL;
        //    customer = bL.GetCustomer(_customer.CustomerId);
        //    DataContext = customer;
        //    UpdateGridVisibility();
        //}


        private void updateCustomer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
