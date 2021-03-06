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
    
    public partial class AttachmentFile
    {
        public AttachmentFile()
        {
            this.CustomerContactHistoryAttachmentFiles = new HashSet<CustomerContactHistoryAttachmentFile>();
            this.CustomerContacts = new HashSet<CustomerContact>();
            this.CustomerContractAttachmentFiles = new HashSet<CustomerContractAttachmentFile>();
            this.CustomerOrderAttachmentFiles = new HashSet<CustomerOrderAttachmentFile>();
            this.DepartmentLegalAttachments = new HashSet<DepartmentLegalAttachment>();
            this.DepartmentPolicyAttachments = new HashSet<DepartmentPolicyAttachment>();
            this.DepartmentRegulatoryAttachments = new HashSet<DepartmentRegulatoryAttachment>();
            this.DepartmentRequirmentAttachments = new HashSet<DepartmentRequirmentAttachment>();
            this.DispatchGoAttachmentFiles = new HashSet<DispatchGoAttachmentFile>();
            this.DispatchToAttachmentFiles = new HashSet<DispatchToAttachmentFile>();
            this.DocumentAttachmentFiles = new HashSet<DocumentAttachmentFile>();
            this.EquipmentMeasureAttachmentFiles = new HashSet<EquipmentMeasureAttachmentFile>();
            this.EquipmentProductionAttachmentFiles = new HashSet<EquipmentProductionAttachmentFile>();
            this.HumanEmployees = new HashSet<HumanEmployee>();
            this.HumanProfileAttachments = new HashSet<HumanProfileAttachment>();
            this.ProductDevelopmentRequirementAttachmentFiles = new HashSet<ProductDevelopmentRequirementAttachmentFile>();
            this.ProfileAttachmentFiles = new HashSet<ProfileAttachmentFile>();
            this.QualityMeetingAttachmentFiles = new HashSet<QualityMeetingAttachmentFile>();
            this.QualityMeetingResultAttachmentFiles = new HashSet<QualityMeetingResultAttachmentFile>();
            this.SupplierBidToAttachmentFiles = new HashSet<SupplierBidToAttachmentFile>();
            this.Suppliers = new HashSet<Supplier>();
            this.TaskAttachmentFiles = new HashSet<TaskAttachmentFile>();
            this.TaskPerformAttachmentFiles = new HashSet<TaskPerformAttachmentFile>();
            this.TemplateExports = new HashSet<TemplateExport>();
            this.V3ProblemAttachmentFiles = new HashSet<V3ProblemAttachmentFile>();
        }
    
        public System.Guid ID { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public double Size { get; set; }
        public string Type { get; set; }
        public byte[] Data { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual ICollection<CustomerContactHistoryAttachmentFile> CustomerContactHistoryAttachmentFiles { get; set; }
        public virtual ICollection<CustomerContact> CustomerContacts { get; set; }
        public virtual ICollection<CustomerContractAttachmentFile> CustomerContractAttachmentFiles { get; set; }
        public virtual ICollection<CustomerOrderAttachmentFile> CustomerOrderAttachmentFiles { get; set; }
        public virtual ICollection<DepartmentLegalAttachment> DepartmentLegalAttachments { get; set; }
        public virtual ICollection<DepartmentPolicyAttachment> DepartmentPolicyAttachments { get; set; }
        public virtual ICollection<DepartmentRegulatoryAttachment> DepartmentRegulatoryAttachments { get; set; }
        public virtual ICollection<DepartmentRequirmentAttachment> DepartmentRequirmentAttachments { get; set; }
        public virtual ICollection<DispatchGoAttachmentFile> DispatchGoAttachmentFiles { get; set; }
        public virtual ICollection<DispatchToAttachmentFile> DispatchToAttachmentFiles { get; set; }
        public virtual ICollection<DocumentAttachmentFile> DocumentAttachmentFiles { get; set; }
        public virtual ICollection<EquipmentMeasureAttachmentFile> EquipmentMeasureAttachmentFiles { get; set; }
        public virtual ICollection<EquipmentProductionAttachmentFile> EquipmentProductionAttachmentFiles { get; set; }
        public virtual ICollection<HumanEmployee> HumanEmployees { get; set; }
        public virtual ICollection<HumanProfileAttachment> HumanProfileAttachments { get; set; }
        public virtual ICollection<ProductDevelopmentRequirementAttachmentFile> ProductDevelopmentRequirementAttachmentFiles { get; set; }
        public virtual ICollection<ProfileAttachmentFile> ProfileAttachmentFiles { get; set; }
        public virtual ICollection<QualityMeetingAttachmentFile> QualityMeetingAttachmentFiles { get; set; }
        public virtual ICollection<QualityMeetingResultAttachmentFile> QualityMeetingResultAttachmentFiles { get; set; }
        public virtual ICollection<SupplierBidToAttachmentFile> SupplierBidToAttachmentFiles { get; set; }
        public virtual ICollection<Supplier> Suppliers { get; set; }
        public virtual ICollection<TaskAttachmentFile> TaskAttachmentFiles { get; set; }
        public virtual ICollection<TaskPerformAttachmentFile> TaskPerformAttachmentFiles { get; set; }
        public virtual ICollection<TemplateExport> TemplateExports { get; set; }
        public virtual ICollection<V3ProblemAttachmentFile> V3ProblemAttachmentFiles { get; set; }
    }
}
