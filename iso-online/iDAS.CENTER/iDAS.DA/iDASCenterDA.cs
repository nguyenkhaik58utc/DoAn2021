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
    public class iDASCenterDA<TEntity> : BaseDA<TEntity> where TEntity : class
    {
        public iDASCenterEntities Repository
        {
            get
            {
                return (iDASCenterEntities)Context;
            }
        }
        public iDASCenterDA()
        {
            Context = BaseDbContext.Instance.GetDbContextCenter<iDASCenterEntities>();
        }
        public iDASCenterDA(DbContext context): base(context)
        {
        }
        public iDASCenterDA(iDAS.Core.Database database)
        {
            Context = BaseDbContext.Instance.GetDbContext<iDASCenterEntities>(database);
        }
    }
}
