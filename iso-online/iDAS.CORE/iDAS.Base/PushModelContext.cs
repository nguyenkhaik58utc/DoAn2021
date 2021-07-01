using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Base
{
    public partial class iDASDataEntities 
    {
        public iDASDataEntities() { }
        public iDASDataEntities(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
    }
}
