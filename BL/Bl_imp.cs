﻿using System;
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

        public bool CheckAvailableDate(HostingUnit unit, GuestRequest guest)
        {
            DateTime date = guest.EntryDate;
            while (date<guest.ReleaseDate)
            {
                if (unit.Diary[date.Month, date.Day])
                    return false;
                date = date.AddDays(1);
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

        public bool CreateOrder(GuestRequest guest)
        {
            int count = 0;
            if (guest.Status != StatusGuest.Open)
                throw new CannotAddException("cannot add order beacuse has problem with the status of request");
            var v = from a in GetAllHostingUnit()
                    where a.Area == guest.Area
                    where a.HostingType == guest.HostingType
                    select a;
            foreach (var item in v)
            {
                if (!(CheckAvailableDate(item, guest)))
                    continue;
                if (guest.Adults > item.Adults && guest.Children > item.Children)
                    continue;
                if (guest.SubArea != "" && guest.SubArea != item.SubArea)
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
                return false;//לא נמצאה יחידה העונה לבקשת הלקוח
            return true;
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
            if (request.EntryDate >= request.ReleaseDate)
                throw new CannotAddException("the entry date is after the relasing date");
            request.Status = StatusGuest.Open;
            try
            {
                dal.AddRequest(request);
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
        /// <summary>
        /// updete the host details in all the units.
        /// </summary>
        /// <param name="host"></param>
        /// <returns>
        /// true or false
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
           
            HostingUnit hostingUnit = new HostingUnit();
            if (unit.Owner.CollectionClearance == false)
            {                
               if( !ChecksWhethertheUnitHasOpenOrders(unit.HostingUnitKey))
                    throw new CannotUpdateException("Can't remove debit authorization for account because there is at least one open order ");                                                  
                dal.UpdateHostingUnit(unit);
                return true;
            }
            else
            {
                dal.UpdateHostingUnit(unit);
                return true;
            }

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
            //"בעל יחידת אירוח יוכל לשלוח הזמנה ללקוח רק אם חתם על הרשאה"
            if (!unit.Owner.CollectionClearance)
                throw new CannotUpdateException("the Owner " + unit.Owner.PrivateName + " " + unit.Owner.FamilyName + " did not settle a payment agreement");
            //"כאשר סטטוס הזמנה משתנה לסגירת עסקה - לא ניתן לשנות יותר את הסטטוס שלה"          
            if (order.Status == StatusOrder.CustomerUnresponsiveness || order.Status == StatusOrder.CustomerResponsiveness || order.Status == StatusOrder.RequestChanged)
                throw new CannotUpdateException("the Order number:" + order.OrderKey + " is closed");

            if (orderUpdate.Status == StatusOrder.CustomerUnresponsiveness || orderUpdate.Status == StatusOrder.RequestChanged)
            {
                order.Status = orderUpdate.Status;
                dal.UpdateOrder(order);//לא עשיתי טרי קאטצ כי מצאתי את ההזמנה הזאת בתחילת הפונקציה
                return true;
            }
            if (orderUpdate.Status == StatusOrder.MailSent)
            {
                orderUpdate.Status = StatusOrder.MailSent;
                dal.UpdateOrder(orderUpdate);
                Console.WriteLine("mail sent\n");
                return true;
            }
            if (orderUpdate.Status == StatusOrder.CustomerResponsiveness)
            {
                orderUpdate.commision = Configuration.commision * SumDays(request.EntryDate, request.ReleaseDate);
                orderUpdate.Status = StatusOrder.CustomerResponsiveness;
                request.Status = StatusGuest.ClosesBySite;//מעדכן בקשה נסגרה כי הלקוח רצה
                for (DateTime date = request.EntryDate; date < request.ReleaseDate; date.AddDays(1))
                    unit.Diary[date.Month, date.Day] = true;//מעדכן את המטריצה בימים שהלקוח רצה              
                UpdateHostingUnit(unit);
                UpdateRequest(request);
                List<Order> orders = GetAllOrders();
                var v = from a in orders
                        where a.GuestRequestKey == request.GuestRequestKey
                        where a.Status != StatusOrder.CustomerResponsiveness
                        select a;
                foreach (var item in v)
                {
                    item.Status = StatusOrder.RequestChanged;
                    dal.UpdateOrder(item);
                }
                dal.UpdateOrder(orderUpdate);
                return true;
            }
            return true;
        }

        public bool UpdateRequest(GuestRequest request)
        {
            try
            {
                GuestRequest guestRequest = GetGuestRequest(request.GuestRequestKey);
            }
            catch (MissingMemberException mme)
            {
                throw new CannotUpdateException("cannot update request beacuse ", mme);
            }
            if (request.Status != StatusGuest.Open)
                throw new CannotUpdateException("Request number: " + request.GuestRequestKey.ToString() + " is closed!");
            List<Order> orders = GetAllOrders();
            var v = from a in orders
                    where a.GuestRequestKey == request.GuestRequestKey
                    select a;
            foreach (var item in v)
            {
                item.Status = StatusOrder.CustomerUnresponsiveness;
                UpdateOrder(item);
            }
            request.Status = StatusGuest.Expired;
            AddRequest(request);
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

        public List<Branche> GetAllBranches()
        {
            List<Branche> branches = dal.GetAllBranches();
            return branches;
        }

        public List<GuestRequest> GetAllGuestRequest()
        {
            List<GuestRequest> guestRequests = dal.GetAllGuestRequest();
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

        public List<GuestRequest> GuestRequestCondition(delegateRequest requestCondition)
        {
            List<GuestRequest> guestRequests = new List<GuestRequest>();
            foreach (var item in dal.GetAllGuestRequest())
            {
                if (requestCondition(item))
                    guestRequests.Add(item);
            }
            return guestRequests;
        }

        //public List<GuestRequest> GetAllGuestRequestByArea(Area area)
        //{
        //    List<GuestRequest> guestRequests = new List<GuestRequest>();
        //    var v = from item in GetAllGuestRequest()
        //            group item by item.Area into areasInGroup
        //            select new { area = areasInGroup };
        //    foreach (var group in v)
        //    {
        //        foreach (var ar in group.area)
        //            guestRequests.Add(ar);
        //    }
        //    return guestRequests;
        //}

        //public List<GuestRequest> GetAllGuestRequestByNumRelax(int num)
        //{
        //    List<GuestRequest> guestRequests = new List<GuestRequest>();
        //    var v = from a in GetAllGuestRequest()
        //            group a by a.Adults + a.Children == num;
        //    foreach (var item in v)
        //    {
        //        guestRequests.Add(item);
        //    }
        //    return guestRequests;
        //}

        //public List<HostingUnit> GetAllHostingUnitByArea(Area area)
        //{
        //    List<HostingUnit> hostingUnits = new List<HostingUnit>();
        //    var v = from a in GetAllHostingUnit()
        //            group a by a.Area == area;
        //    foreach (var item in v)
        //    {
        //        hostingUnits.Add(item);
        //    }
        //    return hostingUnits;
        //}

        //public List<Host> GetAllHostByNumHostingUnit()
        //{

        //}

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
    }
}
