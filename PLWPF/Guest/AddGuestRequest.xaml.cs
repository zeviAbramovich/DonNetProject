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
using Utilities;

namespace PLWPF.Guest
{
    /// <summary>
    /// Interaction logic for AddGuestRequest.xaml
    /// </summary>
    public partial class AddGuestRequest : Page
    {
        GuestRequest request = new GuestRequest();
        public AddGuestRequest()
        {
            InitializeComponent();
            entryDateDatePicker.SelectedDate = DateTime.Now;
            releaseDateDatePicker.SelectedDate = DateTime.Now.AddDays(1);
            grid1.DataContext = request;
            for (int i = 0; i < 20; i++)
            {
                adultsComboBox.Items.Add(i);
                childrenComboBox.Items.Add(i);
            }

            entryDateDatePicker.DisplayDateStart = DateTime.Now.AddMonths(-1);
            entryDateDatePicker.DisplayDateEnd = DateTime.Now.AddMonths(11);
            releaseDateDatePicker.DisplayDateStart = request.EntryDate.AddDays(1);
            releaseDateDatePicker.DisplayDateEnd = DateTime.Now.AddMonths(+11);
            areaComboBox.ItemsSource = Enum.GetValues(typeof(BE.Area));
            gardenComboBox.ItemsSource = Enum.GetValues(typeof(BE.Requirements));
            childrensAttractionsComboBox.ItemsSource = Enum.GetValues(typeof(BE.Requirements));
            poolComboBox.ItemsSource = Enum.GetValues(typeof(BE.Requirements));
            jacuzziComboBox.ItemsSource = Enum.GetValues(typeof(BE.Requirements));
            hostingTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.HostingType));

        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                BL.FactoryMethode.GetBL().AddRequest(request);
                MessageBox.Show("נוסף בהצלחה!!");

            }
            catch (CannotAddException cae)
            {

                MessageBox.Show(cae.Message);
            }
        }

        private void mailAddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool result = Utilities.Tools.ValidateMail(mailAddressTextBox.Text);
        }

        private void entryDateDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            releaseDateDatePicker.DisplayDateStart = entryDateDatePicker.SelectedDate;
        }



        private void mailAddressTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (((TextBox)sender).Name == "privateNameTextBox")
            {
                if (!privateNameTextBox.Text.All(Char.IsLetter))
                {
                    privateNameTextBox.BorderBrush = Brushes.Red;
                }
                else
                    privateNameTextBox.BorderBrush = Brushes.Gray;


            }
            else if (((TextBox)sender).Name == "mailAddressTextBox")
            {
                if (!Utilities.Tools.ValidateMail(mailAddressTextBox.Text))
                {
                    mailAddressTextBox.BorderBrush = Brushes.Red;
                    confirm.IsEnabled = false;
                }
                else
                {
                    mailAddressTextBox.BorderBrush = Brushes.Gray;
                    confirm.IsEnabled = true;
                }
            }
            else if (((TextBox)sender).Name == "familyNameTextBox")
            {
                if (!familyNameTextBox.Text.All(Char.IsLetter))
                {
                    familyNameTextBox.BorderBrush = Brushes.Red;
                }
                else
                    familyNameTextBox.BorderBrush = Brushes.Gray;
            }
        }

        private void adultsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)adultsComboBox.SelectedItem.ToString() == "0")
                adultsComboBox.BorderBrush = Brushes.Red;
            adultsComboBox.BorderBrush = Brushes.Gray;
        }
    }
}
