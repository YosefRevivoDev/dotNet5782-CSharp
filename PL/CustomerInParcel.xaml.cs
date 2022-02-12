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
    /// Interaction logic for CustomerInParcek.xaml
    /// </summary>
    public partial class CustomerInParcel : Window
    {
        IBL GetBL;

        private Parcel parcel;
        private ParcelToList parcelToList;

        //public CustomerInParcel(IBL bL, int id)
        //{
        //    InitializeComponent();
        //    GetBL = bL;
        //    try
        //    {
        //        parcelToList = GetBL.GetParcelToListsByPredicate(i => i.Id == id).First();
        //        parcel = GetBL.GetParcel(id); DataContext = parcel;
        //        DataContext = parcel;
        //        switch (parcelToList.parcelStatus)
        //        {
        //            case BO.ParcelStatus.Defined:
        //                ScheduledTextBox.Text = "לא שוייך";
        //                PickedUpTextBox.Text = "לא נאסף";
        //                DeliveredTextBox.Text = "לא סופק";
        //                droneInParcelButton.Text = "החבילה לא שויכה לרחפן";
        //                break;
        //            case BO.ParcelStatus.associated:
        //                PickedUpTextBox.Text = "לא נאסף";
        //                DeliveredTextBox.Text = "לא סופק";
        //                break;
        //            case BO.ParcelStatus.collected:
        //                deliv.Text = "לא סופק";
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (BO.CheckIfIdNotException ex)
        //    {
        //        MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
        //        MessageBoxResult.None, MessageBoxOptions.RightAlign);
        //    }
        //}

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void droneInParcelButton_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
