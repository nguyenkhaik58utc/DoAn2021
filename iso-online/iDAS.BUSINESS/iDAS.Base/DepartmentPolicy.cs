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
    
    public partial class DepartmentPolicy
    {
        public DepartmentPolicy()
        {
            this.DepartmentPolicyAttachments = new HashSet<DepartmentPolicyAttachment>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> ISOID { get; set; }
        public string Description { get; set; }
        public bool IsUse { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public string Scope { get; set; }
        public string Object { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual ICollection<DepartmentPolicyAttachment> DepartmentPolicyAttachments { get; set; }
    }
}
