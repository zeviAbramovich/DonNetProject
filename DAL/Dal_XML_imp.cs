﻿using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
//using BE;

namespace DAL
{
    class Dal_XML_imp : IDal
    {
        #region Add Delete And Update

        public bool AddHostingUnit(HostingUnit unit)
        {
            if (unit.HostingUnitKey == 0)//must be new unit
            {
                HostingUnit hostingUnit = unit.Clone();//על פי נספח 1     
                XElement xElement;
                xElement = XElement.Load("serialHostingUnit");
                BE.
                int temp = int.Parse(XEl)
                hostingUnit.Diary = new bool[12, 31];
                DS.DataSource.hostingUnitList.Add(hostingUnit);
                if (!DS.DataSource.hosts.Any(x => x.HostId != unit.Owner.HostId))
                    DS.DataSource.hosts.Add(unit.Owner);
                return true;
            }
            try//The key is not 0, so I'm posting an update 
            {
                UpdateHostingUnit(unit);
            }
            catch (CannotUpdateException cue)
            {
                throw new CannotAddException("the key must be empty or correct key for update", cue);
            }
            return true;
        }

        public bool AddOrder(Order order)
        {
            if (order.OrderKey == 0)//must be new order
            {
                Order order1 = order.Clone();
                order1.OrderKey = ++Configuration.serialOrder;
                DS.DataSource.orders.Add(order1);
                return true;
            }
            try//The key is not 0, so I'm posting an update 
            {
                UpdateOrder(order);
            }
            catch (CannotUpdateException cue)
            {
                throw new CannotAddException("the key must be empty or correct key for update", cue);
            }
            return true;
        }

        public bool AddRequest(GuestRequest guest)
        {
            var v = from item in GetAllGuestRequests()
                    where item.GuestRequestKey == guest.GuestRequestKey
                    select item;
            if (!v.Any())//must be new request
            {
                GuestRequest guestRequest = guest.Clone();
                //guestRequest.GuestRequestKey = ++Configuration.serialGuestRequest;
                guestRequest.RegistrationDate = DateTime.Now;
                //guestRequest.Status = StatusGuest.Open;
                DS.DataSource.guestRequests.Add(guestRequest);
                return true;
            }
            try
            {
                UpdateRequest(guest);//The key is not 0, so I'm posting an update 
            }
            catch (CannotUpdateException cue)
            {
                throw new CannotAddException("the key must be empty or correct key for update", cue);
            }
            return true;
        }

        public bool AddHost(Host host)
        {
            DS.DataSource.hosts.Add(host);
            return true;
        }

        public bool DeleteHostingUnit(long key)
        {
            HostingUnit unit = new HostingUnit();
            var v = from item in DS.DataSource.hostingUnitList
                    where item.HostingUnitKey == key//catch the unit I want to delete
                    select item;
            foreach (HostingUnit item in v)
                unit = item;//unit is originally unit
            try
            {
                DS.DataSource.hostingUnitList.Remove(unit);
                return true;
            }
            catch (MissingMemberException ms)
            {
                throw new MissingMemberException("No match key", ms);
            }
        }

        public bool UpdateHostingUnit(HostingUnit updateUnit)
        {
            HostingUnit hosting = new HostingUnit();
            try
            {
                hosting = GetHostingUnit(updateUnit.HostingUnitKey);
            }
            catch (MissingMemberException me)
            {
                throw new CannotUpdateException("Hosting Unit number " + updateUnit.HostingUnitKey + " not found", me);
            }
            hosting = DS.DataSource.hostingUnitList.FirstOrDefault(x => x.HostingUnitKey == updateUnit.HostingUnitKey);//hosting is the originally unit
            DS.DataSource.hostingUnitList.Remove(hosting);//delete the originally
            DS.DataSource.hostingUnitList.Add(updateUnit);//add the originally
            return true;
        }

        public bool UpdateOrder(Order updateOrder)
        {
            Order originalOrder = DS.DataSource.orders.FirstOrDefault(c => c.OrderKey == updateOrder.OrderKey);
            if (originalOrder == null)
                throw new CannotUpdateException("Cannot update! order number " + updateOrder.OrderKey.ToString() + "not exsist");
            originalOrder.Status = updateOrder.Status;
            originalOrder.Commision = updateOrder.Commision;
            HostingUnit unit = DS.DataSource.hostingUnitList.Find(x => x.HostingUnitKey == updateOrder.HostingUnitKey);
            unit.SumComission += updateOrder.Commision;
            return true;
        }

        public bool UpdateRequest(GuestRequest updateRequest)
        {
            GuestRequest OriginalRequest = DS.DataSource.guestRequests.Find(g => g.GuestRequestKey == updateRequest.GuestRequestKey);
            if (OriginalRequest == null)
                throw new CannotUpdateException("There is no RequestKey " + updateRequest.GuestRequestKey.ToString());
            //if my status change is open, then I close the old request and add the new one
            if (updateRequest.Status == StatusGuest.Open)
            {
                OriginalRequest.Status = StatusGuest.Expired;
                AddRequest(OriginalRequest);//מעדכן מה שרציתי
                return true;
            }
            ////if my status change is not open, then i update the status
            if (updateRequest.Status == StatusGuest.ClosesBySite || updateRequest.Status == StatusGuest.Expired)
            {
                OriginalRequest.Status = updateRequest.Status;
                return true;
            }
            return true;
        }

        public bool UpdateHost(Host host)
        {
            Host hostOriginal = DS.DataSource.hosts.FirstOrDefault(x => x.HostId == host.HostId);
            DS.DataSource.hosts.Remove(hostOriginal);
            DS.DataSource.hosts.Add(host);
            var v = from item in DS.DataSource.hostingUnitList
                    where item.Owner.HostId == host.HostId
                    select item;
            foreach (HostingUnit item in v)
            {
                item.Owner.FamilyName = host.FamilyName;
                item.Owner.MailAddress = host.MailAddress;
                item.Owner.PhoneNumber = host.PhoneNumber;
                item.Owner.PrivateName = host.PrivateName;
                item.Owner.HostBankAccount.BankAccountNumber = host.HostBankAccount.BankAccountNumber;
                item.Owner.HostBankAccount.BankName = host.HostBankAccount.BankName;
                item.Owner.HostBankAccount.BankNumber = host.HostBankAccount.BankNumber;
                item.Owner.HostBankAccount.BranchAddress = host.HostBankAccount.BranchAddress;
                item.Owner.HostBankAccount.BranchCity = host.HostBankAccount.BranchCity;
                item.Owner.HostBankAccount.BranchNumber = host.HostBankAccount.BranchNumber;
            }
            return true;
        }
        #endregion

        #region getters
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
        /// <summary>
        /// get only the not expired requests
        /// </summary>
        /// <returns>
        /// List<GuestRequest>
        /// </returns>
        public List<GuestRequest> GetAllGuestRequests()
        {
            List<GuestRequest> guestRequests = new List<GuestRequest>();
            var v = from item in DS.DataSource.guestRequests
                    where item.Status != StatusGuest.Expired
                    select item;
            foreach (var item in v)
                guestRequests.Add(item.Clone());
            return guestRequests;
        }

        public List<HostingUnit> GetAllHostingUnits()
        {
            List<HostingUnit> hostingUnits = new List<HostingUnit>();
            foreach (var item in DS.DataSource.hostingUnitList)
                hostingUnits.Add(item.Clone());
            return hostingUnits;
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();
            foreach (var item in DS.DataSource.orders)
                orders.Add(item.Clone());
            return orders;
        }

        public List<Host> GetAllHosts()
        {
            List<Host> hosts = new List<Host>();
            foreach (var item in DS.DataSource.hosts)
                hosts.Add(item.Clone());
            return hosts;
        }

        public HostingUnit GetHostingUnit(long key)
        {
            HostingUnit tempUnit = DS.DataSource.hostingUnitList.FirstOrDefault(x => x.HostingUnitKey == key);
            if (tempUnit == null)
                throw new MissingMemberException("did not find unit");
            HostingUnit unit = tempUnit.Clone();
            return unit;
        }

        public Order GetOrder(long key)
        {
            Order tempOrder = DS.DataSource.orders.FirstOrDefault(x => x.OrderKey == key);
            if (tempOrder == null)
                throw new MissingMemberException("did not find order");
            Order order = tempOrder.Clone();
            return order;
        }

        public GuestRequest GetGuestRequest(long key)
        {
            GuestRequest tempRequest = DS.DataSource.guestRequests.FirstOrDefault(x => x.GuestRequestKey == key);//check if exist
            if (tempRequest == null)
                throw new MissingMemberException("did not find guest request", this.GetType().ToString());
            GuestRequest guest = tempRequest.Clone();
            return guest;
        }

        public Host GetHost(long key)
        {
            Host host = new Host();

            try
            {
                host = DS.DataSource.hosts.FirstOrDefault(x => x.HostId == key).Clone();

            }
            catch (ArgumentNullException)
            {

                throw new MissingMemberException("not found the Host in the list");
            }
            return host;
        }

        #endregion
    }
}
    

