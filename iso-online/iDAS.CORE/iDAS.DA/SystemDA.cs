using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Web;
using System.Collections;
namespace iDAS.DAL
{
    public class SystemBaseDA<TEntity> : BaseDA<TEntity> where TEntity : class
    {
        public iDASDataEntities SystemContext
        {
            get
            {
                return (iDASDataEntities)Context;
            }
        }

        public SystemBaseDA(DbContext context)
            : base(context)
        {
        }

        public SystemBaseDA()
        {
            Context = new iDASDataEntities(BaseDatabase.GetConnectionString(BaseDatabase.GetDatabaseCenter()));
        }
    }
    public class SystemCustomerDA : SystemBaseDA<SystemCustomer>
    {
        
    }
    public class SystemActionDA : SystemBaseDA<SystemAction>{}
    public class SystemFunctionDA : SystemBaseDA<SystemFunction>{}
    public class SystemModuleDA : SystemBaseDA<SystemModule>{}
    public class SystemUserDA : SystemBaseDA<SystemUser>{
        public SystemUserDA() {
            //Context = //BaseDatabase.GetDatabaseCenter<iDASDataEntities>();
        }
        public SystemUserDA(DbContext context) : base(context) { }
    }
    public class SystemGroupDA: SystemBaseDA<SystemGroup>{}
    public class SystemRoleDA : SystemBaseDA<SystemRole> {}
    public class SystemOrganizationDA : SystemBaseDA<SystemOrganization> { }
    public class SystemPermissionDA : SystemBaseDA<SystemPermission> { }
}
