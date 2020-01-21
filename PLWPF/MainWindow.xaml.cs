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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {

            InitializeComponent();
            


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //HostControlMenu a = new HostControlMenu();
            //MenuView.Children.Add(a);
 
        }

        private void Host_Click(object sender, RoutedEventArgs e)
        {

            zevi.Navigate(new HostControlMenu());

        }

        private void BundledTheme_IsActiveChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           

        }

        private void zevi_ContentRendered(object sender, EventArgs e)
        {
            zevi.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;

        }
    }
}
