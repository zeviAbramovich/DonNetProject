using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BE
{
    public class Order
    {
        public int OrderKey { get; set; }

        public String HostingUnitKey { get; set; }
        public String GuestRequestKey { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime OrderDate { get; set; }
        public Status Status { get; set; }

        public override string ToString()
        {
            return this.TostringProperties();
        }
    }
}
