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
namespace PLWPF.Admin
{
    /// <summary>
    /// Interaction logic for AdminDashBoard.xaml
    /// </summary>
    public partial class AdminDashBoard : Page
    {
        public AdminDashBoard()
        {
            InitializeComponent();
            numOfHosting.Value = BL.FactoryMethode.GetBL().GetAllHostingUnit().Count;
            numOfOrders.Value = BL.FactoryMethode.GetBL().GetAllOrders().Count;
            NumOfRequest.Value = BL.FactoryMethode.GetBL().GetAllGuestRequest().Count;

            List<BE.Host> hosts = BL.FactoryMethode.GetBL().GetAllHosts().OrderByDescending(x => x.numOfUnits).ToList();
           // highestUnits.Value = hosts.FirstOrDefault().numOfUnits;
           //var a = BL.FactoryMethode.GetBL().GetAllHostByNumHostingUnit(BL.FactoryMethode.GetBL().GetAllHosts());
           // highestUnits.Value = a.FirstOrDefault().Key;
           // var b = (BE.Host)a.FirstOrDefault().First();
           // numHighestUnitsText.Text = b.FamilyName;

            // HostingUnit unit = (HostingUnit)BL.FactoryMethode.GetBL().GetAllHostByNumOrders(BL.FactoryMethode.GetBL().GetAllHostingUnit()).FirstOrDefault().First();
            //  highstOrders.Value= BL.FactoryMethode.GetBL().GetAllHostByNumOrders(BL.FactoryMethode.GetBL().GetAllHostingUnit()).FirstOrDefault().Key;
            // numHighestOrdersText.Text = unit.HostingUnitName;

        }

        private void AllDataButton_Checked(object sender, RoutedEventArgs e)
        {
            HostingData.Visibility = Visibility.Collapsed;
            AllData.Visibility = Visibility.Visible;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            AllData.Visibility = Visibility.Collapsed;
            HostingData.Visibility = Visibility.Visible; 
        }
    }
}
