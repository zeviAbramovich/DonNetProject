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

        public Branche(int bank_num, string bank_name, int brunch_num, string branch_address, string branch_city)
        {
            BankNumber = bank_num;
            BankName = bank_name;
            BranchNumber = brunch_num;
            BranchAddress = branch_address;
            BranchCity = branch_city;
        }
    }
}
