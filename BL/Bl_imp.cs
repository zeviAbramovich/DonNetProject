using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using BE;
using DAL;


namespace BL
{
    class Bl_imp : IBL
    {
        public IDal dal = FactoryMethode.GetDal();

        #region helpingFunction

        public static bool ValidateMail(string emailAddress)
        {
            var regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            bool isValid = Regex.IsMatch(emailAddress, regex, RegexOptions.IgnoreCase);
            return isValid;
        }

        public bool IsAllLetters(string s)
        {
            return (Regex.IsMatch(s, @"^[א-ת]+$"));
        }

        public bool CheckAvailableDate(HostingUnit unit, GuestRequest guest)
        {
            for (DateTime date = guest.EntryDate; date < guest.ReleaseDate; date.AddDays(1))
            {
                if (unit.Diary[date.Month, date.Day])
                    return false;
            }
            return true;
        }

        public bool CheckAvailableDateByDateAndSumDays(HostingUnit unit, DateTime date, int VacationDays)
        {
            for (int i = 0; i < VacationDays; i++)
            {
                if (unit.Diary[date.Month, date.Day])
                    return false;
                date.AddDays(1);
            }
            return true;
        }

        public void CreateOrder(GuestRequest guest)
        {
            int count = 0;
            if (guest.Status != StatusGuest.Open)
                return;//TODO try catch,thr order not open.
            List<HostingUnit> hostings = GetAllHostingUnit();
            var v = from a in hostings
                    where a.Area == guest.Area
                    where a.HostingType == guest.HostingType
                    select a;
            foreach (var item in v)
            {
                if (!(CheckAvailableDate(item, guest)))
                    continue;
                if (guest.Adults > item.Adults && guest.Children > item.Children)
                    continue;
                if (guest.SubArea != null && guest.SubArea != item.SubArea)
                    continue;
                if (guest.Pool == Requirements.Necessary && item.Pool == false)
                    continue;
                if (guest.Pool == Requirements.notInterested && item.Pool == true)
                    continue;
                if (guest.Jacuzzi == Requirements.Necessary && item.Jacuzzi == false)
                    continue;
                if (guest.Jacuzzi == Requirements.notInterested && item.Jacuzzi == true)
                    continue;
                if (guest.Garden == Requirements.Necessary && item.Garden == false)
                    continue;
                if (guest.Garden == Requirements.notInterested && item.Garden == true)
                    continue;
                if (guest.ChildrensAttractions == Requirements.Necessary && item.ChildrensAttractions == false)
                    continue;
                if (guest.ChildrensAttractions == Requirements.notInterested && item.ChildrensAttractions == true)
                    continue;
                Order order = new Order()
                {
                    GuestRequestKey = guest.GuestRequestKey,
                    HostingUnitKey = item.HostingUnitKey,
                    CreateDate = DateTime.Now,
                    Status = StatusOrder.NotYetApproved
                };
                count++;
                AddOrder(order);
            }
            if (count == 0)
                return;//TODO try catch have not match.
        }

        #endregion

        #region AddDeleteUpdate       

        public void AddHostingUnit(HostingUnit t)
        {
            if (t.HostingUnitName == "" || !(IsAllLetters(t.HostingUnitName)))
                return;//TODO try catch, or miss name  or not all lleters.
            //if (!(t.Owner.CollectionClearance))
            //    return; try catch, have not CollectionClearance
            if (!(IsAllLetters(t.Owner.PrivateName)) || !(IsAllLetters(t.Owner.FamilyName)))
                return;//try catch, not all the name is words
            if (t.Owner.MailAddress == "")
                return;//try catch, miss mail.
            if (t.Owner.HostBankAccount.BankAccountNumber == 0 || t.Owner.HostBankAccount.BankNumber == 0 || t.Owner.HostBankAccount.BranchNumber == 0)
                return;//try catch, miss or account or branch noumber or bank noumber.
            AddHostingUnit(t);
            return;
        }

        public void AddOrder(Order o)
        {
            if (o.Status == StatusOrder.NotYetApproved)
            {
                dal.AddOrder(o);
                return;
            }
            return;//try catch, error in status of the order
        }

        public void AddRequest(GuestRequest t)
        {
            if (t.EntryDate >= t.ReleaseDate)
                return;//try catch,the date not proper.
            if (t.MailAddress == "" || !(t.MailAddress.Contains("@")))
                return;//try catch,miss mail or not all letters
            if (t.PrivateName == "" || !(IsAllLetters(t.PrivateName)) || t.FamilyName == "" || !(IsAllLetters(t.FamilyName)))
                return;//try catch, has problem in the name
            t.Status = StatusGuest.Open;
            dal.AddRequest(t);
            return;
        }

        public void DeleteGuest(GuestRequest guest)
        {
            if (guest.Status != StatusGuest.Open)
            {
                dal.DeleteGuest(guest);
                return;
            }
            return;//try catch, the guest is open.
        }

        public void DeleteHostingUnit(HostingUnit unit)
        {
            List<Order> orders = GetAllOrders();
            var v = from a in orders
                    where a.HostingUnitKey == unit.HostingUnitKey
                    where a.Status == StatusOrder.MailSent
                    select a;
            foreach (var item in v)
            {
                return;//TODO try catch, לא יכול למחוק יש לפחות הצעה אחת פתוחה
            }
            dal.DeleteHostingUnit(unit);
            return;
        }

        public void DeleteOrder(Order o)
        {
            if (o.Status == StatusOrder.MailSent)
                return;//try catch,לא יכול למחוק , במצב של שליחת מייל 
            dal.DeleteOrder(o);
            return;
        }

        public void UpdateHost(Host host)
        {
            dal.UpdateHost(host);
            return;
        }

        public void UpdateHostingUnit(HostingUnit t)
        {
            if (t.Owner.CollectionClearance == false)
            {
                List<Order> orders = GetAllOrders();
                var v = from a in orders
                        where a.HostingUnitKey == t.HostingUnitKey
                        where a.Status == StatusOrder.MailSent
                        select a;
                foreach (var item in v)
                {
                    return;//try catch, ישנה לפחות הזמנה אחת פתוחה ליחידה ואי אפשר לשנות את הרשאת חיוב 
                }
                dal.UpdateHostingUnit(t);
                return;
            }
            else
            {
                dal.UpdateHostingUnit(t);
                return;
            }
        }

        public void UpdateOrder(Order o)
        {
            HostingUnit unit = GetUnit(o.HostingUnitKey);
            Order order = GetOrder(o.OrderKey);
            GuestRequest guestRequest = GetGuestRequest(o.GuestRequestKey);
            if (!unit.Owner.CollectionClearance)
                return;//TODO try catch, אין אישור גביה
            if (order.Status == StatusOrder.CustomerUnresponsiveness || order.Status == StatusOrder.CustomerResponsiveness)
                return;//try catch, לא יכול לבצע שינויים כי ההזמנה נסגרה
            if (o.Status != StatusOrder.NotYetApproved)
            {
                dal.UpdateOrder(o);
                if (o.Status == StatusOrder.MailSent)
                    Console.WriteLine("mail sent\n");
                if (o.Status == StatusOrder.CustomerResponsiveness)
                {
                    double commission = Configuration.commision;
                    guestRequest.Status = StatusGuest.ClosesBySite;
                    UpdateRequest(guestRequest);
                    for (DateTime date = guestRequest.EntryDate; date < guestRequest.ReleaseDate; date.AddDays(1))
                        unit.Diary[date.Month, date.Day] = true;
                    UpdateHostingUnit(unit);
                    List<Order> orders = dal.GetAllOrders();
                    var v = from a in orders
                            where a.GuestRequestKey == guestRequest.GuestRequestKey
                            where a.Status != StatusOrder.CustomerResponsiveness
                            select a;
                    foreach (var item in v)
                    {
                        item.Status = StatusOrder.CustomerUnresponsiveness;
                        UpdateOrder(item);
                    }
                    return;
                }
                return;
            }
            return;//try catch,beacuse have not update.
        }

        public void UpdateRequest(GuestRequest t)
        {
            if (t.Status != StatusGuest.Open)
                return;//try catch,הזמנה לא תקפה או נסגרה עסקה
            List<Order> orders = GetAllOrders();
            var v = from a in orders
                    where a.GuestRequestKey == t.GuestRequestKey
                    select a;
            foreach (var item in v)
            {
                item.Status = StatusOrder.CustomerUnresponsiveness;
                UpdateOrder(item);
            }
            t.Status = StatusGuest.Expired;
            DeleteGuest(t);
            AddRequest(t);
            return;
        }

        #endregion

        #region getters 

        public List<GuestRequest> AllGuestRequest(Delegate d, List<GuestRequest> l)
        {
            throw new NotImplementedException();
        }

        public HostingUnit GetUnit(long key)
        {
            HostingUnit unit = dal.GetHostingUnit(key);
            return unit;
        }

        public GuestRequest GetGuestRequest(long key)
        {
            GuestRequest request = dal.GetGuestRequest(key);
            return request;
        }

        public Order GetOrder(long key)
        {
            Order order = dal.GetOrder(key);
            return order;
        }

        public List<Branche> GetAllBranches()
        {
            List<Branche> branches = dal.GetAllBranches();
            return branches;
        }

        public List<GuestRequest> GetAllGuestRequest()
        {
            List<GuestRequest> guestRequests = GetAllGuestRequest();
            return guestRequests;
        }

        public List<HostingUnit> GetAllHostingUnit()
        {
            List<HostingUnit> units = dal.GetAllHostingUnit();
            return units;
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = dal.GetAllOrders();
            return orders;
        }

        public List<GuestRequest> GetAllGuestRequestByArea(Area area)
        {
            List<GuestRequest> guestRequests = new List<GuestRequest>();
            var v = from item in GetAllGuestRequest()
                    group item by item.Area into areasInGroup                    
                    select new { area = areasInGroup };                   
            foreach (var group in v)
            {               
                foreach (var ar in group.area)                
                    guestRequests.Add(ar);                
            }
            return guestRequests;
        }

        public List<GuestRequest> GetAllGuestRequestByNumRelax(int num)
        {
            List<GuestRequest> guestRequests = new List<GuestRequest>();
            var v = from a in GetAllGuestRequest()
                    group a by a.Adults + a.Children == num;
            foreach (var item in v)
            {
                guestRequests.Add(item);
            }
            return guestRequests;
        }

        public List<HostingUnit> GetAllHostingUnitByArea(Area area)
        {
            List<HostingUnit> hostingUnits = new List<HostingUnit>();
            var v = from a in GetAllHostingUnit()
                    group a by a.Area == area;
            foreach (var item in v)
            {
                hostingUnits.Add(item);
            }
            return hostingUnits;
        }

        public List<Host> GetAllHostByNumHostingUnit()
        {

        }

        #endregion

        #region SpecialDemands

        public int NumOrdersOfGuest(GuestRequest guestRequest)
        {
            int count = 0;
            var v = from a in GetAllOrders()
                    where a.GuestRequestKey == guestRequest.GuestRequestKey                    
                    select a;
            foreach (var item in v)            
                count++;
            return count;
        }

        public int NumOrdersOfHostingUnit(HostingUnit hostingUnit)
        {
            int count = 0;
            var v = from a in GetAllOrders()
                    where a.HostingUnitKey == hostingUnit.HostingUnitKey
                    where a.Status == StatusOrder.CustomerResponsiveness
                    select a;
            foreach (var item in v)
                count++;
            return count;
        }

        public List<Order> OrderFromNumDaysOrMore(int numDays)
        {
            List<Order> orders = new List<Order>();
            var v = from a in GetAllOrders()
                    let temp = SumDays(a.CreateDate)
                    where temp >= numDays
                    select a;
            foreach (var item in v)           
                orders.Add(item);
            //if (orders.Count == 0)
            //try caych
            return orders;
        }

        public int SumDays(DateTime dateTime)
        {
            DateTime date = DateTime.Now;
            return SumDays(dateTime, date);
        }

        public int SumDays(DateTime dateTime1, DateTime dateTime2)
        {
            TimeSpan time = dateTime2 - dateTime1;
            return time.Days; 
        }

        public List<HostingUnit> UnitsAvailable(DateTime dateTime, int VacationDays)
        {
            List<HostingUnit> units = new List<HostingUnit>();
            var v = from a in GetAllHostingUnit()                 
                    where CheckAvailableDateByDateAndSumDays(a, dateTime, VacationDays) == true
                    select a;
            foreach (var item in v)            
                units.Add(item);
            //if (units.Count == 0)
            //    try catch
            return units;
        }
        #endregion
    }
}
