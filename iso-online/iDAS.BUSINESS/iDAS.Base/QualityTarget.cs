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
    
    public partial class QualityTarget
    {
        public QualityTarget()
        {
            this.CustomerCampaignTargets = new HashSet<CustomerCampaignTarget>();
            this.QualityPlans = new HashSet<QualityPlan>();
        }
    
        public int ID { get; set; }
        public Nullable<int> QualityTargetCategoryID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Unit { get; set; }
        public System.DateTime CompleteAt { get; set; }
        public bool Type { get; set; }
        public Nullable<int> QualityTargetLevelID { get; set; }
        public bool IsEdit { get; set; }
        public bool IsApproval { get; set; }
        public string ApprovalNote { get; set; }
        public bool IsAccept { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> ApprovalBy { get; set; }
        public Nullable<System.DateTime> ApprovalAt { get; set; }
        public string Description { get; set; }
        public bool IsEnd { get; set; }
        public Nullable<System.DateTime> EndAt { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual ICollection<CustomerCampaignTarget> CustomerCampaignTargets { get; set; }
        public virtual ICollection<QualityPlan> QualityPlans { get; set; }
        public virtual QualityTargetCategory QualityTargetCategory { get; set; }
        public virtual QualityTargetLevel QualityTargetLevel { get; set; }
    }
}
