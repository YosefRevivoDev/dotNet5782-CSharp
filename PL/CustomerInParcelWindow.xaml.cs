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
    /// Interaction logic for CustomerInParcek.xaml
    /// </summary>
    public partial class CustomerInParcelWindow : Window
    {
        private IBL GetBL;

        private Parcel parcel;
        private ParcelToList parcelToList;

        public CustomerInParcelWindow(IBL bL, int id)
        {
            InitializeComponent();
            GetBL = bL;
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

        }
    }
}
