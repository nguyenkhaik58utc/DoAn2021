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
    
    public partial class QualityAuditProgramEmployee
    {
        public int ID { get; set; }
        public int QualityAuditProgramID { get; set; }
        public int HumanEmployeeID { get; set; }
        public bool IsCaptain { get; set; }
        public bool IsAuditor { get; set; }
        public bool IsAbsent { get; set; }
        public string AbsentReason { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual HumanEmployee HumanEmployee { get; set; }
        public virtual QualityAuditProgram QualityAuditProgram { get; set; }
    }
}
