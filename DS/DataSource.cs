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
        List<Branche> branches = new List<Branche>
      {new Branche(12,"Hapoalim",655,"rabi akiva","Bney brak"),
            new Branche(10,"leumi",865,"rabi akiva","Bney brak"),
        new Branche(12,"Hapoalim",69,"ashomer","Bney brak"),
        new Branche(10,"leumi",770,"herzel","Bney brak"),
        new Branche(10,"leumi",100,"herzel","jerusalem")
        };

        List<HostingUnit> hostingUnits=new List<HostingUnit>
        {
            new HostingUnit("Nof hagalil",new Host(1234,"zeev","Ab","0502418419","zevi3190@...",new BankAccount()


        }
    }
}
