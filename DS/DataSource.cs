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
        public static List<HostingUnit> hostingUnits = new List<HostingUnit>
        {
            new HostingUnit("Nof hagalil",new Host("1234","zeev","Ab","050241","zevi3190@gmail.com",new BankAccount(12,"Hapoalim",111,"Rabi akiva","Bney brak",235800),false)),
            new HostingUnit("Villa",new Host("1234","zeev","Ab","050241","zevi3190@gmail.com",new BankAccount(12,"Hapoalim",222,"Hashomer","Bney brak",244779),false)),
            new HostingUnit("Primium",new Host("1234","zeev","Ab","050241","zevi3190@gmail.com",new BankAccount(12,"Hapoalim",333,"Herzel","Bney brak",335665),false))
        };
        public static List<Order> orders;
        public static List<GuestRequest> guestRequests;
    }
}
