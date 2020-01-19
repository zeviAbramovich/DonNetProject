using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

namespace DAL
{
    //בעיקרון יכולתי להשתמש רק בגטרים של יחידה בודדת או הזמנה בודדת ולא להשתמש בכל השאילתות
    //,אבל זו היתה דרישתכם ובאמת אצל דן ביטלו את הדרישה הזו
    class Dal_imp : IDal
    {
        #region Add Delete And Update

        public bool AddHostingUnit(HostingUnit unit)
        {
            if (unit.HostingUnitKey == 0)
            {
                HostingUnit hostingUnit = unit.Clone();//על פי נספח 1
                hostingUnit.HostingUnitKey = ++Configuration.serialHostingUnit;
                hostingUnit.Diary = new bool[12, 31];
                DataSource.hostingUnitList.Add(hostingUnit);
                if (!DataSource.hosts.Any(x => x.HostId != unit.Owner.HostId))
                    DataSource.hosts.Add(unit.Owner);
                return true;
            }
            try
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
            if (order.OrderKey == 0)
            {
                Order order1 = order.Clone();
                order1.OrderKey = Configuration.serialOrder++;
                DataSource.orders.Add(order1);
                return true;
            }
            try
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
            if (guest.GuestRequestKey == 0)
            {
                GuestRequest guestRequest = guest.Clone();
                guestRequest.GuestRequestKey = ++Configuration.serialGuestRequest;
                DataSource.guestRequests.Add(guestRequest);
                return true;
            }
            try
            {
                UpdateRequest(guest);
            }
            catch (CannotUpdateException cue)
            {
                throw new CannotAddException("the key must be empty or correct key for update", cue);
            }
            return true;
        }

        public bool DeleteHostingUnit(long key)
        {
            HostingUnit unit = new HostingUnit();
            var v = from item in DataSource.hostingUnitList
                    where item.HostingUnitKey == key
                    select item;
            foreach (HostingUnit item in v)
                unit = item;
            try
            {
                DataSource.hostingUnitList.Remove(unit);    
                return true;
            }
            catch (MissingMemberException ms)
            {
                throw new MissingMemberException ("No match key",ms);
            }
            catch(NullReferenceException nre)
            {
                throw new MissingMemberException("",nre);
            }          
        }

        public bool UpdateHostingUnit(HostingUnit unit)
        {
            HostingUnit hosting = new HostingUnit();
            try
            {
                hosting = GetHostingUnit(unit.HostingUnitKey);
            }
            catch (MissingMemberException me)
            {
                throw new CannotUpdateException("Hosting Unit number " + unit.HostingUnitKey + " not found", me);
            }
            hosting.HostingUnitName = unit.HostingUnitName;
            DeleteHostingUnit(unit.HostingUnitKey);
            DataSource.hostingUnitList.Add(hosting);
            return true;
        }

        public bool UpdateOrder(Order order)
        {
            Order order1 = DataSource.orders.FirstOrDefault(c => c.OrderKey == order.OrderKey);
            if (order1 == null)
                throw new CannotUpdateException("Cannot update! order number " + order.OrderKey.ToString() + "not exsist");
            return true;
        }

        public bool UpdateRequest(GuestRequest updateRequest)
        {
            GuestRequest request = DataSource.guestRequests.Find(g => g.GuestRequestKey == updateRequest.GuestRequestKey);
            if (request == null)           
                throw new CannotUpdateException("There is no RequestKey " + updateRequest.GuestRequestKey.ToString());                      
            request.Status = StatusGuest.Expired;
            AddRequest(request);//מעדכן מה שרציתי
            return true;
        }

        public bool UpdateHost(Host host)
        {
            var v = from item in DataSource.hostingUnitList
                    where host.HostId == item.Owner.HostId
                    select item;
            if (!v.Any())
                throw new MissingMemberException("Host ID not exsist");
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
            foreach (var item in DataSource.hostingUnitList)
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
            HostingUnit tempUnit = DataSource.hostingUnitList.FirstOrDefault(x => x.HostingUnitKey == key);
            if (tempUnit == null)
                throw new MissingMemberException("did not find unit");
            HostingUnit unit = tempUnit.Clone();
            return unit;
        }

        public Order GetOrder(long key)
        {
            Order tempOrder = DataSource.orders.FirstOrDefault(x => x.OrderKey == key);
            Order order = tempOrder.Clone();
            if (order == null)
                throw new MissingMemberException("did not find order");
            return order;
        }

        public GuestRequest GetGuestRequest(long key)
        {
            GuestRequest tempRequest = DataSource.guestRequests.FirstOrDefault(x => x.GuestRequestKey == key);//check if exist
            if (tempRequest == null)
                throw new MissingMemberException("did not find guest request",this.GetType().ToString());
            GuestRequest guest = tempRequest.Clone();
            return guest;
        }

        #endregion
    }
}

