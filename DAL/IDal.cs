using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using DS;

namespace DAL
{
    public interface IDal
    {
        void AddRequest(GuestRequest t);
       // void DeleteGuest(GuestRequest guest);
        void UpdateRequest(GuestRequest t);

        void AddHostingUnit(HostingUnit t);
        void DeleteHostingUnit(HostingUnit t);
        void UpdateHostingUnit(HostingUnit t);

        void AddOrder(Order o);
     //   void DeleteOrder(Order o);
        void UpdateOrder(Order o);

        void UpdateHost(Host host);

        HostingUnit GetHostingUnit(long key);
        Order GetOrder(long key);
        GuestRequest GetGuestRequest(long key);

        List<HostingUnit> GetAllHostingUnit();
        List<GuestRequest> GetAllGuestRequest();
        List<Order> GetAllOrders();
        List<Branche> GetAllBranches();        
    }
}
