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
    
    public partial class TaskCheck
    {
        public int ID { get; set; }
        public int TaskID { get; set; }
        public int EmployeeID { get; set; }
        public System.DateTime Time { get; set; }
        public decimal Rate { get; set; }
        public bool IsPass { get; set; }
        public string Feedback { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual Task Task { get; set; }
    }
}
