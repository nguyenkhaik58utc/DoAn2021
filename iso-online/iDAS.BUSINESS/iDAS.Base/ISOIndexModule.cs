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
    
    public partial class ISOIndexModule
    {
        public int ID { get; set; }
        public int ISOIndexID { get; set; }
        public int CenterModuleID { get; set; }
        public bool IsUse { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    
        public virtual CenterModule CenterModule { get; set; }
        public virtual ISOIndex ISOIndex { get; set; }
    }
}
