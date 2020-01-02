﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BE
{
    public class Order
    {
        public long HostingUnitKey { get; set; }
        public long GuestRequestKey { get; set; }
        public long OrderKey { get; set; }
        public StatusOrder Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime OrderDate { get; set; }        

        public override string ToString()
        {
            return this.TostringProperties();
        }
    }
}
