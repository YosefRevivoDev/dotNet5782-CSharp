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
using BO;


namespace PL
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BL getBL;
        public MainWindow()
        {
            InitializeComponent();
            getBL = new BL();
        }
            
        private void btnListDrone_Click(object sender, RoutedEventArgs e)
        {
           new ShowDronesWindow(getBL).Show();
        }

        private void btnEnterApp_Click(object sender, RoutedEventArgs e)
        {
            if (UserName.Text == "" || Password.Text == "")
            {
                MessageBox.Show("You must fill all the fildes");
                return;
            }
            try
            {
                User user = getBL.GetUser(UserName.Text);
                if (user.UserName == UserName.Text && user.Password == Password.Text)
                {
                    CompanyManagement.Visibility = Visibility.Visible;
                    EnterTheApp.Visibility = Visibility.Hidden;
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
