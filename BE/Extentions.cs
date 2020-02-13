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
    static class Extentions
    {
        public static XElement ToXML(this Order d)
        {
            return new XElement("Order",
                                 new XElement("OrderKey", d.OrderKey.ToString()),
                                 new XElement("HostingUnitKey", d.HostingUnitKey.ToString()),
                                 new XElement("GuestRequestKey", d.GuestRequestKey.ToString()),
                                 new XElement("CreateDate", d.CreateDate.ToString()),
                                 new XElement("OrderDate", d.OrderDate.ToString()),
                                 new XElement("Status", d.Status.ToString())
                                  );
        }

       

        public static void SaveToXML<T>(T source, string path)
        {
            FileStream file = new FileStream(path, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
            xmlSerializer.Serialize(file, source);
            file.Close();
        }

        public static T LoadFromXML<T>( string path)
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
