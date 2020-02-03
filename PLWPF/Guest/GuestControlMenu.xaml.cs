using PLWPF.Guest;
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
using PLWPF;
using System.Windows.Forms;


namespace PLWPF.Guest
{
    /// <summary>
    /// Interaction logic for GuestControlMenu.xaml
    /// </summary>
    public partial class GuestControlMenu : Page
    {
        public GuestControlMenu()
        {
            InitializeComponent();

        }

       
        private void AddRequest_button(object sender, RoutedEventArgs e)
        {
            view.Navigate(new AddGuestRequest());   
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainMenu());
        }

        private void UpdateRequest_Click(object sender, RoutedEventArgs e)
        {
            view.Navigate(new UpdateRequests());
        }
    }
}
