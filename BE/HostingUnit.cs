using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Utilities;



namespace BE
{
    public class HostingUnit
    {
        public long HostingUnitKey { get; set; }
        public Host Owner { get; set; }
        public String HostingUnitName { get; set; }
        public Area Area { get; set; }
        public String SubArea { get; set; }
        public HostingType HostingType { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public bool Pool { get; set; }
        public bool Jacuzzi { get; set; }
        public bool Garden { get; set; }
        public bool ChildrensAttractions { get; set; }
        [XmlIgnore]
        public bool[,] Diary { get;  set; }
        [XmlArray("Diary")]
        public bool[] DiaryDto
        {
            get { return Diary.Flatten(); }
            set { Diary = value.Expand(4); }
        }        

        public override string ToString()
        {
            return this.TostringProperties();
        }

    }
}
