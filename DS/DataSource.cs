using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DS
{
    public class DataSource
    {
        public static List<HostingUnit> hostingUnitList = new List<HostingUnit>()
        {
                new HostingUnit()
                {
                  HostingUnitName = "Plaza",HostingUnitKey = Configuration.serialHostingUnit++,Diary=new bool[12,31],
                    HostingType=(HostingType)1,Adults=3,Children=3,Area = (Area)1,Garden=true,
                    Pool = true, Jacuzzi =true,ChildrensAttractions = true,
                    Owner = new Host()
                    {
                          HostId = 313583460,
                          Password="A1234",
                          PrivateName = "Zeev",
                          FamilyName = "Abra",
                          MailAddress = "zevi3190@gmail.com",
                          PhoneNumber = 0502418419,
                          CollectionClearance = true,
                          HostBankAccount = new BankAccount()
                             {
                                 BranchNumber = 111,
                                 BranchCity = "Bney brak",
                                 BranchAddress = "Rabi akiva",
                                 BankName = "Hapoalim",
                                 BankNumber = 10,
                                 BankAccountNumber = 235800
                             }
                    }
                },

                 new HostingUnit()
                 {
                     HostingUnitName = "nirvana",HostingUnitKey = Configuration.serialHostingUnit,
                     Diary=new bool[12,31],HostingType=(HostingType)1,Adults=3,
                     Children=3,Area = (Area)1,Garden=true,
                    Pool = true, Jacuzzi =true,ChildrensAttractions = true,
                     Owner = new Host()
                     {
                        HostId = 313583460,
                        PrivateName = "Zeev",
                        FamilyName = "Abra",
                        MailAddress = "zevi3190@gmail.com",
                        PhoneNumber = 0502418419,
                        CollectionClearance = true,
                        HostBankAccount = new BankAccount()
                        {
                            BranchNumber = 111,
                            BranchCity = "Bney brak",
                            BranchAddress = "Rabi akiva",
                            BankName = "Hapoalim",
                            BankNumber = 10,
                            BankAccountNumber = 235800
                        }
                     }
                 }
        };
        public static List<Order> orders = new List<Order> { };
        public static List<GuestRequest> guestRequests = new List<GuestRequest> { };
        public static List<Host> hosts = new List<Host> { };
    }
}

