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


        static void Main(string[] args)
        {
            IBL bl =BL.FactoryMethode.GetBL();
            Console.WriteLine("Wellcome to Gooking!");
            Console.WriteLine("Who are you? (Client/Owner)");
            Console.Write(Menu.GetMenu(Menu.MainMenu.Client));//client / owner
            int chois;
            while (Int32.TryParse(Console.ReadLine(), out chois) != true)
            {
                Console.WriteLine("You have to choose a number!");
            }
            do switch (chois)
                {
                    case 1:
                        chois = 0;
                        Console.WriteLine("What you would like to do?");
                        Console.Write(Menu.GetMenu(Menu.ClientMenu.AddRequest));
                        while (Int32.TryParse(Console.ReadLine(), out chois) != true)
                        {
                            Console.WriteLine("You have to choose a number!");
                        }
                        switch (chois)
                        {
                            case 1://add request
                                //TODO exceptions in order the whiles
                                string stringTemp = "";
                                int intTemp = 0;
                                DateTime dateTemp;
                                GuestRequest guestTemp = new GuestRequest();
                                Console.WriteLine("Please fill all details:\n");
                                Console.WriteLine("Private Name?");
                                while (!Utilities.Tools.IsAllLetters(Console.ReadLine()))
                                {
                                    Console.WriteLine("use only letters, please enter again");
                                }
                                guestTemp.PrivateName = stringTemp;
                                Console.WriteLine("Family Name?");
                                while (!Utilities.Tools.IsAllLetters(Console.ReadLine()))
                                {
                                    Console.WriteLine("use only letters, please enter again");
                                }
                                guestTemp.FamilyName = stringTemp;
                                Console.WriteLine("Your e-mail address?");
                                while (!Utilities.Tools.ValidateMail(Console.ReadLine()))
                                {
                                    Console.WriteLine("E-mail not valid!");

                                }
                                guestTemp.MailAddress = stringTemp;
                                guestTemp.RegistrationDate = DateTime.Now;
                                Console.WriteLine("When you would like to come?(dd/mm/yyyy)");
                                while (!DateTime.TryParse(Console.ReadLine(), out dateTemp))
                                {
                                    Console.WriteLine("Incorect date...");
                                    Console.WriteLine("When you would like to come?(dd/mm/yyyy)");
                                }
                                guestTemp.EntryDate = dateTemp;
                                Console.WriteLine("When you would like to leave?(dd/mm/yyyy)");
                                while (!DateTime.TryParse(Console.ReadLine(), out dateTemp))
                                {
                                    Console.WriteLine("Incorect date...");
                                    Console.WriteLine("When you would like to leave?(dd/mm/yyyy)");

                                }
                                guestTemp.ReleaseDate = dateTemp;
                                Console.WriteLine("where? (1-4)\n" + Utilities.Tools.GetEnum(guestTemp.Area));
                                while (!Int32.TryParse(Console.ReadLine(), out intTemp))
                                {
                                    Console.WriteLine("You must enter a number!");
                                }
                                guestTemp.Area = (Area)intTemp;
                                Console.WriteLine("where exactly?\n");
                                stringTemp = Console.ReadLine();
                                guestTemp.SubArea = stringTemp;
                                Console.WriteLine("What type of hosting? (1-4)\n" + Utilities.Tools.GetEnum(guestTemp.HostingType));
                                while (!Int32.TryParse(Console.ReadLine(), out intTemp))
                                {
                                    Console.WriteLine("You must enter a number!");
                                }
                                guestTemp.HostingType = (HostingType)intTemp;
                                Console.WriteLine("How many adults?");
                                while (!Int32.TryParse(Console.ReadLine(), out intTemp))
                                {
                                    Console.WriteLine("You must enter a number!");
                                }
                                guestTemp.Adults = intTemp;
                                Console.WriteLine("How many childrens?");
                                while (!Int32.TryParse(Console.ReadLine(), out intTemp))
                                {

                                    Console.WriteLine("You must enter a number!");
                                }
                                guestTemp.Children = intTemp;
                                Console.WriteLine("Do you want a Swimming Pool? (1-3)\n" + Utilities.Tools.GetEnum(guestTemp.Pool));
                                while (!Int32.TryParse(Console.ReadLine(), out intTemp))
                                {
                                    Console.WriteLine("You must enter a number!");
                                }
                                guestTemp.Pool = (Requirements)intTemp;
                                Console.WriteLine("Do you want a Jacuzzi? (1-3)\n" + Utilities.Tools.GetEnum(guestTemp.Jacuzzi));
                                while (!Int32.TryParse(Console.ReadLine(), out intTemp))
                                {
                                    Console.WriteLine("You must enter a number!");
                                }
                                guestTemp.Jacuzzi = (Requirements)intTemp;
                                Console.WriteLine("Do you want a Garden? (1-3)\n" + Utilities.Tools.GetEnum(guestTemp.Garden));
                                while (!Int32.TryParse(Console.ReadLine(), out intTemp))
                                {
                                    Console.WriteLine("You must enter a number!");
                                }
                                guestTemp.Garden = (Requirements)intTemp;
                                Console.WriteLine("Do you want a Childrens Attractions? (1-3)\n" + Utilities.Tools.GetEnum(guestTemp.Jacuzzi));
                                while (!Int32.TryParse(Console.ReadLine(), out intTemp))
                                {
                                    Console.WriteLine("You must enter a number!");
                                }
                                guestTemp.ChildrensAttractions = (Requirements)intTemp;
                                try
                                {
                                    bl.AddRequest(guestTemp);
                                    Console.WriteLine(BL.FactoryMethode.GetBL().GetGuestRequest(10000000).ToString());
                                    Console.WriteLine("Your request added successfuly!");
                                }
                                catch (CannotAddException cae)
                                {
                                    Console.Write("ERROR! ");
                                    Console.WriteLine(cae.Message);
                                    Console.WriteLine("Your request failed!\n **************");
                                    Console.Write(Menu.GetMenu(Menu.MainMenu.Client));
                                    Console.ReadLine();
                                    break;
                                }

                                break;
                            case 2://update request
                                long longtemp = 0;

                                Console.WriteLine("whats the number of the request");
                                while (!long.TryParse(Console.ReadLine(), out longtemp))
                                {
                                    Console.WriteLine("you must to insert numbers!!!");
                                }
                                try
                                {
                                    guestTemp =bl.GetGuestRequest(longtemp);
                                    Console.WriteLine(guestTemp.ToString());

                                }
                                catch (MissingMemberException me)
                                {
                                    Console.WriteLine("ERROR! " + me.GetType() + " " + me.TargetSite);
                                    Console.Write(Menu.GetMenu(Menu.MainMenu.Client));
                                    Console.ReadLine();
                                    break;
                                }
                                Console.Write(Menu.GetMenu(Menu.MainMenu.Client));
                                chois = Int32.Parse(Console.ReadLine());
                                break;
                            case 3:
                                Console.Write(Menu.GetMenu(Menu.MainMenu.Client));
                                chois = Int32.Parse(Console.ReadLine());
                                break;
                            default:
                                chois = 0;
                                break;



                        }
                        break;
                    case 2://Owner
                        chois = 0;
                        Console.WriteLine("What you would like to do?");
                        Console.WriteLine(Menu.GetMenu(Menu.OwnerMenu.AddHostingUnit));
                        chois = Int32.Parse(Console.ReadLine());
                        switch (chois)
                        {
                            case 1://AddHostingUnit
                                List<HostingUnit> list = new List<HostingUnit>();
                                list =bl.GetAllHostingUnit();
                                Console.WriteLine(list.ToString());
                                break;
                            case 2://DeleteHostingUnit
                            case 3://UpdateHostingUnit

                            default:
                                break;
                        }
                        break;
                    default:
                        break;

                } while (chois != 0);
        }
    }
}
