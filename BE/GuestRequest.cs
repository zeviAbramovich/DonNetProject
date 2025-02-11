﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BE
{
    public class GuestRequest
    {
        public long GuestRequestKey { get; set; }
        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public string MailAddress { get; set; }
        //public string Password { get; set; }
        public StatusGuest Status { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Area Area { get; set; }
        public string SubArea { get; set; }
        public HostingType HostingType { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public Requirements Pool { get; set; }
        public Requirements Jacuzzi { get; set; }
        public Requirements Garden { get; set; }
        public Requirements ChildrensAttractions { get; set; }

        public override string ToString()
        {
            return this.TostringProperties();
        }
    }
}
