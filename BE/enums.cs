using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public enum StatusOrder
    {
        NotYetApproved, MailSent, CustomerUnresponsiveness, CustomerResponsiveness ,RequestChanged
    }

    public enum StatusGuest
    {
        Open=1, ClosesBySite, Expired
    }

    public enum Area 
    {
        Jerusalem=1, North, South, Center
    }

    public enum HostingType
    {
        Zimmer=1, Hotel, Camping, RentingRoom
    }

    public enum Requirements
    {
        Necessary=1, Possible, notInterested
    }
    
}
