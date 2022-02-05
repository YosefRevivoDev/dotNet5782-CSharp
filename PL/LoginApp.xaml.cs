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
            if (UserName.Text == "" || PasswordBox1.Text == "")
            {
                MessageBox.Show("You must fill all the fildes");
                return;
            }
            try
            {
                User user = getBL.GetUser(UserName.Text);
                if (user.UserName == UserName.Text && user.Password == PasswordBox1.Text)
                {
                    this.Close();
                    mainWindow.CompanyManagement.Visibility = Visibility.Visible;
                    EnterTheApp.Visibility = Visibility.Hidden;
                    mainWindow.LoginButtons.Visibility = Visibility.Collapsed;
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
    }
}
