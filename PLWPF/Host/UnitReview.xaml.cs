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
using PLWPF;

namespace PLWPF.Host
{
    /// <summary>
    /// Interaction logic for UnitReview.xaml
    /// </summary>
    public partial class UnitReview : Page
    {
        HostingUnit newUnit = new HostingUnit();
        public UnitReview()
        {
            InitializeComponent();
            grid1.DataContext = newUnit;
            hostingUnitKeyLabel.Content = string.Format("מספר יתווסף בהמשך...");
        }
        public UnitReview(HostingUnit unit)
        {
            InitializeComponent();
            grid1.DataContext = unit;
            areaComboBox.ItemsSource = Enum.GetValues(typeof(BE.Area));
            areaComboBox.Text = unit.Area.ToString();
            hostingTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.HostingType));
            hostingTypeComboBox.Text = unit.HostingType.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //TODO update
        }
    }
}
