using iDAS.Base;
using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.DA
{
    public class BusinessFunctionDA : iDASBusinessDA<BusinessFunction>
    {
        public BusinessFunctionDA() { }
        public BusinessFunctionDA(Database database):base(database) { }
    }
}
