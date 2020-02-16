using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

namespace DAL
{
    class Dal_XML_imp : IDal
    {
        private static string solutionDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName;
        private static string filePath = Path.Combine(solutionDirectory, "DAL", "DataXML");
        private static XElement configurationRoot = null;
        private static string configurationPath = Path.Combine(filePath, "Configuration.xml");
        public List<HostingUnit> hostingUnits = new List<HostingUnit>();
        public List<Order> orders = new List<Order>();
        public List<GuestRequest> guestRequests = new List<GuestRequest>();
        public List<Host> hosts = new List<Host>();
        public string pathHostingUnit = "HostingUnit.xml";
        public string pathGuestRequest = "GuestRequest.xml";
        public string pathOrder = "Order.xml";
        public string pathHost = "Host.xml";
        public string ConfigurationPath = "config.xml";

        public XmlSerializer serializerUnit = new XmlSerializer(typeof(List<HostingUnit>));
        public XmlSerializer serializerOrder = new XmlSerializer(typeof(List<Order>));
        public XmlSerializer serializerHost = new XmlSerializer(typeof(List<Host>));
        public XmlSerializer serializerGuestRequest = new XmlSerializer(typeof(List<GuestRequest>));

        public Dal_XML_imp()
        {
            if (!File.Exists("HostingUnit.xml"))
            {
                FileStream fsout = new FileStream(pathHostingUnit, FileMode.Create);
                serializerUnit.Serialize(fsout, hostingUnits);
                fsout.Close();
            }
            if (!File.Exists("Configuration.xml"))
            {
                CreateFile("Configuration", configurationPath);
            }
            configurationRoot = LoadData(configurationPath);
            if (!File.Exists("GuestRequest.xml"))
            {
                FileStream fsout = new FileStream(pathGuestRequest, FileMode.Create);
                serializerGuestRequest.Serialize(fsout, guestRequests);
                fsout.Close();
            }
            if (!File.Exists("Order.xml"))
            {
               
                FileStream fsout = new FileStream(pathOrder, FileMode.Create);
                serializerOrder.Serialize(fsout, orders);
                fsout.Close();
            }
            if (!File.Exists("Host.xml"))
            {
                FileStream fsout = new FileStream(pathHost, FileMode.Create);
                serializerHost.Serialize(fsout, hosts);
                fsout.Close();
            }
        }
        #region Add Delete And Update
        private void Load(ref XElement t, string a)
        {
            try
            {
                t = XElement.Load(a);
            }
            catch
            {
                throw new DirectoryNotFoundException(" שגיאה! בעיית טעינת קובץ:" + a);
            }

        }

        private XElement ConfigRoot;
      
        public bool AddHostingUnit(HostingUnit unit)
        {
            if (unit.HostingUnitKey == 0)//must be new unit
            {
                hostingUnits.Clear();
                FileStream fsout = new FileStream(pathHostingUnit, FileMode.Open);
                hostingUnits = (List<HostingUnit>)serializerUnit.Deserialize(fsout);//load the data into list
                fsout.Close();
                HostingUnit hostingUnit = unit.Clone();//על פי נספח 1 
                Load(ref ConfigRoot, ConfigurationPath);
                int temp = int.Parse(ConfigRoot.Element("serialHostingUnit").Value);//serial number load
                hostingUnit.HostingUnitKey = ++temp;
                ConfigRoot.Element("serialHostingUnit").Value = temp.ToString();
                hostingUnit.Diary = new bool[12, 31];
                hostingUnits.Add(hostingUnit);           
                FileStream fsin = new FileStream(pathHost, FileMode.Open);
                serializerUnit.Serialize(fsin, hostingUnits);
                fsin.Close();
                //var v = from item in GetAllHosts()
                //        where item.HostId == unit.Owner.HostId
                //        select item;
                //foreach (var item in v)
                //{
                //    item.numOfUnits += 1;
                //}
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
                orders.Clear();
                FileStream fsout = new FileStream(pathOrder, FileMode.Open);
                orders = (List<Order>)serializerOrder.Deserialize(fsout);
                fsout.Close();
                Order tempOrder = order.Clone();
                int temp = int.Parse(Configuration.Element("serialOrder").Value);//serial number load
                tempOrder.OrderKey = ++temp;
                Configuration.Element("serialOrder").Value = temp.ToString();
                orders.Add(order);
                FileStream fsin = new FileStream(pathOrder, FileMode.Open);
                serializerOrder.Serialize(fsin, orders);
                fsin.Close();
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
                guestRequests.Clear();
                FileStream fsout = new FileStream(pathGuestRequest, FileMode.Open);
                guestRequests = (List<GuestRequest>)serializerGuestRequest.Deserialize(fsout);
                fsout.Close();
                GuestRequest guestRequest = guest.Clone();
                guestRequest.RegistrationDate = DateTime.Now;
                //DS.DataSourceXML.Configuration.Element("serialGuestRequest").Value = guestRequest.GuestRequestKey.ToString();
                guestRequests.Add(guestRequest);
                FileStream fsin = new FileStream(pathGuestRequest, FileMode.Open);
                serializerGuestRequest.Serialize(fsin, guestRequests);
                fsin.Close();
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

            hosts.Clear();
            FileStream fsout = new FileStream(pathHost, FileMode.Open);
            hosts = (List<Host>)serializerHost.Deserialize(fsout);
            fsout.Close();
            Host host1 = host.Clone();
            hosts.Add(host1);
            FileStream fsin = new FileStream(pathHost, FileMode.Open);
            serializerHost.Serialize(fsin, hosts);
            fsin.Close();
            return true;
        }

        public bool DeleteHostingUnit(long key)
        {
            hostingUnits.Clear();
            FileStream fsout = new FileStream(pathHostingUnit, FileMode.Open);
            hostingUnits = (List<HostingUnit>)serializerUnit.Deserialize(fsout);
            fsout.Close();
            var v = from item in hostingUnits
                    where item.HostingUnitKey == key//catch the unit I want to delete
                    select item;
            HostingUnit hostingUnit = (HostingUnit)v;
            try
            {
                if (v.Any())
                {
                    hostingUnits.Remove((HostingUnit)v);
                    FileStream fsin = new FileStream(pathHostingUnit, FileMode.Open);
                    serializerUnit.Serialize(fsin, hostingUnits);
                    fsin.Close();
                    //var b = from item in GetAllHosts()
                    //        where item.HostId == hostingUnit.Owner.HostId
                    //        select item;
                    //foreach (var item in v)
                    //{
                    //    item.numOfUnits -= 1;
                    //}
                    //return true;
                }
                throw new MissingMemberException();
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
            hostingUnits.Clear();
            FileStream fsout = new FileStream(pathHostingUnit, FileMode.Open);
            hostingUnits = (List<HostingUnit>)serializerUnit.Deserialize(fsout);
            fsout.Close();
            var v = from item in hostingUnits
                    where item.HostingUnitKey == hosting.HostingUnitKey
                    select item;
            hostingUnits.Remove((HostingUnit)v);
            hostingUnits.Add(hosting);
            FileStream fsin = new FileStream(pathHostingUnit, FileMode.Open);
            serializerUnit.Serialize(fsin, hostingUnits);
            fsin.Close();
            return true;
        }

        public bool UpdateOrder(Order updateOrder)
        {
            orders.Clear();
            FileStream fsout = new FileStream(pathOrder, FileMode.Open);
            orders = (List<Order>)serializerOrder.Deserialize(fsout);
            fsout.Close();
            Order order = orders.Find(x => x.OrderKey == updateOrder.OrderKey);
            if (order == null)
                throw new CannotUpdateException("Cannot update! order number " + updateOrder.OrderKey.ToString() + "not exsist");
            order.Status = updateOrder.Status;
            order.Commision = updateOrder.Commision;
            orders.Add(order);
            FileStream fsin = new FileStream(pathOrder, FileMode.Open);
            serializerOrder.Serialize(fsin, orders);
            fsin.Close();
            hostingUnits.Clear();
            FileStream fsoutUnit = new FileStream(pathHostingUnit, FileMode.Open);
            hostingUnits = (List<HostingUnit>)serializerUnit.Deserialize(fsoutUnit);
            fsoutUnit.Close();
            HostingUnit hostingUnit = hostingUnits.Find(x => x.HostingUnitKey == updateOrder.HostingUnitKey);
            hostingUnit.SumComission += updateOrder.Commision;
            FileStream fsinUnit = new FileStream(pathHostingUnit, FileMode.Open);
            serializerUnit.Serialize(fsinUnit, hostingUnits);
            fsinUnit.Close();
            return true;
        }

        public bool UpdateRequest(GuestRequest updateRequest)
        {
            guestRequests.Clear();
            FileStream fsout = new FileStream(pathGuestRequest, FileMode.Open);
            guestRequests = (List<GuestRequest>)serializerGuestRequest.Deserialize(fsout);
            fsout.Close();
            GuestRequest Request = guestRequests.Find(g => g.GuestRequestKey == updateRequest.GuestRequestKey);
            if (Request == null)
                throw new CannotUpdateException("There is no RequestKey " + updateRequest.GuestRequestKey.ToString());
            //if my status change is open, then I close the old request and add the new one
            if (updateRequest.Status == StatusGuest.Open)
            {
                Request.Status = StatusGuest.Expired;
                AddRequest(Request);//מעדכן מה שרציתי
                return true;
            }
            ////if my status change is not open, then i update the status
            if (updateRequest.Status == StatusGuest.ClosesBySite || updateRequest.Status == StatusGuest.Expired)
            {
                Request.Status = updateRequest.Status;
                guestRequests.Remove(updateRequest);
                guestRequests.Add(Request);
                FileStream fsin = new FileStream(pathGuestRequest, FileMode.Open);
                serializerGuestRequest.Serialize(fsin, guestRequests);
                fsin.Close();
                return true;
            }
            return true;
        }

        public bool UpdateHost(Host host)
        {
            hosts.Clear();
            FileStream fsout = new FileStream(pathHost, FileMode.Open);
            hosts = (List<Host>)serializerHost.Deserialize(fsout);
            fsout.Close();
            Host hostOriginal = hosts.FirstOrDefault(x => x.HostId == host.HostId);
            hosts.Remove(hostOriginal);
            hosts.Add(host);
            FileStream fsin = new FileStream(pathGuestRequest, FileMode.Open);
            serializerHost.Serialize(fsin, hosts);
            fsin.Close();
            var v = from item in GetAllHostingUnits()
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
            FileStream fsinUnit = new FileStream(pathHostingUnit, FileMode.Open);
            serializerUnit.Serialize(fsinUnit, (List<HostingUnit>)v);
            fsinUnit.Close();
            return true;
        }
        #endregion

        #region getters
        public List<Branche> GetAllBranches()
        {
            List<Branche> branches = new List<Branche>();
            //{
            //    new Branche()
            //    {
            //        BankNumber=12,BankName="Hapoalim",BranchAddress="Rabi akiva",
            //        BranchCity="Bney brak",BranchNumber=111
            //    },

            //    new Branche()
            //    {
            //        BankNumber=12,BankName="Hapoalim",BranchAddress="Hashomer",
            //        BranchCity="Bney brak",BranchNumber=222
            //    },

            //    new Branche()
            //    {
            //        BankNumber=12,BankName="Hapoalim",BranchAddress="Herzel",
            //        BranchCity="Bney brak",BranchNumber=333
            //    },

            //    new Branche()
            //    {
            //        BankNumber=12,BankName="Hapoalim",BranchAddress="Ainshteyn",
            //        BranchCity="Bney brak",BranchNumber=444
            //    },


            //    new Branche()
            //    {
            //        BankNumber=12,BankName="Hapoalim",BranchAddress="Hagiborim",
            //        BranchCity="Bney brak",BranchNumber=555
            //    }
            //};

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
            guestRequests.Clear();
            FileStream fsout = new FileStream(pathGuestRequest, FileMode.Open);
            guestRequests = (List<GuestRequest>)serializerGuestRequest.Deserialize(fsout);
            fsout.Close();
            var v = from item in guestRequests
                    where item.Status != StatusGuest.Expired
                    select item;
            foreach (var item in v)
                guestRequests.Add(item.Clone());
            return guestRequests;
        }

        public List<HostingUnit> GetAllHostingUnits()
        {
            hostingUnits.Clear();
            //hostingUnits = DS.DataSource.hostingUnitList;
            FileStream fsout = new FileStream(pathHostingUnit, FileMode.Open);
            //serializerUnit.Serialize(fsout, hostingUnits);
            hostingUnits = (List<HostingUnit>)serializerUnit.Deserialize(fsout);
            fsout.Close();
            //foreach (var item in hostingUnits)
            //    hostingUnits.Add(item.Clone());
            return hostingUnits;
        }

        public List<Order> GetAllOrders()
        {
            orders.Clear();
            //orders = DS.DataSource.orders;
            FileStream fsout = new FileStream(pathOrder, FileMode.Open);
            //serializerOrder.Serialize(fsout, orders);
            orders = (List<Order>)serializerOrder.Deserialize(fsout);
            fsout.Close();
            //foreach (var item in orders)
            //    orders.Add(item.Clone());
            return orders;
        }

        public List<Host> GetAllHosts()
        {
            hosts.Clear();
            //hosts = DS.DataSource.hosts;
            FileStream fsout = new FileStream(pathHost, FileMode.Open);
            //serializerHost.Serialize(fsout, hosts);

            hosts = (List<Host>)serializerHost.Deserialize(fsout);
            fsout.Close();
            //foreach (var item in hosts)
            //    hosts.Add(item.Clone());
            return hosts;
        }

        public HostingUnit GetHostingUnit(long key)
        {
            hostingUnits.Clear();
            FileStream fsout = new FileStream(pathHostingUnit, FileMode.Open);
            hostingUnits = (List<HostingUnit>)serializerUnit.Deserialize(fsout);
            fsout.Close();
            HostingUnit tempUnit = hostingUnits.FirstOrDefault(x => x.HostingUnitKey == key);
            if (tempUnit == null)
                throw new MissingMemberException("did not find unit");
            return tempUnit;
        }

        public Order GetOrder(long key)
        {
            orders.Clear();
            FileStream fsout = new FileStream(pathOrder, FileMode.Open);
            orders = (List<Order>)serializerOrder.Deserialize(fsout);
            fsout.Close();
            Order tempOrder = orders.FirstOrDefault(x => x.OrderKey == key);
            if (tempOrder == null)
                throw new MissingMemberException("did not find order");
            return tempOrder;
        }

        public GuestRequest GetGuestRequest(long key)
        {
            guestRequests.Clear();
            FileStream fsout = new FileStream(pathGuestRequest, FileMode.Open);
            guestRequests = (List<GuestRequest>)serializerGuestRequest.Deserialize(fsout);
            fsout.Close();
            GuestRequest tempRequest = guestRequests.FirstOrDefault(x => x.GuestRequestKey == key);//check if exist
            if (tempRequest == null)
                throw new MissingMemberException("did not find guest request", this.GetType().ToString());
            return tempRequest;
        }

        public Host GetHost(long key)
        {
            hosts.Clear();
            FileStream fsout = new FileStream(pathHost, FileMode.Open);
            hosts = (List<Host>)serializerHost.Deserialize(fsout);
            fsout.Close();
            Host host = hosts.FirstOrDefault(x => x.HostId == key);//check if exist
            if (host == null)
                throw new MissingMemberException("did not find guest request", this.GetType().ToString());
            return host;
        }

        public void CreateFile(string typename, string path)
        {
            XElement root = new XElement(typename);
            root.Save(path);
        }
        public XElement LoadData(string path)
        {
            XElement root;
            try
            {
                root = XElement.Load(path);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
            return root;
        }
        public  XElement Configuration
        {
            get
            {
                configurationRoot = LoadData(configurationPath);
                return configurationRoot;
            }
        }

        #endregion
    }
}


