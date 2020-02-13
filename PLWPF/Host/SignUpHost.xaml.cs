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
using MaterialDesignColors.ColorManipulation;
using BE;
using System.Net;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Xml.Linq;
using System.Net.Http;
using System.Xml.Serialization;

namespace PLWPF.Host
{
    /// <summary>
    /// Interaction logic for SignUpHost.xaml
    /// </summary>

    public partial class SignUpHost : Page
    {

        BackgroundWorker bg = new BackgroundWorker();
        HttpWebRequest httpRequest;
        public BE.Host host = new BE.Host();
        XElement branchesXML;
        string branchesPath = @"boi.xml";
        public SignUpHost()
        {
            InitializeComponent();
            bg.DoWork += Branches_Work;
            bg.RunWorkerCompleted += Branches_RunWorkerCompleted;
            grid1.DataContext = host;




        }

        private void Branches_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Branches_Work(object sender, DoWorkEventArgs e)
        {
            try
            {
                branchesXML = XElement.Load(branchesPath);
            }
            catch
            {
                MessageBox.Show("Cant load");
            }
        }

        private void comboBoxBanks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (branchNumberComboBox.ItemsSource != null)
            {
                host.HostBankAccount.BranchNumber = Int32.Parse(branchNumberComboBox.SelectedItem.ToString());

            }
        }

        private void confirmSignUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL.FactoryMethode.GetBL().AddHost(host);
                MessageBox.Show("נרשמת בהצלחה!!");
            }
            catch (CannotAddException cae)
            {
                MessageBox.Show(cae.Message);
            }

            this.NavigationService.Navigate(new HostControlMenu());

        }


    }
}
