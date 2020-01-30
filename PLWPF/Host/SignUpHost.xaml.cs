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

namespace PLWPF.Host
{
    /// <summary>
    /// Interaction logic for SignUpHost.xaml
    /// </summary>
    
    public partial class SignUpHost : Page
    {
        public BE.Host host=new BE.Host();
        public SignUpHost()
        {
            InitializeComponent();
            grid1.DataContext = host;
        }

        private void confirmSignUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL.FactoryMethode.GetBL().AddHost(host);

            }
            catch (CannotAddException cae)
            {
                MessageBox.Show(cae.Message);
            }
            MessageBox.Show("נרשמת בהצלחה!!");
            this.NavigationService.Navigate(new HostControlMenu());
            
        }
    }
}
