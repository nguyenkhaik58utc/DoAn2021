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
    
    public partial class AuditResult
    {
        public int ID { get; set; }
        public int AuditID { get; set; }
        public Nullable<int> QualityNCID { get; set; }
        public int QualityCriteriaID { get; set; }
        public int Point { get; set; }
        public bool IsPass { get; set; }
        public Nullable<int> AuditBy { get; set; }
        public Nullable<System.DateTime> AuditAt { get; set; }
        public string AuditNote { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual Audit Audit { get; set; }
        public virtual QualityCriteria QualityCriteria { get; set; }
        public virtual QualityNC QualityNC { get; set; }
    }
}
