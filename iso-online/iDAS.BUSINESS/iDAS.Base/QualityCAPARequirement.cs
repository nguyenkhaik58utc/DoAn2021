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
    
    public partial class QualityCAPARequirement
    {
        public QualityCAPARequirement()
        {
            this.QualityCAPAs = new HashSet<QualityCAPA>();
        }
    
        public int ID { get; set; }
        public int QualityNCID { get; set; }
        public int DepartmentID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public System.DateTime CompleteTime { get; set; }
        public Nullable<System.DateTime> CompleteRealTime { get; set; }
        public bool IsCompleteAuto { get; set; }
        public bool IsComplete { get; set; }
        public Nullable<bool> IsAuditBack { get; set; }
        public Nullable<bool> AuditBackPass { get; set; }
        public string AuditBackNote { get; set; }
        public Nullable<int> AuditBackBy { get; set; }
        public Nullable<System.DateTime> AuditBackTime { get; set; }
        public Nullable<int> ParentID { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual QualityNC QualityNC { get; set; }
        public virtual ICollection<QualityCAPA> QualityCAPAs { get; set; }
    }
}
