using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public static class FactoryMethode
    {
        public static IBL GetBL()
        {
            IBL BLImp = new Bl_imp();
            return BLImp;
        }
    }
}
