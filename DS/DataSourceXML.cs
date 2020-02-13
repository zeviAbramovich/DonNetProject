using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DataSource
{
    public static class DataSourceXML
    {
        //private static string currentDirectory = Directory.GetCurrentDirectory();
        private static string solutionDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName;

        private static string filePath = System.IO.Path.Combine(solutionDirectory, "DataSource", "DataXML");


        private static XElement orderRoot = null;
        private static XElement guestRequestRoot = null;
        private static XElement hostRoot = null;
        private static XElement hostingUnitRoot = null;


        private static string orderPath = Path.Combine(filePath, "OrderXml.xml");
        private static string guestRequestPath = Path.Combine(filePath, "GuestRequestXml.xml");
        private static string hostPath = Path.Combine(filePath, "HostXml.xml");
        private static string hostingUnitPath = Path.Combine(filePath, "HostingUnitXml.xml");



        static DataSourceXML()
        {
            bool exists = Directory.Exists(filePath);
            if (!exists)
            {
                Directory.CreateDirectory(filePath);
            }

            if (!File.Exists(orderPath))
            {
                CreateFile("Orders", orderPath);

            }
            orderRoot = LoadData(orderPath);

            if (!File.Exists(hostPath))
            {
                CreateFile("Hosts", hostPath);

            }
            hostRoot = LoadData(hostPath);

            if (!File.Exists(hostingUnitPath))
            {
                CreateFile("HostingUnits", hostingUnitPath);

            }
            hostingUnitRoot = LoadData(hostingUnitPath);


            if (!File.Exists(guestRequestPath))
            {
                CreateFile("GuestRequests", guestRequestPath);

            }
            guestRequestRoot = LoadData(guestRequestPath);

        }
        private static void CreateFile(string typename, string path)
        {
            XElement root = new XElement(typename);
            root.Save(path);
        }

        public static void SaveOrders()
        {
            orderRoot.Save(orderPath);
        }

        public static void SaveHostingUnits()
        {
            hostingUnitRoot.Save(hostingUnitPath);
        }

        public static void SaveHosts()
        {
            hostRoot.Save(hostPath);
        }

        public static void SaveGuestRequests()
        {
            guestRequestRoot.Save(guestRequestPath);
        }

        public static XElement Orders
        {
            get
            {
                orderRoot = LoadData(orderPath);
                return orderRoot;
            }
        }

        public static XElement Hosts
        {
            get
            {
                hostRoot = LoadData(hostPath);
                return hostRoot;
            }
        }

        public static XElement HostingUnits
        {
            get
            {
                hostingUnitRoot = LoadData(hostingUnitPath);
                return hostingUnitRoot;
            }
        }

        public static XElement GuestRequests
        {
            get
            {
                guestRequestRoot = LoadData(guestRequestPath);
                return guestRequestRoot;
            }
        }

        private static XElement LoadData(string path)
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


    }
}
