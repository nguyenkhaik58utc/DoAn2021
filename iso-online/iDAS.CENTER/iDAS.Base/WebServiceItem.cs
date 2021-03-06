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
    
    public partial class WebServiceItem
    {
        public WebServiceItem()
        {
            this.CustomerRegisterServices = new HashSet<CustomerRegisterService>();
            this.WebServiceComments = new HashSet<WebServiceComment>();
            this.WebServiceItemFiles = new HashSet<WebServiceItemFile>();
        }
    
        public int ID { get; set; }
        public int WebServiceID { get; set; }
        public Nullable<System.Guid> FileID { get; set; }
        public string Name { get; set; }
        public string Html { get; set; }
        public string Tags { get; set; }
        public string Address { get; set; }
        public string Duration { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Note { get; set; }
        public bool IsFirst { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> StartAt { get; set; }
        public Nullable<System.DateTime> EndAt { get; set; }
        public Nullable<System.DateTime> RefreshAt { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual ICollection<CustomerRegisterService> CustomerRegisterServices { get; set; }
        public virtual ICollection<WebServiceComment> WebServiceComments { get; set; }
        public virtual ICollection<WebServiceItemFile> WebServiceItemFiles { get; set; }
        public virtual WebService WebService { get; set; }
    }
}
