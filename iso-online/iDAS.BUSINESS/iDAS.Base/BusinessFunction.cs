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
    
    public partial class BusinessFunction
    {
        public int ID { get; set; }
        public string SystemCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ModuleCode { get; set; }
        public string ParentCode { get; set; }
        public bool IsActive { get; set; }
        public bool IsShow { get; set; }
        public bool IsDelete { get; set; }
        public bool IsGroup { get; set; }
        public int Position { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    }
}
