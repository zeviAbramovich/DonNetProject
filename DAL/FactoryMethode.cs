using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public static class FactoryMethode
    {
        public static IDal DalImp= null;
        public static IDal GetDal()
        {
            if (DalImp == null)
            {
                DalImp = new Dal_imp();
                return DalImp;
            }
            else
                return DalImp;
        }                             
    }
}
