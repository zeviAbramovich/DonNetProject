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
using BL;
using PLWPF.Guest;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for ButtonMain.xaml
    /// </summary>
    public partial class MainMenu : Page
    {

        public MainMenu()
        {
            InitializeComponent();
        }

        private void Host_Button(object sender, RoutedEventArgs e)
        {
            view.Navigate(new HostControlMenu());

            //Grid.SetColumn(host, 1);
        }

        private void Guest_Click(object sender, RoutedEventArgs e)
        {

            //Page guest = new GuestControlMenu();
            //full_screen.Refresh();
            //full_screen.Navigate(guest);
            NavigationService service = NavigationService.GetNavigationService(this);
            service.Navigate(new GuestControlMenu());
        }
    }
}
