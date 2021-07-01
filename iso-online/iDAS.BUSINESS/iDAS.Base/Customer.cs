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
    
    public partial class Customer
    {
        public Customer()
        {
            this.CustomerCategoryCustomers = new HashSet<CustomerCategoryCustomer>();
            this.CustomerContactCalendars = new HashSet<CustomerContactCalendar>();
            this.CustomerContactHistories = new HashSet<CustomerContactHistory>();
            this.CustomerContacts = new HashSet<CustomerContact>();
            this.CustomerContracts = new HashSet<CustomerContract>();
            this.CustomerFeedbacks = new HashSet<CustomerFeedback>();
            this.CustomerOrders = new HashSet<CustomerOrder>();
            this.ServicePlanStages = new HashSet<ServicePlanStage>();
        }
    
        public int ID { get; set; }
        public int CustomerTypeID { get; set; }
        public string Name { get; set; }
        public string TaxCode { get; set; }
        public string RepresentName { get; set; }
        public string RepresentRole { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsNew { get; set; }
        public bool IsPotential { get; set; }
        public bool IsOfficial { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsPotentialView { get; set; }
        public bool IsNotContract { get; set; }
        public Nullable<System.DateTime> TimeNormal { get; set; }
        public Nullable<System.DateTime> TimePotential { get; set; }
        public Nullable<System.DateTime> TimeOfficial { get; set; }
        public string RequireContent { get; set; }
        public Nullable<System.DateTime> RequireTime { get; set; }
        public Nullable<System.DateTime> EstablishDate { get; set; }
        public string Scope { get; set; }
        public string Address { get; set; }
        public Nullable<System.Guid> AttachmentFileID { get; set; }
        public string Note { get; set; }
        public string EditFields { get; set; }
        public Nullable<int> SuccessRate { get; set; }
        public string ReasonNotContract { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual ICollection<CustomerCategoryCustomer> CustomerCategoryCustomers { get; set; }
        public virtual ICollection<CustomerContactCalendar> CustomerContactCalendars { get; set; }
        public virtual ICollection<CustomerContactHistory> CustomerContactHistories { get; set; }
        public virtual ICollection<CustomerContact> CustomerContacts { get; set; }
        public virtual ICollection<CustomerContract> CustomerContracts { get; set; }
        public virtual ICollection<CustomerFeedback> CustomerFeedbacks { get; set; }
        public virtual ICollection<CustomerOrder> CustomerOrders { get; set; }
        public virtual ICollection<ServicePlanStage> ServicePlanStages { get; set; }
    }
}
