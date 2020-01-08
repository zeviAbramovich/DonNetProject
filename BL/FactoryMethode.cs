using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;

namespace BL
{
    public static class FactoryMethode
    {
        public static IBL GetBL()
        {
            IBL BLimp = new Bl_imp();
            return BLimp;
        }
    }
}
