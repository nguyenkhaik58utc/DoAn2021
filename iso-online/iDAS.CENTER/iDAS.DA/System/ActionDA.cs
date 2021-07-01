using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.DA
{
    public class ActionDA : iDASCenterDA<CenterAction>
    {
        public ActionDA() { }
        public ActionDA(DbContext context) : base(context) { }
    }
}
