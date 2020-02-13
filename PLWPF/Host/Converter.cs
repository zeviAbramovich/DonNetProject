using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Threading.Tasks;
using BE;

namespace PLWPF.Host
{

    public class OStatusConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {

            var dataobj = (KeyValuePair<string, StatusOrder>)value;

            var data = ((StatusOrder)dataobj.Value).ToString();

           // var data = ((StatusOrder)value).ToString();

            if (data.IsNullOrEmpty())
            {
                return default(string);
            }

            data = GetHebrewEnumData(data);

            return data;
        }

        private string GetHebrewEnumData(string data)
        {
            var result = string.Empty;

            var hebrewData = GetHebrewData();

            foreach (var item in hebrewData)
            {
                var enumValue =  (StatusOrder)Enum.Parse(typeof(StatusOrder), data);
                if(enumValue == item.Key)
                {
                    result = item.Value;
                }
            }

            return result;  
        }

        private Dictionary<StatusOrder, string> GetHebrewData()
        {
            var dictionary = new Dictionary<StatusOrder, string>();

            dictionary.Add(StatusOrder.CustomerResponsiveness, "עסקה נסגרה");
            dictionary.Add(StatusOrder.CustomerUnresponsiveness, "לקוח לא מגיב");
            dictionary.Add(StatusOrder.MailSent, "מייל נשלח ללקוח");
            dictionary.Add(StatusOrder.NotYetApproved, "לא אושר עדיין");
            dictionary.Add(StatusOrder.RequestChanged, "הזמנה נסגרה");

            return dictionary;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
