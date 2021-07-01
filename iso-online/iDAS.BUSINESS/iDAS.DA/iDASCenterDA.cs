using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Web;
using System.Collections;
using iDAS.Base;
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
    }
}
