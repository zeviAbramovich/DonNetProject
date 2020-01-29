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
            if (unit.HostingUnitKey == 0)//must be new unit
            {
                HostingUnit hostingUnit = unit.Clone();//על פי נספח 1
                hostingUnit.HostingUnitKey = ++Configuration.serialHostingUnit;
                hostingUnit.Diary = new bool[12, 31];
                DataSource.hostingUnitList.Add(hostingUnit);
                if (!DataSource.hosts.Any(x => x.HostId != unit.Owner.HostId))
                    DataSource.hosts.Add(unit.Owner);
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
                DataSource.orders.Add(order1);
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
            if (guest.GuestRequestKey == 0)//must be new request
            {
                GuestRequest guestRequest = guest.Clone();
                guestRequest.GuestRequestKey = ++Configuration.serialGuestRequest;
                DataSource.guestRequests.Add(guestRequest);
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

        public bool DeleteHostingUnit(long key)
        {
            HostingUnit unit = new HostingUnit();
            var v = from item in DataSource.hostingUnitList
                    where item.HostingUnitKey == key//catch the unit I want to delete
                    select item;
            foreach (HostingUnit item in v)
                unit = item;//unit is originally unit
            try
            {
                DataSource.hostingUnitList.Remove(unit);    
                return true;
            }
            catch (MissingMemberException ms)
            {
                throw new MissingMemberException ("No match key",ms);
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
            hosting = DataSource.hostingUnitList.FirstOrDefault(x => x.HostingUnitKey == updateUnit.HostingUnitKey);//hosting is the originally unit
            DataSource.hostingUnitList.Remove(hosting);//delete the originally
            DataSource.hostingUnitList.Add(updateUnit);//add the originally
            return true;
        }

        public bool UpdateOrder(Order updateOrder)
        {
            Order originalOrder = DataSource.orders.FirstOrDefault(c => c.OrderKey == updateOrder.OrderKey);
            if (originalOrder == null)
                throw new CannotUpdateException("Cannot update! order number " + updateOrder.OrderKey.ToString() + "not exsist");
            originalOrder.Status = updateOrder.Status;
            originalOrder.Commision = updateOrder.Commision;
            HostingUnit unit = DataSource.hostingUnitList.Find(x => x.HostingUnitKey == updateOrder.HostingUnitKey);
            unit.SumComission += updateOrder.Commision;
            return true;
        }

        public bool UpdateRequest(GuestRequest updateRequest)
        {
            GuestRequest OriginalRequest = DataSource.guestRequests.Find(g => g.GuestRequestKey == updateRequest.GuestRequestKey);
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
            if (updateRequest.Status == StatusGuest.ClosesBySite|| updateRequest.Status == StatusGuest.Expired)
            {
                OriginalRequest.Status = updateRequest.Status;
                return true;
            }
            return true;
        }

        public bool UpdateHost(Host host)
        {
            Host hostOriginal = DataSource.hosts.FirstOrDefault(x => x.HostId == host.HostId);
            DataSource.hosts.Remove(hostOriginal);
            DataSource.hosts.Add(host);
            var v = from item in DataSource.hostingUnitList
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
            if (tempOrder == null)
                throw new MissingMemberException("did not find order");
            Order order = tempOrder.Clone();
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

        public Host GetHost(long key)
        {
            Host host = new Host();
            HostingUnit unit = new HostingUnit();
            try
            {
                unit = DataSource.hostingUnitList.FirstOrDefault(x => x.Owner.HostId == key);
                host = unit.Owner;
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

