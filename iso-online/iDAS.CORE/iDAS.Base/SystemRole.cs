//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iDAS.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class SystemRole
    {
        public SystemRole()
        {
            this.SystemOrganizations = new HashSet<SystemOrganization>();
            this.SystemPermissions = new HashSet<SystemPermission>();
        }
    
        public int ID { get; set; }
        public int GroupID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    
        public virtual SystemGroup SystemGroup { get; set; }
        public virtual ICollection<SystemOrganization> SystemOrganizations { get; set; }
        public virtual ICollection<SystemPermission> SystemPermissions { get; set; }
    }
}
