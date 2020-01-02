using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public enum StatusOrder
    {
        NotYetApproved, MailSent, CustomerUnresponsiveness, CustomerResponsiveness 
    }

    public enum StatusGuest
    {
        Open, ClosesBySite, Expired
    }

    public enum Area
    {
        Jerusalem, North, South, Center
    }

    public enum HostingType
    {
        Zimmer, Hotel, Camping, RentingRoom
    }

    public enum Requirements
    {
        Necessary, Possible, notInterested
    }
}
