using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    class Dal_imp : IDal
    {
        public void AddHostingUnit(HostingUnit t)
        {
            DS.DataSource.hostingUnits.Add(t);
        }

        public void AddOrder(Order o)
        {
            DS.DataSource.orders.Add(o);
        }

        public void AddRequest(GuestRequest t)
        {
            DS.DataSource.guestRequests.Add(t);
        }

        public void DeleteHostingUnit(HostingUnit t)
        {
            var result = from item in DS.DataSource.hostingUnits
                         where item.HostingUnitKey == t.HostingUnitKey
                         select item;
            foreach (var item in result)
            {
                DS.DataSource.hostingUnits.Remove(item);
            }
        }

        public List<Branche> GetAllBranches()
        {
            List<Branche> branches = new List<Branche>
            {
                new Branche(12,"Hapoalim",111,"Rabi akiva","Bney brak"),
                new Branche(12,"Hapoalim",222,"Hashomer","Bney brak"),
                new Branche(12,"Hapoalim",333,"Herzel","Bney brak"),
                new Branche(12,"Hapoalim",444,"Ainshteyn","Bney brak"),
                new Branche(12,"Hapoalim",555,"Hagiborim","Bney brak")
            };
            return branches;
        }

        public List<GuestRequest> GetAllGuestRequest()
        {
            List<GuestRequest> guestRequests = DS.DataSource.guestRequests.ToList();
            return guestRequests;
        }

        public List<HostingUnit> GetAllHostingUnit()
        {
            List<HostingUnit> hostingUnits = DS.DataSource.hostingUnits.ToList();
            return hostingUnits;
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = DS.DataSource.orders.ToList();
            return orders;
        }

        public void UpdateHostingUnit(HostingUnit t)
        {
            var update = from item in DS.DataSource.hostingUnits
                         where item.HostingUnitKey == t.HostingUnitKey
                         select item;
            foreach (var item in update)
            {
                DeleteHostingUnit(item);
            }
            AddHostingUnit(t);
        }



        public void UpdateOrder(Order o)
        {
            var update = from item in DS.DataSource.orders
                         where item.OrderKey == o.OrderKey
                         select item;
            foreach (var item in update)
            {
                DS.DataSource.orders.Remove(item);
            }
            AddOrder(o);
        }

        public void UpdateRequest(GuestRequest t)
        {
            var update = from item in DS.DataSource.guestRequests
                         where item.GuestRequestKey == t.GuestRequestKey
                         select item;
            foreach (var item in update)
            {
                DS.DataSource.guestRequests.Remove(item);
            }
            AddRequest(t);
        }
    }
}
