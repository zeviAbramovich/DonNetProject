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
        List<HostingUnit> HostingUnits = new List<HostingUnit>()
        {
            new HostingUnit("gozal",new Host("123","Zeev","Ab","0502418419","zevi3190@gmail.com",new BankAccount(10,"le",11,"Yaffo","Bney Brak",235800),false))
          
        };
     
        List<Order> ty;
        List<GuestRequest> v;
    }
}
