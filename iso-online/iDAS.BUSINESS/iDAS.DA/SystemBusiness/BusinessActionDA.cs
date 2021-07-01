using iDAS.Base;
using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.DA
{
    public class BusinessActionDA : iDASBusinessDA<BusinessAction>
    {
        public BusinessActionDA() { }
        public BusinessActionDA(Database database):base(database) { }
    }
}
