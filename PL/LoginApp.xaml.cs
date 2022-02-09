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

namespace PLGui
{
    /// <summary>
    /// Interaction logic for LoginApp.xaml
    /// </summary>
    public partial class LoginApp : Window
    {
        MainWindow mainWindow;
        private IBL getBL;

        public LoginApp(MainWindow _mainWindow, IBL bL)
        {
            getBL = bL;
            InitializeComponent();
            mainWindow = _mainWindow;
        }

        private void btnEnterApp_Click(object sender, RoutedEventArgs e)
        {
            if (UserId.Text == "" || PasswordBox1.Text == "")
            {
                MessageBox.Show("You must fill all the fildes");
                return;
            }
            try
            {
                User user = getBL.GetUser(UserId.Text);
                if (user.UserId == UserId.Text && user.Password == PasswordBox1.Text)
                {
                    mainWindow.CompanyManagement.Visibility = Visibility.Visible;
                    EnterApp.Visibility = Visibility.Hidden;
                    mainWindow.LoginManagement.Visibility = Visibility.Collapsed;
                    this.Close();
                    
                }
                else
                {
                    MessageBox.Show("The Password not correct");

                }
            }
            catch (Exception)
            {
                MessageBox.Show("The User not exist");
                return;
            }
        }

        private void btnCustomerEnter_Click(object sender, RoutedEventArgs e)
        {
            List<CustomerToList> customers = getBL.GetCustomerToList().ToList();
            var customerCombo = from item in customers
                                select item.CustomerId;
            int find = customerCombo.FirstOrDefault(i => i == int.Parse(UserId.Text.ToString()));
            if (find != default)
            {
                int IdCustomer = int.Parse(find.ToString());
                //new CustomerWindow(getBL, IdCustomer).Show();
            }
            else
                MessageBox.Show("מספר המשתמש אינו קיים\n אנא נסה שוב", "אישור");
        }
        private void RegisterApp_Clice(object sender, RoutedEventArgs e)
        {

        }
    }
}
