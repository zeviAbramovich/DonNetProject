using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public enum StatusOrder
    {
        Not_Yet_Approved=1, Mail_Sent, Customer_Unresponsiveness, Customer_Responsiveness ,Request_Changed
    }

    public enum StatusGuest
    {
        Open=1, Closes_By_Site, Expired
    }

    public enum Area 
    {
        Jerusalem=1, North, South, Center
    }

    public enum HostingType
    {
        Zimmer=1, Hotel, Camping, Renting_Room
    }

    public enum Requirements
    {
        Necessary=1, Possible, not_Interested
    }
    
}
