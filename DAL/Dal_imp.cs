using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

namespace DAL
{
    class Dal_imp : IDal
    {
        public void AddHostingUnit(HostingUnit t)
        {
            var v = from a in DataSource.hostsList
                    where t.HostingUnitKey == a.HostingUnitKey
                    select a;
            foreach (var item in v)
                return;//catch try, beacose have a unit with same key 
            if (t.HostingUnitKey == 0)
            {
                HostingUnit hostingUnit = t.Clone();
                hostingUnit.HostingUnitKey = Configuration.serialHostingUnit++;
                hostingUnit.Diary = new bool[12, 31];
                DataSource.hostsList.Add(hostingUnit);
                return;
            }
            HostingUnit unit = t.Clone();
            DataSource.hostsList.Add(unit);
            return;
        }

        public void AddOrder(Order o)
        {
            var v = from a in DataSource.orders
                    where o.OrderKey == a.OrderKey
                    select a;
            foreach (var item in v)
                return;//try catch           
            Order order = o.Clone();
            order.OrderKey = Configuration.serialOrder++;
            DS.DataSource.orders.Add(order);
            return;
        }

        public void AddRequest(GuestRequest t)
        {
            var v = from a in DataSource.guestRequests
                    where a.GuestRequestKey == t.GuestRequestKey
                    select a;
            foreach (var item in v)
                return;//try catch 
            if (t.GuestRequestKey == 0)
            {
                GuestRequest guestRequest = t.Clone();
                guestRequest.GuestRequestKey = Configuration.serialGuestRequest++;
                DataSource.guestRequests.Add(guestRequest);
                return;
            }
            GuestRequest request = t.Clone();
            DataSource.guestRequests.Add(request);
            return;
        }

        public void DeleteHostingUnit(HostingUnit t)
        {
            foreach (var item in DataSource.hostsList)
            {
                if (item.HostingUnitKey == t.HostingUnitKey)
                {
                    DataSource.hostsList.Remove(item);
                    return;
                }
                return;//try catch
            }
        }

        public void DeleteOrder(Order o)
        {
            foreach (var item in DataSource.orders)
            {
                if (item.OrderKey == o.OrderKey)
                {
                    DataSource.orders.Remove(item);
                    return;
                }
                return;//try catch
            }
        }

        public void DeleteGuest(GuestRequest guest)
        {
            foreach (var item in DataSource.guestRequests)
            {
                if (item.GuestRequestKey == guest.GuestRequestKey)
                {
                    DataSource.guestRequests.Remove(item);
                    return;
                }
                return;//try catch, not find request.
            }
        }

        public List<Branche> GetAllBranches()
        {
            List<Branche> branches = new List<Branche>
            {
                new Branche()
                {
                    BankNumber=12,BankName="Hapoalim",BranchAddress="Rabi akiva",
                    BranchCity="Bney brak",BranchNumber=111
                },

                new Branche()
                {
                    BankNumber=12,BankName="Hapoalim",BranchAddress="Hashomer",
                    BranchCity="Bney brak",BranchNumber=222
                },

                new Branche()
                {
                    BankNumber=12,BankName="Hapoalim",BranchAddress="Herzel",
                    BranchCity="Bney brak",BranchNumber=333
                },

                new Branche()
                {
                    BankNumber=12,BankName="Hapoalim",BranchAddress="Ainshteyn",
                    BranchCity="Bney brak",BranchNumber=444
                },


                new Branche()
                {
                    BankNumber=12,BankName="Hapoalim",BranchAddress="Hagiborim",
                    BranchCity="Bney brak",BranchNumber=555
                }
            };

            return branches;
        }

        public List<GuestRequest> GetAllGuestRequest()
        {
            List<GuestRequest> guestRequests = new List<GuestRequest>();
            foreach (var item in DataSource.guestRequests)
                guestRequests.Add(item.Clone());
            return guestRequests;
        }

        public List<HostingUnit> GetAllHostingUnit()
        {
            List<HostingUnit> hostingUnits = new List<HostingUnit>();
            foreach (var item in DataSource.hostsList)
                hostingUnits.Add(item.Clone());
            return hostingUnits;
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();
            foreach (var item in DataSource.orders)
                orders.Add(item.Clone());
            return orders;
        }

        public HostingUnit GetHostingUnit(long key)
        {
            HostingUnit unit = new HostingUnit();
            var v = from a in DataSource.hostsList
                    where a.HostingUnitKey == key
                    select a;
            foreach (var item in v)
            {
                unit = item.Clone();
            }
            return unit;
        }

        public Order GetOrder(long key)
        {
            Order order = new Order();
            var v = from a in DataSource.orders
                    where a.OrderKey == key
                    select a;
            foreach (var item in v)
            {
                order = item.Clone();
            }
            return order;
        }

        public GuestRequest GetGuestRequest(long key)
        {
            GuestRequest guest = new GuestRequest();
            var v = from a in DataSource.guestRequests
                    where a.GuestRequestKey == key
                    select a;
            foreach (var item in v)            
                guest = item.Clone();           
            return guest;
        }

        public void UpdateHostingUnit(HostingUnit unit)
        {
            var v = from a in DataSource.hostsList
                    where a.HostingUnitKey == unit.HostingUnitKey
                    select a;
            foreach (var item in v)
            {
                DeleteHostingUnit(item);
                AddHostingUnit(unit);//add is doing clone...
                return;
            }
            return;//try catch
        }

        public void UpdateOrder(Order o)
        {
            var v = from a in DataSource.orders
                    where o.OrderKey == a.OrderKey
                    select a;
            foreach (var item in v)
            {
                item.Status = o.Status;
                return;
            }
            return;//try catch, this order not exist.
        }

        public void UpdateRequest(GuestRequest t)
        {
            var v = from a in DataSource.guestRequests
                    where a.GuestRequestKey == t.GuestRequestKey
                    select a;
            foreach (var item in DataSource.guestRequests)
            {
                DeleteGuest(item);
                AddRequest(t);//add is doing clone...
                return;
            }
            return;//try catch
        }

        public void UpdateHost(Host host)
        {
            var v = from item in DataSource.hostsList
                    where host.HostId == item.Owner.HostId
                    select item;
            foreach (var item in v)
            {
                item.Owner = new Host
                {
                    FamilyName = host.FamilyName,
                    MailAddress = host.MailAddress,
                    PhoneNumber = host.PhoneNumber,
                    PrivateName = host.PrivateName,
                    HostBankAccount = new BankAccount
                    {
                        BankAccountNumber = host.HostBankAccount.BankAccountNumber,
                        BankName = host.HostBankAccount.BankName,
                        BankNumber = host.HostBankAccount.BankNumber,
                        BranchCity = host.HostBankAccount.BranchCity,
                        BranchAddress = host.HostBankAccount.BranchAddress,
                        BranchNumber = host.HostBankAccount.BranchNumber
                    }
                };
            }
            return;
        }
    }
}
