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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for HostControlMenu.xaml
    /// </summary>
    public partial class HostControlMenu : Page
    {
        public HostControlMenu()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            // Do not load your data at design time.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Load your data here and assign the result to the CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            long id = 0;
            if (!long.TryParse(hostIdTextBox.Text, out id))
            {
                MessageBox.Show("ERROR! you must insert ID and Password");
                return;
            }
            HostingUnit a = new HostingUnit();
            List<HostingUnit> list = BL.FactoryMethode.GetBL().GetAllHostingUnit();
            a=list.Find(x=>x.Owner.HostId==id);
            if (a == null || a.Owner.Password != passwordTextBox.Password)
            {
                MessageBox.Show("Wrong ID or Password");
                hostIdTextBox.Clear();
            }
            
            NavigationService login = NavigationService.GetNavigationService(this);
            login.Navigate(new MainMenu());
            login.Navigate(new HostManagePage());
            

        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
            //NavigationService back = NavigationService.GetNavigationService(this);
            //back.Navigate()
        }
    }
}
