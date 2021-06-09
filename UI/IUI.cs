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
            IBL bl = BL.FactoryMethode.GetBL();
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
                    case 1://client
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
                                    Console.WriteLine(bl.GetGuestRequest(10000001).ToString());
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
                                    guestTemp = bl.GetGuestRequest(longtemp);
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
                                chois = 0;
                                HostingUnit unit = new HostingUnit
                                {
                                    HostingUnitName = "qw",
                                    Area = Area.Center,
                                    SubArea = "vdf",
                                    HostingType = HostingType.Zimmer,
                                    Adults = 3,
                                    Children = 2,
                                    Pool = true,
                                    Garden = true,
                                    Jacuzzi = true,
                                    ChildrensAttractions = true,
                                    Owner = new Host
                                    {
                                        HostId = 313,
                                        PrivateName = "zeev",
                                        FamilyName = "abra",
                                        PhoneNumber = 0502418419,
                                        MailAddress = "zevi3190@gmail.com",
                                        HostBankAccount = new BankAccount
                                        {
                                            BankAccountNumber = 333,
                                            BankNumber = bl.GetAllBranches().FirstOrDefault(x => x.BankNumber == 12).BankNumber,
                                            BranchAddress = bl.GetAllBranches().FirstOrDefault(x => x.BankNumber == 12).BranchAddress,
                                            BankName = bl.GetAllBranches().FirstOrDefault(x => x.BankNumber == 12).BankName,
                                            BranchCity = bl.GetAllBranches().FirstOrDefault(x => x.BankNumber == 12).BranchCity,
                                            BranchNumber = bl.GetAllBranches().FirstOrDefault(x => x.BankNumber == 12).BranchNumber
                                        },
                                    },
                                };
                                while (!Utilities.Tools.ValidateMail(unit.Owner.MailAddress))
                                {
                                    Console.WriteLine("enter mail again");
                                }
                                while (!Utilities.Tools.IsAllLetters(unit.HostingUnitName))
                                {
                                    Console.WriteLine("enter unit name again");
                                }
                                Console.WriteLine("Are you agree for terms & conditions? (Y/N)");
                                if (Console.ReadLine() == "Y")
                                    unit.Owner.CollectionClearance = true;
                                try
                                {
                                    bl.AddHostingUnit(unit);
                                }
                                catch (CannotAddException cae)
                                {
                                    Console.WriteLine("Failed to add" + cae.Message);
                                    Console.WriteLine(Menu.GetMenu(Menu.MainMenu.Client));
                                    chois = Int32.Parse(Console.ReadLine());
                                }
                                Console.WriteLine("this your units ");
                                List<HostingUnit> units = bl.GetAllHostingUnit();
                                foreach (HostingUnit item in units)
                                {
                                    Console.WriteLine(item.HostingUnitKey);
                                }
                                Console.Write(Menu.GetMenu(Menu.MainMenu.Client));
                                chois = Int32.Parse(Console.ReadLine());
                                break;
                            case 2://DeleteHostingUnit
                                Console.WriteLine("Which host you want to delete? (keyNumber)");
                                try
                                {
                                    bl.DeleteHostingUnit(long.Parse(Console.ReadLine()));
                                }
                                catch (CannotDeleteException cde)
                                {
                                    Console.WriteLine("Failed" + cde.Message);
                                    chois = 0;
                                    break;
                                }
                                List<HostingUnit> list = bl.GetAllHostingUnit();
                                foreach (var item in list)
                                {
                                    Console.WriteLine(item.HostingUnitKey.ToString());
                                }

                                Console.Write(Menu.GetMenu(Menu.MainMenu.Client));
                                chois = Int32.Parse(Console.ReadLine());
                                break;

                            case 3://UpdateHostingUnit
                                Console.WriteLine("enter num of unit");
                                long key = 10000000;
                                HostingUnit hostingUnit = bl.GetUnit(key);
                                hostingUnit.Diary[1, 5] = true;
                                //  hostingUnit.Owner.CollectionClearance = false;
                                bl.UpdateHostingUnit(hostingUnit);
                                hostingUnit = bl.GetUnit(10000000);
                                Console.WriteLine(hostingUnit.Diary[1, 5]);
                                break;

                            case 4://add order                                  
                                GuestRequest request = bl.GetGuestRequest(10000001);
                                bl.CreateOrder(request);
                                foreach (Order item in bl.GetAllOrders())
                                {
                                    Console.WriteLine(item.OrderKey);
                                }
                                Console.WriteLine("sucsses");
                                Console.Write(Menu.GetMenu(Menu.MainMenu.Client));
                                chois = Int32.Parse(Console.ReadLine());
                                break;

                            case 5://update order
                                Order order = bl.GetOrder(10000001);
                                order.Status = StatusOrder.CustomerResponsiveness;
                                bl.UpdateOrder(order);
                                order = bl.GetOrder(10000002);
                                Console.WriteLine(order.Status);
                                Console.Write(Menu.GetMenu(Menu.MainMenu.Client));
                                chois = Int32.Parse(Console.ReadLine());
                                break;

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
