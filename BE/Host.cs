using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BE
{
    public class Host
    {
        public String HostKey { get; set; }
        public String PrivateName { get; set; }
        public String FamilyName { get; set; }
        public String PhoneNumber { get; set; }
        public String MailAddress { get; set; }
        public BankAccount HostBankAccount { get; set; }
        public bool CollectionClearance { get; set; }
        //temporary ctor
        public Host(String key, String pname, String fname, String pnum, String mail, BankAccount bank, bool b)
        {
            HostKey = key;
            PrivateName = pname;
            FamilyName = fname;
            PhoneNumber = pnum;
            MailAddress = mail;
            HostBankAccount = bank;
            CollectionClearance = b;
        }

        public override string ToString()
        {
            return this.TostringProperties();
        }

    }
}
