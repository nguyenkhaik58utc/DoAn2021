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
    
    public partial class QualityCriteria
    {
        public QualityCriteria()
        {
            this.AuditResults = new HashSet<AuditResult>();
            this.HumanProfileWorkTrialResults = new HashSet<HumanProfileWorkTrialResult>();
            this.SupplierAuditResults = new HashSet<SupplierAuditResult>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int MinPoint { get; set; }
        public int MaxPoint { get; set; }
        public decimal Factory { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<int> QualityCriteriaCategoryID { get; set; }
    
        public virtual ICollection<AuditResult> AuditResults { get; set; }
        public virtual ICollection<HumanProfileWorkTrialResult> HumanProfileWorkTrialResults { get; set; }
        public virtual QualityCriteriaCategory QualityCriteriaCategory { get; set; }
        public virtual ICollection<SupplierAuditResult> SupplierAuditResults { get; set; }
    }
}
