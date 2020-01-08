using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BL
{
    public interface IBL
    {
       // bool CheckAvailableDate(HostingUnit unit, GuestRequest guest);
       // bool IsAllLetters(string s);
        bool CheckAvailableDateByDateAndSumDays(HostingUnit unit, DateTime date, int VacationDays);

        void AddRequest(GuestRequest t);
       // void DeleteGuest(GuestRequest guest);
        void UpdateRequest(GuestRequest t);

        void AddHostingUnit(HostingUnit t);
        void DeleteHostingUnit(HostingUnit t);
        void UpdateHostingUnit(HostingUnit t);

        void CreateOrder(GuestRequest guest);

        void AddOrder(Order o);
        void DeleteOrder(Order o);
        void UpdateOrder(Order o);

        void UpdateHost(Host host);

        HostingUnit GetUnit(long key);
        GuestRequest GetGuestRequest(long key);
        Order GetOrder(long key);

        List<HostingUnit> GetAllHostingUnit();
        List<GuestRequest> GetAllGuestRequest();
        List<Order> GetAllOrders();
        List<Branche> GetAllBranches();
        //List<GuestRequest> GetAllGuestRequestByArea(Area area);
        //List<GuestRequest> GetAllGuestRequestByNumRelax(int num);
        //List<HostingUnit> GetAllHostingUnitByArea(Area area);
        //List<Host> GetAllHostByNumHostingUnit();

        List<HostingUnit> UnitsAvailable(DateTime dateTime, int Vacation_Days);
        int SumDays(DateTime dateTime);
        int SumDays(DateTime dateTime1, DateTime dateTime2);
        List<Order> OrderFromNumDaysOrMore(int numDays);
        int NumOrdersOfGuest(GuestRequest guestRequest);
        int NumOrdersOfHostingUnit(HostingUnit hostingUnit);
        List<GuestRequest> AllGuestRequest(Delegate d, List<GuestRequest> l);
    }
}
