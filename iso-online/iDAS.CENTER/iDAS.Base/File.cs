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
    
    public partial class File
    {
        public File()
        {
            this.Customers = new HashSet<Customer>();
            this.CustomerSystems = new HashSet<CustomerSystem>();
            this.Tutorials = new HashSet<Tutorial>();
            this.WebCustomerComments = new HashSet<WebCustomerComment>();
            this.WebNews = new HashSet<WebNew>();
            this.WebRecruimentProfiles = new HashSet<WebRecruimentProfile>();
            this.WebServiceItemFiles = new HashSet<WebServiceItemFile>();
            this.WebServiceStages = new HashSet<WebServiceStage>();
            this.WebSitemaps = new HashSet<WebSitemap>();
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
    
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<CustomerSystem> CustomerSystems { get; set; }
        public virtual ICollection<Tutorial> Tutorials { get; set; }
        public virtual ICollection<WebCustomerComment> WebCustomerComments { get; set; }
        public virtual ICollection<WebNew> WebNews { get; set; }
        public virtual ICollection<WebRecruimentProfile> WebRecruimentProfiles { get; set; }
        public virtual ICollection<WebServiceItemFile> WebServiceItemFiles { get; set; }
        public virtual ICollection<WebServiceStage> WebServiceStages { get; set; }
        public virtual ICollection<WebSitemap> WebSitemaps { get; set; }
    }
}
