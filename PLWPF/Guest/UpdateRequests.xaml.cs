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
using System.Net.Mail;
using BE;
using BL;

namespace PLWPF.Guest
{
    /// <summary>
    /// Interaction logic for UpdateRequests.xaml
    /// </summary>
    public partial class UpdateRequests : Page
    {
        List<GuestRequest> guestRequests = new List<GuestRequest>();
        Random random = new Random();
        long cod = 0;
        int count = 0;

        public UpdateRequests()
        {
            InitializeComponent();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            guestRequests = BL.FactoryMethode.GetBL().GuestRequestCondition(x => x.MailAddress == mailAddressTextBox.Text);
            if (!guestRequests.Any())
                MessageBox.Show("no match");
            else
            {
                long rand = random.Next(99999, 999999);
                cod = rand;
                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("gooking69770@gmail.com");
                mail.To.Add(mailAddressTextBox.Text);
                mail.Subject = "confirmation code";
                mail.Body = string.Format("Hello!\nyour code is{0}\n", rand.ToString());

                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential("gooking69770", "Zevi1234");
                smtpClient.EnableSsl = true;

                smtpClient.Send(mail);
                System.Windows.Forms.MessageBox.Show("mail sent");
                codeTextBox.IsEnabled = true;
                send.Visibility=Visibility.Collapsed;
                count = 0;
            }

        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            
            if (codeTextBox.Text != cod.ToString())
            {
                if (++count > 3)
                    MessageBox.Show("too many tryes!!");
                MessageBox.Show("code doesnt match, please check the code");
                
            }
            else
            {
                MainWindow.GetParent<Page>(this).NavigationService.Navigate(new RequestsView());
            }
        }
    }
}
