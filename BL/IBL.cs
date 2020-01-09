﻿using System;
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
        bool CheckAvailableDate(HostingUnit unit, GuestRequest guest);
        bool CheckAvailableDateByDateAndSumDays(HostingUnit unit, DateTime date, int VacationDays);
        bool CreateOrder(GuestRequest guest);

        bool AddRequest(GuestRequest request);
        bool UpdateRequest(GuestRequest request);

        bool AddHostingUnit(HostingUnit unit);
        bool DeleteHostingUnit(HostingUnit unit);
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
        //TODO grouping List<GuestRequest> GetAllGuestRequestByArea(Area area);
        //TODO grouping List<GuestRequest> GetAllGuestRequestByNumRelax(int num);
        //TODO grouping List<HostingUnit> GetAllHostingUnitByArea(Area area);
        //TODO grouping List<Host> GetAllHostByNumHostingUnit();

        List<HostingUnit> UnitsAvailable(DateTime dateTime, int VacationDays);
        int SumDays(DateTime dateTime);
        int SumDays(DateTime dateTime1, DateTime dateTime2);
        List<Order> OrderFromNumDaysOrMore(int numDays);
        int NumOrdersOfGuest(GuestRequest guestRequest);
        int NumOrdersOfHostingUnit(HostingUnit hostingUnit);
        List<GuestRequest> AllGuestRequest(Delegate d, List<GuestRequest> l);
    }
}
