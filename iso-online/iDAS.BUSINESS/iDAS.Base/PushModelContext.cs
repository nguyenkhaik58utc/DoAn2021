using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Base
{
    public partial class iDASBusinessEntities
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        } 
        public iDASBusinessEntities(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
        
    }
    public partial class iDASCenterEntities 
    {
        public iDASCenterEntities(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
    }
}
