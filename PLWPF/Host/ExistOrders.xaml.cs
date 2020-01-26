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
            orderListView.ItemsSource = orders;

        }
        /// <summary>
        /// catch the current order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void orderListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var order = orderListView.SelectedItem as Order;
            OrderReview review = new OrderReview(order);
            this.NavigationService.Navigate(review);
        }
    }
}
