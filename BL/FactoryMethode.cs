using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public static class FactoryMethode
    {
        public static IBL BL = null;
        public static IBL GetBL()
        {
            if (BL==null)
            {
                IBL BLimp = new Bl_imp();
                return BLimp;
            }
            return BL;
        }
    }
}
