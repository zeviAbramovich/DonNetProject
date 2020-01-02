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
        public static List<HostingUnit> hostsList = new List<HostingUnit>()
        {
                new HostingUnit()
                {
                  HostingUnitName = "Plaza",HostingUnitKey = Configuration.serialHostingUnit++,
                    Owner = new Host()
                    {
                          HostId = 313583460,
                          PrivateName = "Zeev",
                          FamilyName = "Abra",
                          MailAddress = "zevi3190@gmail.com",
                          PhoneNumber = 0502418419,
                          CollectionClearance = false,
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
                     Owner = new Host()
                     {
                        HostId = 313583460,
                        PrivateName = "Zeev",
                        FamilyName = "Abra",
                        MailAddress = "zevi3190@gmail.com",
                        PhoneNumber = 0502418419,
                        CollectionClearance = false,
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
    }
}

