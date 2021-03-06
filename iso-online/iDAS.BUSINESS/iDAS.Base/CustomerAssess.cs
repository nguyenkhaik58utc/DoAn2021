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
    
    public partial class CustomerAssess
    {
        public CustomerAssess()
        {
            this.CustomerAssessObjects = new HashSet<CustomerAssessObject>();
            this.CustomerAssessResults = new HashSet<CustomerAssessResult>();
        }
    
        public int ID { get; set; }
        public Nullable<int> AuditID { get; set; }
        public Nullable<int> CustomerCriteriaCategoryID { get; set; }
        public string Name { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public bool IsActive { get; set; }
        public string Method { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    
        public virtual Audit Audit { get; set; }
        public virtual CustomerCriteriaCategory CustomerCriteriaCategory { get; set; }
        public virtual ICollection<CustomerAssessObject> CustomerAssessObjects { get; set; }
        public virtual ICollection<CustomerAssessResult> CustomerAssessResults { get; set; }
    }
}
