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
    
    public partial class CustomerCategory
    {
        public CustomerCategory()
        {
            this.CustomerAssessObjects = new HashSet<CustomerAssessObject>();
            this.CustomerCareObjects = new HashSet<CustomerCareObject>();
            this.CustomerCategoryDepartments = new HashSet<CustomerCategoryDepartment>();
            this.CustomerCategoryCustomers = new HashSet<CustomerCategoryCustomer>();
            this.CustomerSurveyObjects = new HashSet<CustomerSurveyObject>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual ICollection<CustomerAssessObject> CustomerAssessObjects { get; set; }
        public virtual ICollection<CustomerCareObject> CustomerCareObjects { get; set; }
        public virtual ICollection<CustomerCategoryDepartment> CustomerCategoryDepartments { get; set; }
        public virtual ICollection<CustomerCategoryCustomer> CustomerCategoryCustomers { get; set; }
        public virtual ICollection<CustomerSurveyObject> CustomerSurveyObjects { get; set; }
    }
}