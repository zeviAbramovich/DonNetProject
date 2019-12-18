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
        public int HostingUnitKey { get; private set; }
        public Host Owner { get; set; }
        public String HostingUnitName { get; set; }
        [XmlIgnore]
        public bool[,] Diary { get; private set; }
        [XmlArray("Diary")]
        public bool[] DiaryDto
        {
            get { return Diary.Flatten(); }
            set { Diary = value.Expand(4); }
        }

        public HostingUnit()
        {
            Diary = new bool[12, 31];
            HostingUnitKey = Configuration.serialHostingUnit++;
        }

        public override string ToString()
        {
            return this.TostringProperties();
        }

    }
}
