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

namespace PLWPF.Host
{
    /// <summary>
    /// Interaction logic for OrderReview.xaml
    /// </summary>
    public partial class OrderReview : Page
    {
        Order viewOrder = new Order();
        public OrderReview()
        {
            InitializeComponent();
            grid1.DataContext = viewOrder;
        }
        public OrderReview(Order order)
        {
            InitializeComponent();
            viewOrder = BL.FactoryMethode.GetBL().GetOrder(order.OrderKey);
            grid1.DataContext = viewOrder;
            statusComboBox.ItemsSource = Enum.GetValues(typeof(BE.StatusOrder));
            statusComboBox.Text = order.Status.ToString();
            
        }
        private void confirmation_Click(object sender, RoutedEventArgs e)
        {
            if ((StatusOrder)statusComboBox.SelectedItem == StatusOrder.MailSent)
            {
                viewOrder.Status = (BE.StatusOrder)Enum.Parse(typeof(BE.StatusOrder), statusComboBox.SelectedItem.ToString());

                try
                {
                    BL.FactoryMethode.GetBL().UpdateOrder(viewOrder);
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
                    System.Windows.Forms.MessageBox.Show("mail sent");
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
                    MessageBox.Show("not");
                }
                catch (CannotUpdateException me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }
    }
}
