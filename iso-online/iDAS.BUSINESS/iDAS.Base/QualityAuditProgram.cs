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
    
    public partial class QualityAuditProgram
    {
        public QualityAuditProgram()
        {
            this.QualityAuditProgramDepartments = new HashSet<QualityAuditProgramDepartment>();
            this.QualityAuditProgramEmployees = new HashSet<QualityAuditProgramEmployee>();
            this.QualityAuditProgramISOes = new HashSet<QualityAuditProgramISO>();
            this.QualityAuditRecordedVotes = new HashSet<QualityAuditRecordedVote>();
        }
    
        public int ID { get; set; }
        public int QualityAuditPlanID { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public bool IsNew { get; set; }
        public bool IsEdit { get; set; }
        public bool IsApproval { get; set; }
        public bool IsAccept { get; set; }
        public Nullable<System.DateTime> ApprovalAt { get; set; }
        public Nullable<int> ApprovalBy { get; set; }
        public string ApprovalNote { get; set; }
        public string Content { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual QualityAuditPlan QualityAuditPlan { get; set; }
        public virtual ICollection<QualityAuditProgramDepartment> QualityAuditProgramDepartments { get; set; }
        public virtual ICollection<QualityAuditProgramEmployee> QualityAuditProgramEmployees { get; set; }
        public virtual ICollection<QualityAuditProgramISO> QualityAuditProgramISOes { get; set; }
        public virtual ICollection<QualityAuditRecordedVote> QualityAuditRecordedVotes { get; set; }
    }
}