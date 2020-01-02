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
        public long HostId { get; set; }
        public String PrivateName { get; set; }
        public String FamilyName { get; set; }
        public long PhoneNumber { get; set; }
        public String MailAddress { get; set; }
        public BankAccount HostBankAccount { get; set; }
        public bool CollectionClearance { get; set; }

        public override string ToString()
        {
            return this.TostringProperties();
        }

    }
}
