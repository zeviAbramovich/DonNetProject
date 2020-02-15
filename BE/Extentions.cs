using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BE
{
    public static class Extentions
    {
        public static XElement ToXML(this Order d)
        {
            return new XElement("Order",
                                 new XElement("OrderKey", d.OrderKey.ToString()),
                                 new XElement("HostingUnitKey", d.HostingUnitKey.ToString()),
                                 new XElement("GuestRequestKey", d.GuestRequestKey.ToString()),
                                 new XElement("CreateDate", d.CreateDate.ToString()),
                                 new XElement("OrderDate", d.OrderDate.ToString()),
                                 new XElement("Status", d.Status.ToString()),
                                 new XElement("Commision", d.Commision.ToString())
                                  );
        }
        public static XElement ToXML(this HostingUnit d)
        {
            return new XElement("HostingUnit",
                                  new XElement("HostingUnitKey", d.HostingUnitKey.ToString()),
                                  new XElement("Owner", new XElement(d.Owner.ToXML())),
                                 new XElement("HostingUnitName", d.HostingUnitName.ToString()),
                                 new XElement("Area", d.Area.ToString()),
                                 new XElement("HostingType", d.HostingType.ToString()),
                                 new XElement("Adults", d.Adults.ToString()),
                                 new XElement("Children", d.Children.ToString()),
                                 new XElement("Pool", d.Pool.ToString()),
                                 new XElement("Jacuzzi", d.Jacuzzi.ToString()),
                                 new XElement("Garden", d.Garden.ToString()),
                                 new XElement("ChildrensAttractions", d.ChildrensAttractions.ToString()),
                                 new XElement("SumComission", d.SumComission.ToString()),
                                 new XElement("bookDates", d.bookDates.Select(i => new XElement("Date", i))),
                                   new XElement("DiariDto", d.DiaryDto.ToString())
                                  );
        }
        public static XElement ToXML(this Host d)
        {
            return new XElement("Host",
                                 new XElement("HostId", d.HostId.ToString()),
                                 new XElement("Password", d.Password.ToString()),
                                 new XElement("Name", new XElement("PrivateName", d.PrivateName.ToString()), new XElement("FamilyName", d.FamilyName.ToString())),
                                 new XElement("PhoneNumber", d.PhoneNumber.ToString()),
                                 new XElement("MailAddress", d.MailAddress.ToString()),
                                 new XElement("HostBankAccount", d.HostBankAccount.ToXML()),
                                 new XElement("CollectionClearance", d.CollectionClearance.ToString())
                                  );
        }

        public static XElement ToXML(this BankAccount d)
        {
            return new XElement("BankAccount",
                                 new XElement("BankNumber", d.BankNumber.ToString()),
                                 new XElement("BankName", d.BankName.ToString()),
                                 new XElement("BranchNumber", d.BranchNumber.ToString()),
                                 new XElement("BranchAddress", d.BranchAddress.ToString()),
                                 new XElement("BranchCity", d.BranchCity.ToString()),
                                 new XElement("BankAccountNumber", d.BankAccountNumber.ToString())
                                  );
        }
        public static XElement ToXML(this GuestRequest d)
        {
            return new XElement("GuestRequest",
                                 new XElement("GuestRequestKey", d.GuestRequestKey.ToString()),
                                  new XElement("Name", new XElement("PrivateName", d.PrivateName.ToString()), new XElement("FamilyName", d.FamilyName.ToString())),
                                 new XElement("MailAddress", d.MailAddress.ToString()),
                                 new XElement("Status", d.Status.ToString()),
                                 new XElement("RegistrationDate", d.RegistrationDate.ToString()),
                                 new XElement("EntryDate", d.EntryDate.ToString()),
                                 new XElement("ReleaseDate", d.ReleaseDate.ToString()),
                                 new XElement("Area", d.Area.ToString()),
                                 new XElement("SubArea", d.SubArea.ToString()),
                                 new XElement("HostingType", d.HostingType.ToString()),
                                 new XElement("Adults", d.Adults.ToString()),
                                 new XElement("Children", d.Children.ToString()),
                                 new XElement("Pool", d.Pool.ToString()),
                                new XElement("Jacuzzi", d.Jacuzzi.ToString()),
                                 new XElement("Garden", d.Garden.ToString()),
                                 new XElement("ChildrensAttractions", d.ChildrensAttractions.ToString())
                                 );
        }

        public static void SaveToXML<T>(T source, string path)
        {
            FileStream file = new FileStream(path, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
            xmlSerializer.Serialize(file, source);
            file.Close();
        }

        public static T LoadFromXML<T>(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            T result = (T)xmlSerializer.Deserialize(file);
            file.Close();
            return result;
        }

        public static string ToXMLstring<T>(this T toSerialize)
        {
            using (StringWriter textWriter = new StringWriter())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
        public static T ToObject<T>(this string toDeserialize)
        {
            using (StringReader textReader = new StringReader(toDeserialize))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }

    }
}
