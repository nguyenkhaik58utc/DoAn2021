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
    
    public partial class HumanAbsent
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public int HumantAbsentTypeID { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public Nullable<int> DayNumber { get; set; }
        public string Contents { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<bool> IsApproval { get; set; }
        public Nullable<System.DateTime> ApprovalAt { get; set; }
        public Nullable<int> ApprovalBy { get; set; }
        public string ApprovalNote { get; set; }
        public Nullable<bool> IsEdit { get; set; }
        public Nullable<bool> IsAccept { get; set; }
        public Nullable<bool> HaftDay { get; set; }
    
        public virtual HumanAbsentType HumanAbsentType { get; set; }
    }
}