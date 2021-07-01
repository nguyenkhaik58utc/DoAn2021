using iDAS.Base;
using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.DA
{
    public class ModuleDA : iDASCenterDA<CenterModule> {
        public ModuleDA() { }
        public ModuleDA(DbContext context) : base(context) { }
    }
}
