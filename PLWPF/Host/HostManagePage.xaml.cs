﻿using System;
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
        public HostManagePage()
        {
            InitializeComponent();
        }

        private void Exist_Orders(object sender, RoutedEventArgs e)
        {

            view.Navigate(new ExistOrders());
                

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainMenu());
        }
    }
}
