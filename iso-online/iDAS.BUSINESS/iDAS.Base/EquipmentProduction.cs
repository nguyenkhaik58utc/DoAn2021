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
    
    public partial class EquipmentProduction
    {
        public EquipmentProduction()
        {
            this.EquipmentProductionAttaches = new HashSet<EquipmentProductionAttach>();
            this.EquipmentProductionAttachmentFiles = new HashSet<EquipmentProductionAttachmentFile>();
            this.EquipmentProductionDepartments = new HashSet<EquipmentProductionDepartment>();
            this.EquipmentProductionErrors = new HashSet<EquipmentProductionError>();
            this.EquipmentProductionHistories = new HashSet<EquipmentProductionHistory>();
            this.EquipmentProductionMaintains = new HashSet<EquipmentProductionMaintain>();
            this.ProductionPlanEquipments = new HashSet<ProductionPlanEquipment>();
        }
    
        public int ID { get; set; }
        public int EquipmentProductionCategoryID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string MadeIn { get; set; }
        public Nullable<System.DateTime> MadeYear { get; set; }
        public Nullable<System.DateTime> UseStartTime { get; set; }
        public string Specifications { get; set; }
        public string MaintainContent { get; set; }
        public string MaintainOther { get; set; }
        public Nullable<int> MaintainPeriodic { get; set; }
        public string SupportInfomation { get; set; }
        public string Features { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsUsing { get; set; }
        public bool IsError { get; set; }
        public bool IsFixed { get; set; }
        public bool IsMaintain { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    
        public virtual ICollection<EquipmentProductionAttach> EquipmentProductionAttaches { get; set; }
        public virtual ICollection<EquipmentProductionAttachmentFile> EquipmentProductionAttachmentFiles { get; set; }
        public virtual EquipmentProductionCategory EquipmentProductionCategory { get; set; }
        public virtual ICollection<EquipmentProductionDepartment> EquipmentProductionDepartments { get; set; }
        public virtual ICollection<EquipmentProductionError> EquipmentProductionErrors { get; set; }
        public virtual ICollection<EquipmentProductionHistory> EquipmentProductionHistories { get; set; }
        public virtual ICollection<EquipmentProductionMaintain> EquipmentProductionMaintains { get; set; }
        public virtual ICollection<ProductionPlanEquipment> ProductionPlanEquipments { get; set; }
    }
}
