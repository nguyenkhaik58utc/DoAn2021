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
    
    public partial class HumanAuditGradationCriteria
    {
        public HumanAuditGradationCriteria()
        {
            this.HumanAuditGradationCriteriaPoints = new HashSet<HumanAuditGradationCriteriaPoint>();
        }
    
        public int ID { get; set; }
        public int HumanAuditGradationRoleID { get; set; }
        public string HumanAuditCriteriaCategoryName { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int Factory { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual ICollection<HumanAuditGradationCriteriaPoint> HumanAuditGradationCriteriaPoints { get; set; }
        public virtual HumanAuditGradationRole HumanAuditGradationRole { get; set; }
    }
}