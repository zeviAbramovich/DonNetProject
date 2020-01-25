using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BL
{
    public delegate bool delegateRequest(GuestRequest guestRequest);

    public interface IBL
    {
        bool CheckAvailableDate(HostingUnit unit, GuestRequest guest);
        bool CheckAvailableDateByDateAndSumDays(HostingUnit unit, DateTime date, int VacationDays);
        bool CreateOrder(GuestRequest guest);
        bool ChecksWhethertheUnitHasOpenOrders(long unitKey);
        int NumOfUnits(Host host);

        bool AddRequest(GuestRequest request);
        bool UpdateRequest(GuestRequest request);

        bool AddHostingUnit(HostingUnit unit);
        bool DeleteHostingUnit(long key);
        bool UpdateHostingUnit(HostingUnit unit);

        bool AddOrder(Order order);
        bool UpdateOrder(Order order);

        bool UpdateHost(Host host);

        HostingUnit GetUnit(long key);
        GuestRequest GetGuestRequest(long key);
        Order GetOrder(long key);

        List<HostingUnit> GetAllHostingUnit();
        List<GuestRequest> GetAllGuestRequest();
        List<Order> GetAllOrders();
        List<Branche> GetAllBranches();
        List<Order> GetAllHostOrders(long key);
        List<HostingUnit> GetAllHostUnits(long key);
       

        IEnumerable<IGrouping<Area, GuestRequest>> GetAllGuestRequestByArea(IEnumerable<GuestRequest> guestRequests);
        IEnumerable<IGrouping<int, GuestRequest>> GetAllGuestRequestByNumRelax(IEnumerable<GuestRequest> guestRequests);
        IEnumerable<IGrouping<Area, HostingUnit>> GetAllHostingUnitByArea(IEnumerable<HostingUnit> hostingUnits);
        IEnumerable<IGrouping<int, Host>> GetAllHostByNumHostingUnit(IEnumerable<Host> hosts);

        List<HostingUnit> UnitsAvailable(DateTime dateTime, int VacationDays);
        int SumDays(DateTime dateTime);
        int SumDays(DateTime dateTime1, DateTime dateTime2);
        List<Order> OrderFromNumDaysOrMore(int numDays);
        int NumOrdersOfGuest(GuestRequest guestRequest);
        int NumOrdersOfHostingUnit(HostingUnit hostingUnit);
        List<GuestRequest> GuestRequestCondition(delegateRequest requestCondition);
    }
}
