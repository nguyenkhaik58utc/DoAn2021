using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.DA
{
    public class FunctionDA : iDASCenterDA<CenterFunction>
    {
        public FunctionDA() { }
        public FunctionDA(DbContext context) : base(context) { }
    }
}
