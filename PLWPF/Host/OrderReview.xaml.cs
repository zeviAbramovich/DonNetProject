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
using System.Windows.Shapes;
using BE;
using System.Net.Mail;
using System.ComponentModel;

namespace PLWPF.Host
{
    /// <summary>
    /// Interaction logic for OrderReview.xaml
    /// </summary>
    public partial class OrderReview : Page
    {
        BackgroundWorker worker = new BackgroundWorker();
        Order viewOrder = new Order();
        Order old = new Order();
        public OrderReview()
        {
            InitializeComponent();

            grid1.DataContext = viewOrder;
        }
        public OrderReview(Order order)
        {
            InitializeComponent();
            worker.DoWork += Mail_DoWork;
            worker.RunWorkerCompleted += Mail_RunWorkerCompleted;
            viewOrder = BL.FactoryMethode.GetBL().GetOrder(order.OrderKey);
            grid1.DataContext = viewOrder;
            //statusComboBox.ItemsSource = Enum.GetValues(typeof(BE.StatusOrder));
            statusComboBox.ItemsSource = Enum.GetValues(typeof(BE.StatusOrder));
          //  statusComboBox.DisplayMemberPath = "Key";
          //  statusComboBox.SelectedValuePath = "Value";
            //statusComboBox.Text = order.Status.ToString();
            old = order;
            confirmation.IsEnabled = false;
            
        }

        private void Mail_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show("ERROR " + e.Error.Message);
            viewOrder.OrderDate = DateTime.Now;
            System.Windows.Forms.MessageBox.Show("mail sent");
        }

        private void Mail_DoWork(object sender, DoWorkEventArgs e)
        {
            viewOrder = (Order)e.Argument;
            GuestRequest cliant = BL.FactoryMethode.GetBL().GetGuestRequest(viewOrder.GuestRequestKey);
            HostingUnit unit = BL.FactoryMethode.GetBL().GetUnit(viewOrder.HostingUnitKey);
            MailMessage mail = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("gooking69770@gmail.com");
            mail.To.Add(cliant.MailAddress);
            mail.Subject = "We have good news for you!!";
            mail.Body = string.Format("Hello {0}!\nwe have new offer for your vecation\n{1}\n Please contact the owner for continue your reservation.", cliant.PrivateName, unit.ToString());

            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential("gooking69770", "Zevi1234");
            smtpClient.EnableSsl = true;

            smtpClient.Send(mail);
            
        }

        private List<KeyValuePair<string,StatusOrder>> GetStatusOrderDataSource()
        {
            var data = new List<KeyValuePair<string, StatusOrder>>();

            foreach (var item in Enum.GetValues(typeof(StatusOrder)))
            {
                var keyValue = new KeyValuePair<string, StatusOrder>(item.ToString(), (StatusOrder)item);
                data.Add(keyValue);
            }
           
            return data;
        }

        private void confirmation_Click(object sender, RoutedEventArgs e)
        {
            if ((StatusOrder)statusComboBox.SelectedItem == StatusOrder.MailSent&&old.Status!=StatusOrder.MailSent)
            {
               
                viewOrder.Status = (BE.StatusOrder)Enum.Parse(typeof(BE.StatusOrder), statusComboBox.SelectedItem.ToString());
                old.Status = StatusOrder.MailSent;
                try
                {
                    BL.FactoryMethode.GetBL().UpdateOrder(viewOrder);

                    worker.RunWorkerAsync(viewOrder);
                    
                    this.NavigationService.Navigate(new ExistOrders(ExistOrders.currentId));
                }
                catch (CannotUpdateException cue)
                {
                    System.Windows.Forms.MessageBox.Show(cue.Message, "Error!", System.Windows.Forms.MessageBoxButtons.OK);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            { 
                viewOrder.Status = (BE.StatusOrder)Enum.Parse(typeof(BE.StatusOrder), statusComboBox.SelectedItem.ToString());
                try
                {
                    BL.FactoryMethode.GetBL().UpdateOrder(viewOrder);
                    MessageBox.Show("עודכן בהצלחה!!!");
                }
                catch (CannotUpdateException me)
                {
                    MessageBox.Show(me.Message);
                    statusComboBox.SelectedItem = old.Status;

                }
                if ((StatusOrder)statusComboBox.SelectedItem == StatusOrder.CustomerResponsiveness)
                    statusComboBox.IsEnabled = false;
            }
        }

        private void statusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            confirmation.IsEnabled = true;
        }
    }
}
