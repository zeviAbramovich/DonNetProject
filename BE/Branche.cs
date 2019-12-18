using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Branche
    {
        public int BankNumber { get; set; }
        public String BankName { get; set; }
        public int BranchNumber { get; set; }
        public String BranchAddress { get; set; }
        public String BranchCity { get; set; }
    }
    static List<Branche> branches => new List<Branche> {
         new List<Branche> { new Branche { BankName = "hapoalim", BankNumber = 10, BranchAddress = "rabi akiva", BranchCity = "Bney Brak", BranchNumber = 655 },
        new Branche { BankName = "leumi", BankNumber = 9, BranchAddress = "rabi akiva", BranchCity = "Bney Brak", BranchNumber = 865 },
        new Branche{ BankName = "hapoalim", BankNumber = 10, BranchAddress = "Hashomer", BranchCity = "Bney Brak", BranchNumber = 69 }
        };
         }





