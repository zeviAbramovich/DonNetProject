﻿using PLWPF.Host;
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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for HostManagePage.xaml
    /// </summary>
    public partial class HostManagePage : Page
    {
        public long currentId { get; set; }
        public static BE.Host host = new BE.Host();
        public HostManagePage()
        {
            InitializeComponent();
        }

        public HostManagePage(long hostId)
        {
            InitializeComponent();
            currentId = hostId;
            host = BL.FactoryMethode.GetBL().GetHost(currentId);
            privateName.Text = string.Format("שלום " +host.PrivateName+"!");
        }
        private void Exist_Orders(object sender, RoutedEventArgs e)
        {
            view.Navigate(new ExistOrders(currentId));                
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainMenu());
        }

        private void Exist_Units(object sender, RoutedEventArgs e)
        {
            view.NavigationService.Navigate(new UnitsPage(currentId));
        }
    }
}
