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

            Frame.Navigate(new MainMenu());

        }





        

        public static T GetParent<T>(DependencyObject child) where T : DependencyObject

        {

            DependencyObject dependencyObject = VisualTreeHelper.GetParent(child);

            if (dependencyObject != null)

            {

                T parent = dependencyObject as T;

                if (parent != null)

                {

                    return parent;

                }

                else

                {

                    return GetParent<T>(dependencyObject);

                }

            }

            else

            {

                return null;

            }

        }
        private void zevi_ContentRendered(object sender, EventArgs e)
        {
            Frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }
    }
}
