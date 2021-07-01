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
    
    public partial class HumanRecruitmentRequirement
    {
        public HumanRecruitmentRequirement()
        {
            this.HumanRecruitmentPlanRequirements = new HashSet<HumanRecruitmentPlanRequirement>();
            this.HumanRecruitmentProfileInterviews = new HashSet<HumanRecruitmentProfileInterview>();
        }
    
        public int ID { get; set; }
        public int HumanRoleID { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public System.DateTime DateRequired { get; set; }
        public string Form { get; set; }
        public string Reason { get; set; }
        public bool IsEdit { get; set; }
        public bool IsApproval { get; set; }
        public bool IsAccept { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> ApprovalAt { get; set; }
        public Nullable<int> ApprovalBy { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    
        public virtual ICollection<HumanRecruitmentPlanRequirement> HumanRecruitmentPlanRequirements { get; set; }
        public virtual ICollection<HumanRecruitmentProfileInterview> HumanRecruitmentProfileInterviews { get; set; }
        public virtual HumanRole HumanRole { get; set; }
    }
}
