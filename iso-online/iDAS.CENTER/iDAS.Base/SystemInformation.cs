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
    
    public partial class SystemInformation
    {
        public int ID { get; set; }
        public int CenterSystemID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsShow { get; set; }
        public string Icon { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<System.DateTime> PriceTime { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public int Order { get; set; }
    
        public virtual CenterSystem CenterSystem { get; set; }
    }
}