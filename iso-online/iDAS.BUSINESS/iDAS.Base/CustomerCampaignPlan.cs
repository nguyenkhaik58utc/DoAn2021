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
    
    public partial class CustomerCampaignPlan
    {
        public int ID { get; set; }
        public int QualityPlanID { get; set; }
        public int CustomerCampaignID { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    
        public virtual CustomerCampaign CustomerCampaign { get; set; }
        public virtual QualityPlan QualityPlan { get; set; }
    }
}
