using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public static class Configuration
    {
        public static long serialGuestRequest = 10000000;
        public static double commision = 10;   
        public static long serialHostingUnit = 10000000;
        public static long serialOrder = 10000000;

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action?.Invoke(item);
            }
        }

        public static void ForEach<T,P>(this IEnumerable<T> collection, Func<T,P> func)
        {
            foreach (var item in collection)
            {
                func?.Invoke(item);
            }
        }

        public static bool IsNullOrEmpty(this IEnumerable collection)
        {
            if(collection == null)
            {
                return true;
            }

            foreach (var item in collection)
            {
                return false;
            }

            return true;
        }
    }

}
