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
    
    public partial class RiskAudit
    {
        public int ID { get; set; }
        public int RiskControlID { get; set; }
        public bool IsAccept { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> AuditTime { get; set; }
        public Nullable<int> QualityNCID { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual QualityNC QualityNC { get; set; }
        public virtual RiskControl RiskControl { get; set; }
    }
}
