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
using MaterialDesignThemes.Wpf.Transitions;
using BE;
using PLWPF;
using MaterialDesignThemes.Wpf;

namespace PLWPF.Host
{
    /// <summary>
    /// Interaction logic for UnitReview.xaml
    /// </summary>
    /// 
    public partial class UnitReview : Page
    {
        HostingUnit newUnit = new HostingUnit();
        BE.Host host=new BE.Host();
        public UnitReview()
        {
            InitializeComponent();
            try
            {
                host = BL.FactoryMethode.GetBL().GetHost(UnitsPage.currentId);
            }
            catch (MissingMemberException mme)
            {

                MessageBox.Show(mme.Message);
            }
            newUnit.Owner = host;
            grid1.DataContext = newUnit;
            hostingUnitKeyLabel.Content = string.Format("מספר יתווסף בהמשך...");
            areaComboBox.ItemsSource = Enum.GetValues(typeof(BE.Area));
            hostingTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.HostingType));
        }
        public UnitReview(HostingUnit unit)
        {
            InitializeComponent();
            newUnit = unit;
            grid1.DataContext = newUnit;
            areaComboBox.ItemsSource = Enum.GetValues(typeof(BE.Area));
            areaComboBox.Text = newUnit.Area.ToString();
            hostingTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.HostingType));
            hostingTypeComboBox.Text = newUnit.HostingType.ToString();
            SetBlackOutDates();
            
        }

        private void hostingTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            newUnit.HostingType = (BE.HostingType)Enum.Parse(typeof(BE.HostingType), hostingTypeComboBox.SelectedItem.ToString());
        }


        private void AreaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            newUnit.Area = (BE.Area)Enum.Parse(typeof(BE.Area), areaComboBox.SelectedItem.ToString());
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {

  
            try
            {
                BL.FactoryMethode.GetBL().AddHostingUnit(newUnit);
                grid1.DataContext = newUnit;
                
                this.NavigationService.Navigate(new UnitsPage(UnitsPage.currentId));
            }
            catch (CannotAddException cae)
            {
                MessageBox.Show(cae.Message);
            }
            catch (CannotUpdateException cue)
            {
                MessageBox.Show(cue.Message);
            }
        }

        private void SetBlackOutDates()
        {
            foreach (DateTime date in newUnit.bookDates)
            {
                Calendar.BlackoutDates.Add(new CalendarDateRange(date));
            }
        }
        public void CalendarDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            SetBlackOutDates();
        }

        private void deleteUnit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL.FactoryMethode.GetBL().DeleteHostingUnit(newUnit.HostingUnitKey);
                this.NavigationService.Navigate(new UnitsPage(UnitsPage.currentId));
            }
            catch (CannotDeleteException cde)
            {
                MessageBox.Show(cde.Message);
            }
        }

        //private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    newUnit.bookDates.Add(Calendar.SelectedDates.);
        //}
    }
}
