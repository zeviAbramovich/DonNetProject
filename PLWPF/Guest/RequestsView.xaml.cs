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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BE;

namespace PLWPF.Guest
{
    /// <summary>
    /// Interaction logic for RequestsView.xaml
    /// </summary>
    public partial class RequestsView : Page
    {
        List<GuestRequest> guestRequests = new List<GuestRequest>();
        GuestRequest guest = new GuestRequest();
        public RequestsView()
        {
            InitializeComponent();
        }
        public RequestsView(List<GuestRequest> requests)
        {
            InitializeComponent();
            guestRequests = requests;
            guestRequestListView.ItemsSource = guestRequests;
            
            areaColumn.ItemsSource = Enum.GetValues(typeof(BE.Area));
            gardenCB.ItemsSource = Enum.GetValues(typeof(BE.Requirements));
            childrensAttCB.ItemsSource = Enum.GetValues(typeof(BE.Requirements));
            poolCB.ItemsSource = Enum.GetValues(typeof(BE.Requirements));
            jacuzziCB.ItemsSource = Enum.GetValues(typeof(BE.Requirements));
            hostingTypeColumn.ItemsSource = Enum.GetValues(typeof(BE.HostingType));
            
        }

        //private void guestRequestListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    GuestRequest g = guestRequestListView.SelectedItem as GuestRequest;
        //    this.NavigationService.Navigate(new UpdateRequestView(g));
        //}

        private void guestRequestListView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GuestRequest g = (GuestRequest)guestRequestListView.SelectedItem;
            this.NavigationService.Navigate(new UpdateRequestView(g));
        }
    }
}
