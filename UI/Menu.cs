using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using Utilities;

namespace UI
{
    public static class Menu
    {
        //public string GetMenu(enum a);
        public enum MainMenu
        {
            Client = 1, Owner
        }
        public enum ClientMenu
        {
            AddRequest = 1, UpdateRequest
        }
        public enum OwnerMenu
        {
            AddHostingUnit = 1, DeleteHostingUnit, UpdateHostingUnit, SignCollectionClearance, CheckYourBill, UpdateDetails
        }
        public enum ExceptionMenu
        {
            MainMenu=1, TryAgain
        }
        public static String GetMenu<T>(this T t)
        {
            int count = 1;
            var stringBuilder = new StringBuilder();
            foreach (string menu in Enum.GetNames(typeof(T)))
            {
                stringBuilder.Append(count.ToString()+". " + menu + "\n");
                ++count;
            }
            return stringBuilder.ToString();
        }
    }
}
