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
    
    public partial class HumanEmployee
    {
        public HumanEmployee()
        {
            this.Chats = new HashSet<Chat>();
            this.DispatchGoCCs = new HashSet<DispatchGoCC>();
            this.DispatchToCCs = new HashSet<DispatchToCC>();
            this.EquipmentMeasureCalibrations = new HashSet<EquipmentMeasureCalibration>();
            this.EquipmentProductionErrors = new HashSet<EquipmentProductionError>();
            this.EquipmentProductionHistories = new HashSet<EquipmentProductionHistory>();
            this.HumanAuditTicks = new HashSet<HumanAuditTick>();
            this.HumanEmployeeAudits = new HashSet<HumanEmployeeAudit>();
            this.HumanOrganizations = new HashSet<HumanOrganization>();
            this.HumanProfileAssesses = new HashSet<HumanProfileAssess>();
            this.HumanProfileAttachments = new HashSet<HumanProfileAttachment>();
            this.HumanProfileCertificates = new HashSet<HumanProfileCertificate>();
            this.HumanProfileContracts = new HashSet<HumanProfileContract>();
            this.HumanProfileCuriculmViates = new HashSet<HumanProfileCuriculmViate>();
            this.HumanProfileDiplomas = new HashSet<HumanProfileDiploma>();
            this.HumanProfileDisciplines = new HashSet<HumanProfileDiscipline>();
            this.HumanProfileInsurances = new HashSet<HumanProfileInsurance>();
            this.HumanProfileRelationships = new HashSet<HumanProfileRelationship>();
            this.HumanProfileRewards = new HashSet<HumanProfileReward>();
            this.HumanProfileSalaries = new HashSet<HumanProfileSalary>();
            this.HumanProfileTrainings = new HashSet<HumanProfileTraining>();
            this.HumanProfileWorkExperiences = new HashSet<HumanProfileWorkExperience>();
            this.HumanTrainingPractioners = new HashSet<HumanTrainingPractioner>();
            this.HumanUsers = new HashSet<HumanUser>();
            this.Notifies = new HashSet<Notify>();
            this.QualityAuditProgramEmployees = new HashSet<QualityAuditProgramEmployee>();
            this.Tasks = new HashSet<Task>();
            this.HumanProfileWorkTrials = new HashSet<HumanProfileWorkTrial>();
            this.TaskCCs = new HashSet<TaskCC>();
            this.TaskRequests = new HashSet<TaskRequest>();
        }
    
        public int ID { get; set; }
        public string Avatar { get; set; }
        public Nullable<System.Guid> AttachmentFileID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string WeddingStatus { get; set; }
        public bool Gender { get; set; }
        public bool IsNew { get; set; }
        public bool IsTrial { get; set; }
        public bool IsLeave { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    
        public virtual AttachmentFile AttachmentFile { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
        public virtual ICollection<DispatchGoCC> DispatchGoCCs { get; set; }
        public virtual ICollection<DispatchToCC> DispatchToCCs { get; set; }
        public virtual ICollection<EquipmentMeasureCalibration> EquipmentMeasureCalibrations { get; set; }
        public virtual ICollection<EquipmentProductionError> EquipmentProductionErrors { get; set; }
        public virtual ICollection<EquipmentProductionHistory> EquipmentProductionHistories { get; set; }
        public virtual ICollection<HumanAuditTick> HumanAuditTicks { get; set; }
        public virtual ICollection<HumanEmployeeAudit> HumanEmployeeAudits { get; set; }
        public virtual ICollection<HumanOrganization> HumanOrganizations { get; set; }
        public virtual ICollection<HumanProfileAssess> HumanProfileAssesses { get; set; }
        public virtual ICollection<HumanProfileAttachment> HumanProfileAttachments { get; set; }
        public virtual ICollection<HumanProfileCertificate> HumanProfileCertificates { get; set; }
        public virtual ICollection<HumanProfileContract> HumanProfileContracts { get; set; }
        public virtual ICollection<HumanProfileCuriculmViate> HumanProfileCuriculmViates { get; set; }
        public virtual ICollection<HumanProfileDiploma> HumanProfileDiplomas { get; set; }
        public virtual ICollection<HumanProfileDiscipline> HumanProfileDisciplines { get; set; }
        public virtual ICollection<HumanProfileInsurance> HumanProfileInsurances { get; set; }
        public virtual ICollection<HumanProfileRelationship> HumanProfileRelationships { get; set; }
        public virtual ICollection<HumanProfileReward> HumanProfileRewards { get; set; }
        public virtual ICollection<HumanProfileSalary> HumanProfileSalaries { get; set; }
        public virtual ICollection<HumanProfileTraining> HumanProfileTrainings { get; set; }
        public virtual ICollection<HumanProfileWorkExperience> HumanProfileWorkExperiences { get; set; }
        public virtual ICollection<HumanTrainingPractioner> HumanTrainingPractioners { get; set; }
        public virtual ICollection<HumanUser> HumanUsers { get; set; }
        public virtual ICollection<Notify> Notifies { get; set; }
        public virtual ICollection<QualityAuditProgramEmployee> QualityAuditProgramEmployees { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<HumanProfileWorkTrial> HumanProfileWorkTrials { get; set; }
        public virtual ICollection<TaskCC> TaskCCs { get; set; }
        public virtual ICollection<TaskRequest> TaskRequests { get; set; }
    }
}