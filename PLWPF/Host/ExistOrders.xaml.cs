using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BE;
using PLWPF.Host;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for ExistOrders.xaml
    /// </summary>
    public partial class ExistOrders : Page
    {
        public long currentId { get; set; }
        List<Order> orders = new List<Order>();
        public ExistOrders()
        {
            InitializeComponent();

            orderListView.ItemsSource = orders;
           
        }

        public ExistOrders(long id)
        {
            InitializeComponent();
            currentId = id;
            orders=BL.FactoryMethode.GetBL().GetAllHostOrders(currentId);
        }
        /// <summary>
        /// catch the current order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var item = sender as System.Windows.Controls.ListViewItem;
            if (item != null && item.IsSelected)
            {
                this.NavigationService.Navigate(new OrderReview(orders[orderListView.SelectedIndex]));
            }
        }
    }
}
