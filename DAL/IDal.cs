﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BE;
using DAL;
using DS;

namespace DAL
{
    public interface IDal
    {
        
        bool AddRequest(GuestRequest request);
        bool UpdateRequest(GuestRequest request);

        bool AddHostingUnit(HostingUnit unit);
        bool DeleteHostingUnit(long unit);
        bool UpdateHostingUnit(HostingUnit unit);

        bool AddOrder(Order order);
        bool UpdateOrder(Order order);

        bool AddHost(Host host);
        bool UpdateHost(Host host);

        HostingUnit GetHostingUnit(long key);
        Order GetOrder(long key);
        GuestRequest GetGuestRequest(long key);
        Host GetHost(long key);

        List<HostingUnit> GetAllHostingUnits();
        List<GuestRequest> GetAllGuestRequests();
        List<Order> GetAllOrders();
        List<Branche> GetAllBranches();
        List<Host> GetAllHosts();
        void CreateFile(string typename, string path);
        XElement LoadData(string path);
    }
}
