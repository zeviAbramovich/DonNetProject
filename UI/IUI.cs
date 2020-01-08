using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using BL;

namespace UI
{
    class IUI
    {
        public IBL bl = BL.FactoryMethode.GetBL();
        
      
        static void Main(string[] args)
        {
            Console.WriteLine(Menu.GetMenu(Menu.OwnerMenu.AddHostingUnit));
            int chois;
            while (Int32.TryParse(Console.ReadLine(), out chois) != true)
            {
                Console.WriteLine("You have to choose a number!");
            }

            switch (chois)
            {
                case 1:
                    Console.WriteLine("What you would like to do?");
                    Console.WriteLine(Menu.GetMenu(Menu.ClientMenu.AddRequest));
                    while (Int32.TryParse(Console.ReadLine(), out chois) != true)
                    {
                        Console.WriteLine("You have to choose a number!");
                    }
                    switch (chois)
                    {
                        case 1:
                            //TODO exceptions in order the whiles
                            string stringTemp = "";
                            int intTemp = 0;
                            DateTime dateTemp;
                            GuestRequest guestTemp = new GuestRequest();
                            guestTemp.GuestRequestKey = Configuration.serialGuestRequest++;
                            Console.WriteLine("Please fill all details () :\n" + guestTemp.ToString());
                            Console.WriteLine("Private Name?");
                            guestTemp.PrivateName = Console.ReadLine();
                            Console.WriteLine("Family Name?");
                            guestTemp.FamilyName = Console.ReadLine();
                            Console.WriteLine("Your e-mail address?");
                            stringTemp = Console.ReadLine();
                            while (!Utilities.Tools.ValidateMail(stringTemp))
                            {
                                Console.WriteLine("E-mail not valid!");
                                stringTemp = Console.ReadLine();
                            }
                            guestTemp.MailAddress = stringTemp;
                            guestTemp.RegistrationDate=DateTime.Now;
                            Console.WriteLine("When you would like to come?(dd/mm/yyyy)");
                            stringTemp = Console.ReadLine();
                            while(!DateTime.TryParse(stringTemp,out dateTemp))
                            {
                                Console.WriteLine("Incorect date...");
                                Console.WriteLine("When you would like to come?(dd/mm/yyyy)");
                                stringTemp = Console.ReadLine();
                            }
                            Console.WriteLine("When you would like to leave?(dd/mm/yyyy)");
                            stringTemp = Console.ReadLine();
                            guestTemp.EntryDate = dateTemp;
                            while (!DateTime.TryParse(stringTemp, out dateTemp))
                            {
                                Console.WriteLine("Incorect date...");
                                Console.WriteLine("When you would like to leave?(dd/mm/yyyy)");
                                stringTemp = Console.ReadLine();
                            }
                            guestTemp.ReleaseDate = dateTemp;
                            Console.WriteLine("where? (1-4)\n"+Utilities.Tools.GetEnum(guestTemp.Area));
                            stringTemp = Console.ReadLine();
                            while(!Int32.TryParse(stringTemp,out intTemp))
                            {
                                Console.WriteLine("You must enter a number!");
                            }
                            guestTemp.Area = (Area)intTemp; Console.WriteLine("where exactly?\n");
                            stringTemp = Console.ReadLine();
                            guestTemp.SubArea = stringTemp;
                            Console.WriteLine("What type of hosting? (1-4)\n" + Utilities.Tools.GetEnum(guestTemp.HostingType));
                            stringTemp = Console.ReadLine();
                            while (!Int32.TryParse(stringTemp, out intTemp))
                            {
                                Console.WriteLine("You must enter a number!");
                            }
                            guestTemp.HostingType = (HostingType)intTemp;
                            Console.WriteLine("How many adults?");
                            guestTemp.Adults = Int32.Parse(Console.ReadLine());//TODO catch
                            Console.WriteLine("How many childrens?");
                            guestTemp.Children = Int32.Parse(Console.ReadLine());
                            Console.WriteLine("Do you want a Swiming Pool? (1-3)\n" + Utilities.Tools.GetEnum(guestTemp.Pool));
                            stringTemp = Console.ReadLine();
                            while (!Int32.TryParse(stringTemp, out intTemp))
                            {
                                Console.WriteLine("You must enter a number!");
                            }
                            guestTemp.Pool = (Requirements)intTemp;
                            Console.WriteLine("Do you want a Jacuzzi? (1-3)\n" + Utilities.Tools.GetEnum(guestTemp.Jacuzzi));
                            stringTemp = Console.ReadLine();
                            while (!Int32.TryParse(stringTemp, out intTemp))
                            {
                                Console.WriteLine("You must enter a number!");
                            }
                            guestTemp.Jacuzzi=(Requirements)intTemp;
                            Console.WriteLine("Do you want a Garden? (1-3)\n" + Utilities.Tools.GetEnum(guestTemp.Garden));
                            stringTemp = Console.ReadLine();
                            while (!Int32.TryParse(stringTemp, out intTemp))
                            {
                                Console.WriteLine("You must enter a number!");
                            }
                            guestTemp.Garden = (Requirements)intTemp;
                            Console.WriteLine("Do you want a Childrens Attractions? (1-3)\n" + Utilities.Tools.GetEnum(guestTemp.Jacuzzi));
                            stringTemp = Console.ReadLine();
                            while (!Int32.TryParse(stringTemp, out intTemp))
                            {
                                Console.WriteLine("You must enter a number!");
                            }
                            guestTemp.ChildrensAttractions = (Requirements)intTemp;
                            
                            Console.WriteLine(guestTemp.ToString());
                            break;
                        case 2:
                            //delete request
                            //TODO אולי כדאי שהארגומנט יהיה מספר הבקשה. 
                            GuestRequest guestRequest = new GuestRequest();
                            //BL.FactoryMethode.GetBL().DeleteGuest(guestRequest);
                                ;
                            break;
                            
                    }
                    break;
                case 2:
                    Console.WriteLine("What you would like to do?");
                    Console.WriteLine(Menu.GetMenu(Menu.OwnerMenu.AddHostingUnit));
                    break;
                default:
                    break;
            }
        }
    }
}
