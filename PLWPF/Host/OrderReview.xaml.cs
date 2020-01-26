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
using BE;

namespace PLWPF.Host
{
    /// <summary>
    /// Interaction logic for OrderReview.xaml
    /// </summary>
    public partial class OrderReview : Page
    {
        Order viewOrder = new Order();
        public OrderReview()
        {
            InitializeComponent();
            grid1.DataContext = viewOrder;
            
        }
        public OrderReview(Order order)
        {
            
            InitializeComponent();
            viewOrder = BL.FactoryMethode.GetBL().GetOrder(order.OrderKey);
            grid1.DataContext = viewOrder;
            statusComboBox.ItemsSource = Enum.GetValues(typeof(BE.StatusOrder));
            statusComboBox.Text = order.Status.ToString();
            

        }

        //private void statusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    confirm.IsEnabled = true;

        //}

        //private void confirm_Click(object sender, RoutedEventArgs e)
        //{
        //    viewOrder.Status = (BE.StatusOrder)statusComboBox.SelectedValue;
        //    try
        //    {
        //        BL.FactoryMethode.GetBL().UpdateOrder(viewOrder);
        //    }
        //    catch (CannotUpdateException cue)
        //    {
        //        System.Windows.Forms.MessageBox.Show(cue.Message);
        //        this.NavigationService.Navigate(new ExistOrders(1));
        //    }
        //}
    }
}
