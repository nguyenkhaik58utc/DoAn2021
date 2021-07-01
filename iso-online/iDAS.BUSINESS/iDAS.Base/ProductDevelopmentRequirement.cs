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
    
    public partial class ProductDevelopmentRequirement
    {
        public ProductDevelopmentRequirement()
        {
            this.ProductDevelopmentRequirementAttachmentFiles = new HashSet<ProductDevelopmentRequirementAttachmentFile>();
            this.ProductDevelopmentRequirementPlans = new HashSet<ProductDevelopmentRequirementPlan>();
        }
    
        public int ID { get; set; }
        public int StockProductID { get; set; }
        public Nullable<int> DevelopmentFromProduct { get; set; }
        public string Reason { get; set; }
        public string Contents { get; set; }
        public bool IsWork { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual ICollection<ProductDevelopmentRequirementAttachmentFile> ProductDevelopmentRequirementAttachmentFiles { get; set; }
        public virtual ICollection<ProductDevelopmentRequirementPlan> ProductDevelopmentRequirementPlans { get; set; }
        public virtual StockProduct StockProduct { get; set; }
    }
}
