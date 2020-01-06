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
        public void AddHostingUnit(HostingUnit unit)
        {
            if (unit.HostingUnitKey == 0)
            {
                HostingUnit hostingUnit = unit.Clone();//על פי נספח 1
                hostingUnit.HostingUnitKey = Configuration.serialHostingUnit++;
                hostingUnit.Diary = new bool[12, 31];
                DataSource.hostsList.Add(hostingUnit);
                return;
            }
            UpdateHostingUnit(unit);
        }

        public void AddOrder(Order order)
        {
            if (order.OrderKey == 0)
            {
                Order order1 = order.Clone();
                order1.OrderKey = Configuration.serialOrder++;
                DataSource.orders.Add(order1);
                return;
            }
            UpdateOrder(order);
        }

        public void AddRequest(GuestRequest guest)
        {
            if (guest.GuestRequestKey == 0)
            {
                GuestRequest guestRequest = guest.Clone();
                guestRequest.GuestRequestKey = Configuration.serialGuestRequest++;
                DataSource.guestRequests.Add(guestRequest);
                return;
            }
            UpdateRequest(guest);
        }

        public void DeleteHostingUnit(HostingUnit unit)
        {
            HostingUnit unit1 = new HostingUnit();
            try
            {
                 unit1 = GetHostingUnit(unit.HostingUnitKey);
            }
            catch (Exception)
            {
                throw;//לא נמצא היחידה למחוק אותה
            }        
                DataSource.hostsList.Remove(unit1);
                return;                      
        }


        //public void DeleteOrder(Order o)
        //{
        //    foreach (var item in DataSource.orders)
        //    {
        //        if (item.OrderKey == o.OrderKey)
        //        {
        //            DataSource.orders.Remove(item);
        //            return;
        //        }
        //        return;//try catch
        //    }
        //}

        //public void DeleteGuest(GuestRequest guest)
        //{
        //    foreach (var item in DataSource.guestRequests)
        //    {
        //        if (item.GuestRequestKey == guest.GuestRequestKey)
        //        {
        //            DataSource.guestRequests.Remove(item);
        //            return;
        //        }
        //        return;//try catch, not find request.
        //    }
        //}

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
            var v = from a in GetAllHostingUnit()
                    where a.HostingUnitKey == key
                    select a;
            foreach (var item in v)
                unit = item.Clone();   
            try
            {
                if (unit == null)
                    throw new System.IndexOutOfRangeException();//TODO 
            }
            catch (Exception)
            {
                throw;
            }
            return unit;
        }

        public Order GetOrder(long key)
        {
            Order order = new Order();
            var v = from a in GetAllOrders()
                    where a.OrderKey == key
                    select a;
            foreach (var item in v)
                order = item.Clone();         
            try
            {
                if (order == null)
                    throw new System.IndexOutOfRangeException();//TODO לא נמצאה יחידה  
            }
            catch (Exception)
            {
                throw;
            }
            return order;
        }

        public GuestRequest GetGuestRequest(long key)
        {
            GuestRequest guest = new GuestRequest();
            var v = from a in GetAllGuestRequest()
                    where a.GuestRequestKey == key
                    select a;
            foreach (var item in v)
                guest = item.Clone();
            try
            {
                if (guest == null)
                    throw new System.IndexOutOfRangeException();
            }                                          
            catch (Exception)
            {
                throw;
            }
            return guest;
        }

        public void UpdateHostingUnit(HostingUnit unit)
        {
            HostingUnit hosting = new HostingUnit();
            try
            {
                 hosting = GetHostingUnit(unit.HostingUnitKey);
            }
            catch (Exception)
            {
                throw;//לא נמצא יחידה לעדכן
            }
                DeleteHostingUnit(hosting);                
                DataSource.hostsList.Add(hosting);
                return;            
        }

        public void UpdateOrder(Order order)
        {
            Order order1 = new Order();
            try
            {
                 order1 = GetOrder(order.OrderKey);
            }
            catch (Exception)
            {
                throw;
            }
                order1.Status = order.Status;
                return;                       
        }

        public void UpdateRequest(GuestRequest guestRequest)
        {
            GuestRequest guestRequest1 = new GuestRequest();
            try
            {
                guestRequest1 = GetGuestRequest(guestRequest.GuestRequestKey);
            }
            catch (Exception)
            {
                throw;
            }
            var v = from a in GetAllOrders()
                    where a.GuestRequestKey == guestRequest.GuestRequestKey
                    select a;
            foreach (var item in v)
            {
                item.Status = StatusOrder.RequestChanged;
                UpdateOrder(item);
            }            
            var z = from a in DataSource.guestRequests
                    where a.GuestRequestKey == guestRequest1.GuestRequestKey
                    select a;
            foreach (var item in z)
            {
                item.Status = StatusGuest.Expired;               
            }
            AddRequest(guestRequest1);
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

