using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BE
{
    public class BankAccount
    {
        public int BankNumber { get; set; }
        public String BankName { get; set; }
        public int BranchNumber { get; set; }
        public String BranchAddress { get; set; }
        public String BranchCity { get; set; }
        public int BankAccountNumber { get; set; }

        public BankAccount(int _BankNumber, String _BankName, int _BranchNumber, String _BranchAddress, String _BranchCity, int _BankAccountNum)
        {
            BankNumber = _BankNumber;
            BankName = _BankName;
            BranchNumber = _BranchNumber;
            BranchAddress = _BranchAddress;
            BranchCity = _BranchCity;
            BankAccountNumber = _BankAccountNum;
        }

        public override string ToString()
        {
            return this.TostringProperties();
        }
    }
}
