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
    
    public partial class DepartmentLegal
    {
        public DepartmentLegal()
        {
            this.DepartmentLegalAttachments = new HashSet<DepartmentLegalAttachment>();
            this.DepartmentLegalAuditResults = new HashSet<DepartmentLegalAuditResult>();
            this.RiskLegals = new HashSet<RiskLegal>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> ISOID { get; set; }
        public string Description { get; set; }
        public bool IsUse { get; set; }
        public string Content { get; set; }
        public string ListApplyDepartment { get; set; }
        public bool IsApplyAll { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual ICollection<DepartmentLegalAttachment> DepartmentLegalAttachments { get; set; }
        public virtual ICollection<DepartmentLegalAuditResult> DepartmentLegalAuditResults { get; set; }
        public virtual ICollection<RiskLegal> RiskLegals { get; set; }
    }
}
