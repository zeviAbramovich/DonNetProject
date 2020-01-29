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
using PLWPF.Host;
using PLWPF;
using MaterialDesignThemes.Wpf;

namespace PLWPF.Host
{
    /// <summary>
    /// Interaction logic for UnitsPage.xaml
    /// </summary>
    public partial class UnitsPage : Page
    {
        public static long currentId { get; set; }
        public UnitsPage()
        {
            InitializeComponent();
        }
        public UnitsPage(long key)
        {
            InitializeComponent();
            currentId = key;
            List<HostingUnit> units = BL.FactoryMethode.GetBL().GetAllHostUnits(currentId);
            hostingUnitListView.ItemsSource = units;
            hostingUnitListView.HorizontalContentAlignment = HorizontalAlignment.Center;
            
        }

        private void hostingUnitListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HostingUnit unit = new HostingUnit();
            unit = hostingUnitListView.SelectedItem as HostingUnit;
            this.NavigationService.Navigate(new UnitReview(unit));
        }

        private void addUnit_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new UnitReview());
        }
    }
}
