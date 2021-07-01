using iDAS.Core;
using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace iDAS.DA
{
    public class iDASBusinessDA<TEntity> : BaseDA<TEntity> where TEntity : class 
    {
        public iDASBusinessEntities Repository
        {
            get
            {
                return (iDAS.Base.iDASBusinessEntities)Context;
            }
        }
        public iDASBusinessDA()
        {
            Context = BaseDbContext.Instance.GetDbContextByCode<iDASBusinessEntities>();
        }
        public iDASBusinessDA(DbContext context): base(context)
        {
        }
        public iDASBusinessDA(iDAS.Core.Database database) 
        {
            Context = BaseDbContext.Instance.GetDbContext<iDASBusinessEntities>(database);
        }
    }
}
