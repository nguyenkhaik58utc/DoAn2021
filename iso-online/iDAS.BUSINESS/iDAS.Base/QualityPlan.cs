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
    
    public partial class QualityPlan
    {
        public QualityPlan()
        {
            this.AccountingPaymentPlans = new HashSet<AccountingPaymentPlan>();
            this.CustomerCampaignPlans = new HashSet<CustomerCampaignPlan>();
            this.HumanTrainingPlans = new HashSet<HumanTrainingPlan>();
            this.ProductDevelopmentRequirementPlans = new HashSet<ProductDevelopmentRequirementPlan>();
            this.QualityPlanTasks = new HashSet<QualityPlanTask>();
            this.ServicePlans = new HashSet<ServicePlan>();
            this.SupplierAuditPlans = new HashSet<SupplierAuditPlan>();
        }
    
        public int ID { get; set; }
        public Nullable<int> QualityTargetID { get; set; }
        public int DepartmentID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Name { get; set; }
        public bool Type { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public bool IsEdit { get; set; }
        public bool IsApproval { get; set; }
        public bool IsAccept { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> ApprovalBy { get; set; }
        public Nullable<System.DateTime> ApprovalAt { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public bool IsProduction { get; set; }
        public bool IsEnd { get; set; }
        public Nullable<System.DateTime> EndAt { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual ICollection<AccountingPaymentPlan> AccountingPaymentPlans { get; set; }
        public virtual ICollection<CustomerCampaignPlan> CustomerCampaignPlans { get; set; }
        public virtual ICollection<HumanTrainingPlan> HumanTrainingPlans { get; set; }
        public virtual ICollection<ProductDevelopmentRequirementPlan> ProductDevelopmentRequirementPlans { get; set; }
        public virtual QualityTarget QualityTarget { get; set; }
        public virtual ICollection<QualityPlanTask> QualityPlanTasks { get; set; }
        public virtual ICollection<ServicePlan> ServicePlans { get; set; }
        public virtual ICollection<SupplierAuditPlan> SupplierAuditPlans { get; set; }
    }
}
