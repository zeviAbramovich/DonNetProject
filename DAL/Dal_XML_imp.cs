using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BE;

namespace DAL
{
    class Dal_XML_imp : IDal
    {
        public Dal_XML_imp()
        {
            Configuration.serialGuestRequest = long.Parse(DS.DataSourceXML.Configuration.Element("serialGuestRequest").Value);
        }
        #region Add Delete And Update

        public bool AddHostingUnit(HostingUnit unit)
        {
            if (unit.HostingUnitKey == 0)//must be new unit
            {
                HostingUnit hostingUnit = unit.Clone();//על פי נספח 1   

                int temp = int.Parse(DS.DataSourceXML.Configuration.Element("serialHostingUnit").Value);//serial number load
                hostingUnit.HostingUnitKey = ++temp;
                DS.DataSourceXML.Configuration.Element("serialHostingUnit").Value = temp.ToString();
                hostingUnit.Diary = new bool[12, 31];
                DS.DataSourceXML.HostingUnits.Add(hostingUnit.ToXML());
                var a = from item in DS.DataSourceXML.Hosts.Elements()
                        where int.Parse(item.Element("HostId").Value) == hostingUnit.Owner.HostId
                        select item;
                if(!a.Any())
                    DS.DataSourceXML.Hosts.Add(hostingUnit.Owner.ToXML());
                DS.DataSourceXML.SaveHostingUnits();
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
                int temp = int.Parse(DS.DataSourceXML.Configuration.Element("serialOrder").Value);//serial number load
                order1.OrderKey = ++temp;
                DS.DataSourceXML.Configuration.Element("serialOrder").Value = temp.ToString();
                DS.DataSourceXML.Orders.Add(order1.ToXML());
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
                DS.DataSourceXML.Configuration.Element("serialGuestRequest").Value = guestRequest.GuestRequestKey.ToString();
                DS.DataSourceXML.GuestRequests.Add(guestRequest.ToXML());
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
            DS.DataSourceXML.Hosts.Add(host.ToXML());
            return true;
        }

        public bool DeleteHostingUnit(long key)
        {
            HostingUnit unit = new HostingUnit();
            var v = from item in DS.DataSourceXML.HostingUnits.Elements()
                    where long.Parse(item.Element("HostingUnitKey").Value) == key//catch the unit I want to delete
                    select item;
            if (v.Any())
            {
                try
                {
                    XElement a = v.FirstOrDefault();
                    DS.DataSourceXML.HostingUnits.Element(a.Name).RemoveAll();//TODO
                    return true;
                }
                catch (MissingMemberException ms)
                {
                    throw new MissingMemberException("No match key", ms);
                } 
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
            return (from o in DS.DataSourceXML.Orders.Elements("Order")
                    select o.ToString().ToObject<Order>()).ToList();
        }

        public List<Host> GetAllHosts()
        {
            return (from o in DS.DataSourceXML.Hosts.Elements("Host")
                    select o.ToString().ToObject<Host>()).ToList();
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
    

