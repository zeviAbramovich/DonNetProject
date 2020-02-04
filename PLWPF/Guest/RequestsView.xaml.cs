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
        public RequestsView()
        {
            InitializeComponent();
        }
        public RequestsView(List<GuestRequest> requests)
        {
            InitializeComponent();
            guestRequestListView.ItemsSource = requests;
            guestRequests = requests;
            areaColumn.ItemsSource = Enum.GetValues(typeof(BE.Area));
            gardenCB.ItemsSource = Enum.GetValues(typeof(BE.Requirements));
            childrensAttCB.ItemsSource = Enum.GetValues(typeof(BE.Requirements));
            poolCB.ItemsSource = Enum.GetValues(typeof(BE.Requirements));
            jacuzziCB.ItemsSource = Enum.GetValues(typeof(BE.Requirements));
            hostingTypeColumn.ItemsSource = Enum.GetValues(typeof(BE.HostingType));
            SetBlackOutDates();
        }


        private void SetBlackOutDates()
        {
            foreach (var item in guestRequests)
            {
                Calendar.BlackoutDates.Add(new CalendarDateRange(item.EntryDate, item.ReleaseDate));
               
            }

        }

        private void CloseCalendar_Click(object sender, RoutedEventArgs e)
        {
            Calendar.BlackoutDates.Clear();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            //TODO not working
            GuestRequest guest = guestRequestListView.SelectedCells as GuestRequest;
            guest.EntryDate = Calendar.SelectedDates.First();
            guest.ReleaseDate = Calendar.SelectedDates.Last();
            SetBlackOutDates();
        }
    }
}
