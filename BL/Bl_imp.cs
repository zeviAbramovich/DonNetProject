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
        public IDal dal = DAL.FactoryMethode.GetDal();

        #region helpingFunction
        /// <summary>
        /// בודק האם יש ליחידה הזמנות במצב פתוח 
        /// </summary>
        /// <param name=" מספר יחידה"></param>
        /// <returns>
        /// אם יש מחזיר שקר אם אין מחזיר אמת
        /// </returns>
        public bool ChecksWhethertheUnitHasOpenOrders(long unitKey)
        {
            var v = from item in GetAllOrders()
                    where item.HostingUnitKey == unitKey
                    where item.Status == StatusOrder.MailSent
                    select item;
            if (v.Any())
                return false;
            return true;
        }


        /// <summary>
        /// מקבל בקשה ויחידה, ובודק האם הזמן המצוין בבקשה פנוי ביחידה
        /// </summary>
        /// <param name="יחידת אירוח"></param>
        /// <param name="בקשת לקוח"></param>
        /// <returns>
        /// אם פנוי מחזיר אמת, אם לא מחזיר שקר
        /// </returns>
        /// 
        public bool CheckAvailableDate(HostingUnit unit, GuestRequest guest)
        {
            DateTime date = guest.EntryDate;
            while (date < guest.ReleaseDate)
            {
                if (unit.Diary[date.Month, date.Day])
                    return false;
                date = date.AddDays(1);
            }
            return true;
        }

        /// <summary>
        /// מקבל יחידה,תאריך,ומספר ימי חופשה,ובודקת האם היחידה פנויה מהתאריך שהתקבל עד סוף ימי החופשה
        /// </summary>
        /// <param name="יחידת אירוח"></param>
        /// <param name="תאריך כניסה"></param>
        /// <param name="מספר ימי חופשה"></param>
        /// <returns>
        /// אם פנוי מחזיר אמת, אם לא מחזיר שקר
        /// </returns>
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

        /// <summary>
        /// מקבל בקשת לקוח ומייצר הזמנות לפי ההתאמות שהפונקציה מוצאת
        /// </summary>
        /// <param name="בקשת לקוח"></param>
        /// <returns>
        /// אם לא נוצרו הזמנות מחזיר שקר, אחרת מחזיר אמת
        /// </returns>
        public bool CreateOrder(GuestRequest guest)
        {
            if (guest.Status != StatusGuest.Open)
                throw new CannotAddException("cannot add order beacuse has problem with the status of request");

            var orders = GetValidatedOrders(guest);

            if (orders.Count == 0)
                return false;

            AddOrders(orders);          

            return true;
        }

        private List<Order> GetValidatedOrders(GuestRequest guest)
        {
            var units = GetAllHostingUnit();

            var orders = units.Where(unit =>
            {
                var isValidOrder = unit.Area == guest.Area &&
                unit.HostingType == guest.HostingType &&
                ValidateRequestOrder(unit, guest);
                return isValidOrder;

            }).Select(unit => new Order
            {
                GuestRequestKey = guest.GuestRequestKey,
                HostingUnitKey = unit.HostingUnitKey,
                CreateDate = DateTime.Now,
                Status = StatusOrder.NotYetApproved,
                OrderDate = new DateTime()
            }).ToList();

            return orders;
        }

        private bool ValidateRequestOrder(HostingUnit item, GuestRequest guest)
        {
            if (!(CheckAvailableDate(item, guest)))
                return false;
            if (guest.Adults > item.Adults && guest.Children > item.Children)
                return false;
            if (guest.SubArea != "" && guest.SubArea != item.SubArea)
                return false;
            if (guest.Pool == Requirements.Necessary && item.Pool == false)
                return false;
            if (guest.Pool == Requirements.notInterested && item.Pool == true)
                return false;
            if (guest.Jacuzzi == Requirements.Necessary && item.Jacuzzi == false)
                return false;
            if (guest.Jacuzzi == Requirements.notInterested && item.Jacuzzi == true)
                return false;
            if (guest.Garden == Requirements.Necessary && item.Garden == false)
                return false;
            if (guest.Garden == Requirements.notInterested && item.Garden == true)
                return false;
            if (guest.ChildrensAttractions == Requirements.Necessary && item.ChildrensAttractions == false)
                return false;
            if (guest.ChildrensAttractions == Requirements.notInterested && item.ChildrensAttractions == true)
                return false;

            return true;
        }

        /// <summary>
        /// בודק כמה יחידות מחזיק מארח
        /// </summary>
        /// <param name="מארח"></param>
        /// <returns>
        /// מספר יחידות
        /// </returns>
        public int NumOfUnits(Host host)
        {
            var num = from item in GetAllHostingUnit()
                      where item.Owner.HostId == host.HostId
                      select item;
            return num.Count();
        }

        #endregion

        #region AddDeleteUpdate       

        public bool AddHostingUnit(HostingUnit unit)
        {
            try
            {
                dal.AddHostingUnit(unit);
            }
            catch (CannotUpdateException cue)
            {
                throw new CannotAddException("The key must be empty or with correct key for update ", cue);
            }
            return true;
        }

        public void AddOrders(IEnumerable<Order> orders)
        {
            orders.ForEach(AddOrder);         
        }


        public bool AddOrder(Order order)
        {
            if (order.Status != StatusOrder.NotYetApproved)
                throw new CannotAddException("have problem with the status");

            try
            {
                dal.AddOrder(order);
            }
            catch (CannotAddException cae)
            {
                throw cae;
            }
            return true;
        }

        public bool AddRequest(GuestRequest request)
        {
            if (request.EntryDate >= request.ReleaseDate || request.EntryDate < DateTime.Now)
                throw new CannotAddException("the entry date is after the relasing date / or your entry date is befor today");
            //insert serial number here becouse we create new orders and we need key number for the guest request.
            request.GuestRequestKey = ++Configuration.serialGuestRequest;
            request.Status = StatusGuest.Open;
            try
            {
                dal.AddRequest(request);
                CreateOrder(request);
            }
            catch (CannotAddException cae)
            {
                throw cae;
            }
            return true;
        }

        public bool DeleteHostingUnit(long key)
        {
            List<Order> orders = GetAllOrders();
            var v = from a in orders
                    where a.HostingUnitKey == key
                    where a.Status == StatusOrder.MailSent
                    select a;
            if (v.Any())
                throw new CannotDeleteException("Cannot delete! There is at least one open order");
            try
            {
                dal.DeleteHostingUnit(key);
            }
            catch (MissingMemberException mme)
            {
                throw new CannotDeleteException("cannot delete beacuse ", mme);
            }
            return true;
        }

        public bool AddHost(Host host)
        {
            var v = from item in GetAllHosts()
                    where item.HostId == host.HostId
                    select item;
            if (v.Any())
                throw new CannotAddException("already exist ");
            dal.AddHost(host);
            return true;

        }

        /// <summary>
        /// מעדכן פרטי מארח בכל היחידות
        /// </summary>
        /// <param name="מארח"></param>
        /// <returns>
        /// אם הצליח לעדכן מחזיר אמת, אם לא אז זורק חריגה
        /// </returns>
        public bool UpdateHost(Host host)
        {
            try
            {
                dal.UpdateHost(host);
            }
            catch (MissingMemberException cue)
            {
                throw new CannotUpdateException("cannot update host beacuse ", cue);
            }
            return true;
        }

        public bool UpdateHostingUnit(HostingUnit unit)
        {
            if (unit.Owner.CollectionClearance == false)//if the change is the collection clearance is false
            {
                if (!ChecksWhethertheUnitHasOpenOrders(unit.HostingUnitKey))
                    throw new CannotUpdateException("Can't remove debit authorization for account because there is at least one open order ");
                dal.UpdateHostingUnit(unit);
                return true;
            }
            dal.UpdateHostingUnit(unit);
            return true;
        }

        public bool UpdateOrder(Order orderUpdate)
        {
            HostingUnit unit = new HostingUnit();
            Order order = new Order();
            GuestRequest request = new GuestRequest();
            try
            {
                unit = GetUnit(orderUpdate.HostingUnitKey);
                order = GetOrder(orderUpdate.OrderKey);
                request = GetGuestRequest(orderUpdate.GuestRequestKey);
            }
            catch (MissingMemberException mme)
            {
                throw new CannotUpdateException("cannot update order beacuse ", mme);
            }
            //If the original unit does not have a collection clearance, then I will not be able to change order status
            if (!unit.Owner.CollectionClearance)
                throw new CannotUpdateException("the Owner " + unit.Owner.PrivateName + " " + unit.Owner.FamilyName + " did not settle a payment agreement");
            //if the original order has its status closed (no matter what the reason) i don't want to change the order          
            if (order.Status == StatusOrder.CustomerUnresponsiveness || order.Status == StatusOrder.CustomerResponsiveness || order.Status == StatusOrder.RequestChanged)
                throw new CannotUpdateException("the Order number:" + order.OrderKey + " is closed");

            if (orderUpdate.Status == StatusOrder.NotYetApproved)
                throw new CannotUpdateException("order " + order.OrderKey + " already in progress...");

            //if the change I made is because the customer closed because he didn't want to, or because this order was closed because the request was changed
            if (orderUpdate.Status == StatusOrder.CustomerUnresponsiveness || orderUpdate.Status == StatusOrder.RequestChanged)
            {
                order.Status = orderUpdate.Status;
                dal.UpdateOrder(order);//לא עשיתי טרי קאטצ כי מצאתי את ההזמנה הזאת בתחילת הפונקציה
                return true;
            }
            //if the change I made is that I sent an email to the customer
            if (orderUpdate.Status == StatusOrder.MailSent)
            {
                orderUpdate.Status = StatusOrder.MailSent;
                orderUpdate.OrderDate = DateTime.Now;
                dal.UpdateOrder(orderUpdate);
                return true;
            }
            //if the change I made is that the customer has closed the deal because he wants to
            if (orderUpdate.Status == StatusOrder.CustomerResponsiveness)
            {
                orderUpdate.Commision = Configuration.commision * SumDays(request.EntryDate, request.ReleaseDate);
                request.Status = StatusGuest.ClosesBySite;//מעדכן בקשה נסגרה כי הלקוח רצה
                for (DateTime date = request.EntryDate; date < request.ReleaseDate; date = date.AddDays(1))
                {
                    unit.Diary[date.Month, date.Day] = true;//מעדכן את המטריצה בימים שהלקוח רצה 
                    unit.bookDates.Add(date);
                }
                UpdateHostingUnit(unit);
                UpdateRequest(request);
                dal.UpdateOrder(orderUpdate);
                List<Order> orders = GetAllOrders();
                var v = from a in orders
                        where a.GuestRequestKey == request.GuestRequestKey
                        where a.Status != StatusOrder.CustomerResponsiveness
                        select a;
                foreach (var item in v)//changes the status of the rest customer orders 
                {
                    item.Status = StatusOrder.RequestChanged;
                    dal.UpdateOrder(item);
                }
                return true;
            }
            return true;
        }

        public bool UpdateRequest(GuestRequest updateRequest)
        {
            GuestRequest oldRequest = new GuestRequest();
            try
            {
                oldRequest = GetGuestRequest(updateRequest.GuestRequestKey);
            }
            catch (MissingMemberException mme)
            {
                throw new CannotUpdateException("cannot update request beacuse ", mme);
            }
            //if the original order is not open
            if (oldRequest.Status != StatusGuest.Open)
                throw new CannotUpdateException("Request number: " + updateRequest.GuestRequestKey.ToString() + " is closed!");
            //if the status of the update is open, 
            //then I have to close the rest of the order 
            //and the original request becomes expired, and add the new request
            if (updateRequest.Status == StatusGuest.Open)
            {
                List<Order> orders = GetAllOrders();
                var v = from a in orders
                        where a.GuestRequestKey == updateRequest.GuestRequestKey
                        select a;
                foreach (var item in v)
                {
                    item.Status = StatusOrder.CustomerUnresponsiveness;
                    UpdateOrder(item);
                }
                oldRequest.Status = StatusGuest.Expired;//cancle old request
                dal.UpdateRequest(oldRequest);
                AddRequest(updateRequest);//add the update request
                return true;
            }
            //if the status of the update is closed,beacuse the customer wanted, or expired
            if (updateRequest.Status == StatusGuest.ClosesBySite || updateRequest.Status == StatusGuest.Expired)
            {
                dal.UpdateRequest(updateRequest);
                return true;
            }
            return true;
        }

        #endregion

        #region getters 

        public HostingUnit GetUnit(long key)
        {
            HostingUnit unit = new HostingUnit();
            try
            {
                unit = dal.GetHostingUnit(key);
            }
            catch (MissingMemberException mme)
            {
                throw mme;
            }
            return unit;
        }

        public GuestRequest GetGuestRequest(long key)
        {
            GuestRequest request = new GuestRequest();
            try
            {
                request = dal.GetGuestRequest(key);
            }
            catch (MissingMemberException mme)
            {
                throw mme;
            }
            return request;
        }

        public Order GetOrder(long key)
        {
            Order order = new Order();
            try
            {
                order = dal.GetOrder(key);
            }
            catch (MissingMemberException mme)
            {
                throw mme;
            }
            return order;
        }

        public Host GetHost(long key)
        {
            Host host = new Host();
            try
            {
                host = dal.GetHost(key);
            }
            catch (MissingMemberException mme)
            {

                throw mme;
            }
            return host;
        }

        public List<Branche> GetAllBranches()
        {
            List<Branche> branches = dal.GetAllBranches();
            return branches;
        }

        public List<GuestRequest> GetAllGuestRequest()
        {
            List<GuestRequest> guestRequests = dal.GetAllGuestRequests();
            return guestRequests;
        }

        public List<HostingUnit> GetAllHostingUnit()
        {
            List<HostingUnit> units = dal.GetAllHostingUnits();
            return units;
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = dal.GetAllOrders();
            return orders;
        }

        public List<GuestRequest> GuestRequestCondition(delegateRequest requestCondition)
        {
            var v = from item in dal.GetAllGuestRequests()
                    where requestCondition(item)
                    select item;
            return v.ToList();
        }

        /// <summary>
        /// get all the orders of the giving host key
        /// </summary>
        /// <param name="hostkey"></param>
        /// <returns>list<order></returns>
        public List<Order> GetAllHostOrders(long hostkey)
        {
            List<Order> orders = new List<Order>();
            var v = from itemOrder in GetAllOrders()
                    from itemUnit in GetAllHostingUnit()
                    where itemUnit.Owner.HostId == hostkey
                    where itemOrder.HostingUnitKey == itemUnit.HostingUnitKey
                    select itemOrder;
            foreach (var item in v)
            {
                orders.Add(item);
            }
            return orders;
        }
        /// <summary>
        /// get all the units belong to the host
        /// </summary>
        /// <param name="key"></param>
        /// <returns>list<HostingUnit></returns>
        public List<HostingUnit> GetAllHostUnits(long key)
        {
            List<HostingUnit> units = new List<HostingUnit>();
            var v = from item in GetAllHostingUnit()
                    where item.Owner.HostId == key
                    select item;
            foreach (var item in v)
            {
                units.Add(item);
            }
            return units;
        }

        public List<Host> GetAllHosts()
        {
            List<Host> hosts = dal.GetAllHosts();
            return hosts;
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
            if (!v.Any())
                Console.WriteLine("have not orders ");
            foreach (var item in v)
                orders.Add(item);
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
            if (!v.Any())
                Console.WriteLine("have not units available");
            foreach (var item in v)
                units.Add(item);
            return units;
        }
        #endregion

        #region Grouping
        public IEnumerable<IGrouping<Area, GuestRequest>> GetAllGuestRequestByArea(IEnumerable<GuestRequest> guestRequests)
        {
            var guestAr = from item in guestRequests
                          group item by item.Area into guest
                          select guest;
            return guestAr;


        }
        public IEnumerable<IGrouping<int, GuestRequest>> GetAllGuestRequestByNumRelax(IEnumerable<GuestRequest> guestRequests)
        {
            var guestNuA = from item in guestRequests
                           group item by item.Adults + item.Children into guest
                           select guest;
            return guestNuA;
        }
        public IEnumerable<IGrouping<Area, HostingUnit>> GetAllHostingUnitByArea(IEnumerable<HostingUnit> hostingUnits)
        {

            var hostAr = from item in hostingUnits
                         group item by item.Area into host
                         select host;
            return hostAr;
        }
        public IEnumerable<IGrouping<int, Host>> GetAllHostByNumHostingUnit(IEnumerable<Host> hosts)
        {
            var hostNuUnit = from item in GetAllHostingUnit()
                             let owner = item.Owner
                             group owner by NumOfUnits(owner);
            return hostNuUnit;
        }

        #endregion
    }
}
